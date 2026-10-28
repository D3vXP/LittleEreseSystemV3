# POLISH list - every `// POLISH:` comment in the project

A `POLISH` comment marks a place where the owner (or a future developer) may want to adjust a
word, a limit, a rule or a colour. Nothing here is broken; these are the "you could tweak this"
spots. Line numbers refer to the final version of each file.

Total: **26 comments** in **12 files**.

## Controls\

| File | Line | Text |
|---|---|---|
| Controls\ucCalendarMonth.cs | 29 | move BusyFrom if the owner wants the amber warning earlier or later. |
| Controls\ucDashboard.cs | 146 | the heading follows the prompt wording; shorten it here if the owner prefers. |
| Controls\ucSettings.cs | 49 | change the "-" placeholder if you prefer an empty line. |
| Controls\ucSettings.cs | 65 | use ToString() instead of ToString(3) to also show the build number. |

## Data\

| File | Line | Text |
|---|---|---|
| Data\DemoDataSeeder.cs | 22 | set to false before the shop uses the system for real. |
| Data\DemoDataSeeder.cs | 25 | the owner can change these demo passwords here (or delete the accounts in Staff Accounts). |
| Data\DemoDataSeeder.cs | 89 | rename these demo staff accounts to the real employees in Staff Accounts. |
| Data\DemoDataSeeder.cs | 167 | replace these placeholder guests with the real client list before the defense. |
| Data\DemoDataSeeder.cs | 343 | the catalogue has no multi-session laser service yet, so the demo uses the other multi-session service it finds (today: "Facial test", 8 sessions). |
| Data\DemoDataSeeder.cs | 1182 | link the real rooms on the Rooms screen and this fallback is never used again. |
| Data\MaintenanceRepository.cs | 187 | switch to CreationTime if the owner prefers that. |
| Data\ReportRepository.cs | 10 | sales are counted when the service is completed (not when money is received), so a period can show sales that were only paid for later. Ask the owner if the cash-basis view is the one she wants. |
| Data\ReportRepository.cs | 137 | "Rescheduled" counts the moves MADE in the period (changed_at), not the appointments that sit on a rescheduled date. Confirm which one the owner reads. |

## Forms\

| File | Line | Text |
|---|---|---|
| Forms\LoginForm.cs | 11 | the owner may want a different number of tries or a different waiting time. |
| Forms\PackageForm.cs | 109 | change this split if staff should also see single-session services as packages. |
| Forms\StaffEditForm.cs | 255 | relax this if the shop also allows landline numbers. |

## Helpers\

| File | Line | Text |
|---|---|---|
| Helpers\GridPrinter.cs | 16 | the paper header can be changed here. |
| Helpers\ReceiptPrinter.cs | 18 | change the receipt header text here if the shop's address changes. |
| Helpers\Validators.cs | 13 | The Philippine mobile rule below follows the usual 09XXXXXXXXX and +639XXXXXXXXX shapes. |
| Helpers\Validators.cs | 14 | Add or remove digits here if the shop later accepts landlines or foreign numbers. |
| Helpers\Validators.cs | 17 | 3-20 letters, digits or underscore. Tighten it if usernames must never contain "_". |
| Helpers\Validators.cs | 20 | 2-50 characters: letters, spaces, dots, apostrophes and hyphens (for names like "Ma. Cruz-Santos"). |
| Helpers\Validators.cs | 73 | InvariantCulture means "1234.50"; switch to TryParse(value, out amount) if staff type commas. |
| Helpers\Validators.cs | 84 | same InvariantCulture note as IsPositiveMoney above. |
| Helpers\Validators.cs | 106 | widen the length range or allow "-" if a bank starts using longer or dashed reference numbers; the payment screens use this one rule. |
| Helpers\Validators.cs | 107 | reference numbers; the payment screens use this one rule. (second line of the comment that starts on 106) |

## Where to look first

1. **Data\DemoDataSeeder.cs line 22** - turn `EnableDemoSeed` off before the shop uses the system for
   real, otherwise a brand-new empty database fills itself with demo clients and bookings.
2. **Helpers\Validators.cs** - every input rule of the whole system lives in this one file.
3. **Data\ReportRepository.cs lines 10 and 137** - the two counting decisions that change the
   numbers in the Reports module (cash basis vs. completed basis, reschedule moves vs. dates).
4. **Helpers\ReceiptPrinter.cs line 18** - the printed receipt header carries the shop's name and
   address.
