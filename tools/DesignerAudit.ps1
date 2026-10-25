# DesignerAudit.ps1 - checks every *.Designer.cs in Forms\ and Controls\.
#
# WHY: a Designer file that the Visual Studio designer can no longer read gives the famous
# "yellow screen" (the layout cannot be opened) and a click that does nothing is very hard to
# find by hand. This script looks for the six mistakes that cause exactly those problems.
#
# HOW TO RUN:  powershell -ExecutionPolicy Bypass -File tools\DesignerAudit.ps1
# It prints one line per problem and a summary, and exits with code 1 when anything is wrong.
# It only READS files; it never changes them and it is not part of the .csproj.

$ErrorActionPreference = 'Stop'

$root = Split-Path -Parent $PSScriptRoot
$uiFolders = @('Forms', 'Controls')

$problems = New-Object System.Collections.Generic.List[string]
$filesChecked = 0

# Control name prefixes from PROJECT_RULES. Only a "this.<prefix>..." assignment is expected to
# have a field declaration; "this.ClientSize = new ..." sets a property of the form itself.
$controlPrefixes = @('btn', 'txt', 'lbl', 'cbo', 'dgv', 'dtp', 'chk', 'pnl', 'pic', 'tab', 'lst', 'nud', 'rdo', 'grp', 'tlp', 'flp', 'cal', 'chk', 'prg', 'spl', 'tt')

function Add-Problem {
    param([string]$File, [int]$Line, [string]$Kind, [string]$Text)
    $where = if ($Line -gt 0) { "$File" + ":" + $Line } else { $File }
    $problems.Add("$where  [$Kind] $Text")
}

# Field names declared at the bottom of a Designer file (private System.Windows.Forms.Button btnSave;).
function Get-DeclaredFields {
    param([string[]]$Lines)
    $names = @()
    $lineNo = 0
    foreach ($line in $Lines) {
        $lineNo++
        if ($line -match '^\s*(?:private|protected|internal|public)\s+[A-Za-z_][\w\.]*(?:<[^>]*>)?(?:\[\])?\s+([A-Za-z_]\w*)\s*;') {
            $names += $Matches[1]
        }
    }
    return $names
}

# Removes the quoted text from a line, so words inside a caption ("... on the first session.")
# are never mistaken for code.
function Get-CodePart {
    param([string]$Line)
    return [regex]::Replace($Line, '"(\\.|[^"\\])*"', '""')
}

# True when the sibling .cs file really defines a method with that name.
function Test-HandlerExists {
    param([string]$CodeFile, [string]$Handler)
    if (-not (Test-Path -LiteralPath $CodeFile)) { return $false }
    $text = Get-Content -LiteralPath $CodeFile -Raw
    if ($text -eq $null) { return $false }

    $escaped = [regex]::Escape($Handler)

    # private void btnSave_Click(object sender, EventArgs e)
    if ($text -match "(?m)^\s*(?:\[[^\]]*\]\s*)*(?:private|public|protected|internal|static|virtual|override|sealed|async|partial|new|\s)+[\w\.<>\[\]]+\s+$escaped\s*\(")  { return $true }
    # a handler without an explicit modifier on the same line as the return type
    if ($text -match "(?m)^\s*[\w\.<>\[\]]+\s+$escaped\s*\(\s*object\s")  { return $true }

    return $false
}

foreach ($folder in $uiFolders) {
    $path = Join-Path $root $folder
    if (-not (Test-Path -LiteralPath $path)) { continue }

    $designerFiles = Get-ChildItem -LiteralPath $path -Filter '*.Designer.cs' -File -Recurse
    foreach ($file in $designerFiles) {
        $filesChecked++
        $relative = $file.FullName.Substring($root.Length).TrimStart('\')
        $lines = Get-Content -LiteralPath $file.FullName
        $text = ($lines -join "`n")

        # The code-behind next to the Designer file (ucClients.Designer.cs -> ucClients.cs).
        $codeFile = $file.FullName -replace '\.Designer\.cs$', '.cs'

        $declared = Get-DeclaredFields -Lines $lines

        # (d) the three parts the Visual Studio designer insists on.
        $initCount = ([regex]::Matches($text, 'void\s+InitializeComponent\s*\(\s*\)')).Count
        if ($initCount -ne 1) {
            Add-Problem $relative 0 'structure' "expected exactly one InitializeComponent, found $initCount"
        }

        $hasComponents = $text -match '(?:private|protected|internal|public)?\s*System\.ComponentModel\.IContainer\s+components\b'
        if (-not $hasComponents) {
            Add-Problem $relative 0 'structure' 'the components field is missing'
        }

        if ($text -notmatch 'override\s+void\s+Dispose\s*\(\s*bool\s+disposing\s*\)') {
            Add-Problem $relative 0 'structure' 'Dispose(bool disposing) is missing'
        }

        if (-not (Test-Path -LiteralPath $codeFile)) {
            Add-Problem $relative 0 'structure' "the code-behind file is missing ($([System.IO.Path]::GetFileName($codeFile)))"
        }

        $lineNo = 0
        foreach ($line in $lines) {
            $lineNo++
            $code = Get-CodePart -Line $line

            # (a) every "this.X = new" needs a field declaration.
            if ($code -match 'this\.([a-z][A-Za-z0-9_]*)\s*=\s*new\s') {
                $name = $Matches[1]
                $isControl = $false
                foreach ($prefix in $controlPrefixes) {
                    if ($name.StartsWith($prefix, [System.StringComparison]::Ordinal)) { $isControl = $true; break }
                }
                if ($isControl -and $declared -notcontains $name) {
                    Add-Problem $relative $lineNo 'field' "this.$name = new ... but no field declaration for $name"
                }
            }

            # (c) every wired handler needs a method with that name in the code-behind.
            if ($code -match '\+=\s*new\s+[A-Za-z_][\w\.]*\s*\(\s*this\.([A-Za-z_]\w*)\s*\)') {
                $handler = $Matches[1]
                if (-not (Test-HandlerExists -CodeFile $codeFile -Handler $handler)) {
                    Add-Problem $relative $lineNo 'event' "wired handler this.$handler has no method $handler in the code-behind"
                }
            }

            # (e) nothing else may live in a Designer file.
            if ($code -cmatch '\bImage\s*=[^=]') {
                Add-Problem $relative $lineNo 'content' 'Image = is not allowed (images load at runtime through IconHelper)'
            }
            if ($code -cmatch '(?<![\w])Db\.[A-Za-z_]') {
                Add-Problem $relative $lineNo 'content' 'a database call (Db.) is not allowed in a Designer file'
            }
            if ($code -cmatch '(?<![\w])Session\.[A-Za-z_]') {
                Add-Problem $relative $lineNo 'content' 'Session access is not allowed in a Designer file'
            }
            if ($code -cmatch 'System\.Data\.SqlClient|\bSqlConnection\b|\bSqlCommand\b') {
                Add-Problem $relative $lineNo 'content' 'SQL code is not allowed in a Designer file'
            }
            if ($code -cmatch '(?<![\w])[A-Za-z_]\w*Repository\.[A-Za-z_]') {
                Add-Problem $relative $lineNo 'content' 'a repository call is not allowed in a Designer file'
            }
        }

        # (b) every declared control field is instantiated in InitializeComponent.
        foreach ($name in $declared) {
            if ($text -notmatch ("this\." + [regex]::Escape($name) + "\s*=\s*new\s")) {
                Add-Problem $relative 0 'field' "the field $name is declared but never instantiated"
            }
        }
    }
}

Write-Output ''
foreach ($problem in $problems) { Write-Output $problem }

Write-Output ''
if ($problems.Count -eq 0) {
    Write-Output "SUMMARY: DesignerAudit found no problems in $filesChecked Designer file(s)."
    exit 0
}

Write-Output "SUMMARY: DesignerAudit found $($problems.Count) problem(s) in $filesChecked Designer file(s)."
exit 1
