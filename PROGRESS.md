# PROGRESS.md

Update this file at the end of every task.

- Step 14 (this task): the LOGIC for the Clients module (repository + page + the two dialogs).
  The Designer layout was not restructured; two small runtime fixes are listed at the end.
  - Data\ClientRepository.cs (was an empty class): Search(text, consent) returns client_id,
    full_name ("Last, First"), contact_number, birth_date, gender, consent_text ('Signed' /
    'Pending' from consent_status) and last_visit (MAX appointment_date of a Completed
    appointment, NULL when there is none); text matches first name, last name, "Last, First" or
    the contact number; consent is All | Signed | Not signed. GetById (full row for the card),
    GetForCombo() (client_id + display_name "Last, First - contact", for the booking screen
    later), Add (returns the new client_id), Update (stamps updated_at), ContactExists(contact,
    excludeId), GetPackages(clientId) (package_id, service_name, sessions_label "3 / 8",
    expiry_date, status; client 0 = empty table), AddPackage(clientId, serviceId, totalSessions,
    purchaseDate, DateTime? expiry) (returns the new package_id, status 'Active'),
    HasActivePackage(clientId, serviceId) and ExpireOldPackages() (Active + expiry_date < today
    -> 'Expired', returns the row count). Every optional text box is stored as NULL, never as "".
    Clients are never deleted.
  - Controls\ucClients.cs: Load styles both grids and the four buttons with Theme, starts the
    consent filter on "All", calls ExpireOldPackages() and then loads the grid. Selecting a row
    fills the card from GetById: name ("Last, First"), contact, birth date + age, address +
    gender, the consent badge (green "Consent signed" / red "Consent form not signed"), the three
    read-only notes boxes and the packages grid. No selection (or a row that is no longer there)
    clears the card and switches off Edit / Add Package. Search, the consent filter and Refresh
    reload (a _loading flag stops the events from firing twice); Add and Edit open ClientEditForm
    with EditId set and re-select the saved row through GridHelper.SelectRowById; a double click
    is Edit. Add Package opens PackageForm with ClientId set and re-reads the card afterwards so
    the new package shows immediately. CellFormatting: the consent cell is Success (Signed) or
    Warning (Pending), the package Status cell uses Theme.StatusColor; both keep white text on a
    selected row. Staff may add and edit (no delete exists); Add / Edit / Add Package are logged.
  - Forms\ClientEditForm.cs: public EditId (0 = new) and SavedClientId for the caller. Load
    styles the buttons, clears the gender pick and the birth date tick, loads the row when
    EditId > 0 (title becomes "Edit Client") and refreshes the age. dtpBirthDate_ValueChanged
    updates lblAge ("Age: -" while the tick is off). Save validates with Validators +
    errorProvider1 (first and last name via IsValidName, contact via IsValidPhone, birth date not
    in the future and not more than 120 years ago when it is ticked, gender chosen) and asks
    Messages.Confirm("A client with this contact number already exists. Save anyway?") when
    ContactExists is true. Add / Update go through the repository and log "Add" / "Edit".
  - Forms\PackageForm.cs: public ClientId and SavedPackageId. Load shows the client name, fills
    cboService from ServiceRepository.GetActiveForBooking() with "Service - price" (services with
    default_total_sessions > 1 first, all Active services as a fallback when none is flagged as a
    package - POLISH comment left), and sets the purchase date to today with the expiry tick off.
    cboService_SelectedIndexChanged sets nudTotalSessions to the service's default_total_sessions
    and updates lblPriceValue (peso format). Save validates (a client is chosen, a service is
    chosen, a ticked expiry date must be after the purchase date), asks
    Messages.Confirm("This client already has an active package for this service. Add another?")
    when HasActivePackage is true, saves through AddPackage and logs "Add Package".
  - Helpers\SmokeTest.cs: the three read-only checks Search("", "All"), GetForCombo() and
    GetPackages(0) returns an empty table. Nothing is written.
  - Runtime fixes found in the previews (2 rounds, Designer files untouched):
    (1) in ClientEditForm the birth date picker showed a date although it was meant to be empty -
    the value is now set BEFORE the tick is cleared (assigning Value ticks an unchecked picker),
    and lblAge now shows "Age: -"; (2) the same order was applied to the PackageForm expiry picker
    (it is still unchecked in the preview, as intended). The Designer files were NOT edited.
  - LittleEreseSystemV3.csproj was NOT edited (no new file in this task).
- Standard clean build (bin, obj and .vs deleted first, MSBuild /t:Rebuild): 0 errors, 0 warnings.
  --smoketest: 41 passed, 0 failed, 0 skipped (the 3 new Clients checks all PASS).
  --preview checked for ucClients (toolbar, count "0 clients", the 7-column grid and the details
  card with the "Select a client" placeholder, the gray "No client selected" badge and the
  packages grid - nothing overlapping or truncated), ClientEditForm (empty birth date with
  "Age: -", no truncation, footer complete) and PackageForm (client "-", the service list with
  prices, the package price, 8 sessions and the unchecked expiry box). The app was started once
  and closed again (no error.log).

- Step 13 (this task): Layout only (Designer work) for the Clients page and its two dialogs.
  No logic, no SQL, no file or Session access: every wired event got an empty handler with
  "// TODO(logic prompt)", waiting for the logic prompt.
  - Controls\ucClients (.Designer.cs + .cs): the page is now a SplitContainer splitMain
    (Orientation Vertical, Panel1MinSize 400, Panel2MinSize 320, SplitterDistance 619 = about 62%
    of the 1000 px design width) on the AppBack background.
    LEFT panel (Panel1): pnlToolbar (Dock Top, 92 px, Surface, two rows built with tlpToolbar) -
    row 1 holds flpFilters (lblSearch + txtSearch 140 px, lblConsent + cboConsent 102 px with
    items All / Signed / Not signed) and the right-aligned lblCount ("0 clients", SmallFont,
    TextMuted); row 2 holds the right-aligned flpButtons (btnAdd "Add Client" primary,
    btnEdit "Edit" secondary, btnRefresh "Refresh" secondary). Below it the card pnlGrid
    (Dock Fill, Surface, FixedSingle, 1 px padding) with dgvClients (Dock Fill, Theme.StyleGrid
    look copied as literals, AutoGenerateColumns = false). Columns: colClientId (hidden,
    client_id), colName "Client" / full_name, colContact "Contact" / contact_number,
    colBirth "Birth Date" / birth_date (MMM dd, yyyy), colGender "Gender" / gender,
    colConsent "Consent" / consent_text, colLastVisit "Last Visit" / last_visit (MMM dd, yyyy).
    RIGHT panel (Panel2): the scrollable details card pnlDetails (Dock Fill, Surface, FixedSingle,
    AutoScroll, 16 px padding) holding the white tlpDetails (Dock Top, AutoSize) - lblDetailName
    (HeadingFont, placeholder "Select a client"), lblDetailContact, lblDetailBirth (birth date and
    age), lblDetailAddress, the badge-like lblDetailConsent (Semibold 9 pt, TextMuted, 10/4 px
    padding), lblAllergiesTitle "Allergies" + txtDetailAllergies (read-only, multiline),
    lblMedicalTitle "Medical history" + txtDetailMedical (read-only, multiline),
    lblTreatmentTitle "Treatment history (last 6 months)" + txtDetailTreatment (read-only,
    multiline), lblPackagesTitle "Packages" + dgvPackages (colPackageId hidden / package_id,
    colPackageService "Service" / service_name, colPackageSessions "Sessions" / sessions_label,
    colPackageExpiry "Expires" / expiry_date, colPackageStatus "Status" / status) and the
    right-aligned btnAddPackage "Add Package" (secondary).
    Events wired: Load, btnAdd_Click, btnEdit_Click, btnRefresh_Click, btnAddPackage_Click,
    txtSearch_TextChanged, cboConsent_SelectedIndexChanged, dgvClients_CellDoubleClick,
    dgvClients_SelectionChanged, dgvClients_CellFormatting, dgvPackages_CellFormatting.
  - Forms\ClientEditForm (.Designer.cs): the existing dialog was kept (560 x 640 client,
    FixedDialog, CenterParent, AcceptButton btnSave, CancelButton + DialogResult.Cancel on
    btnCancel, errorProvider1, white header + footer, AutoScroll content). Only the vertical
    rhythm was tightened so the whole form fits 768 px screens without scrolling: the fields now
    run from y = 31 (txtFirstName) to y = 424 (lblTreatmentHistory), the three multiline boxes are
    46 px tall and chkConsentSigned sits at y = 492 above the footer. Every control name, TabIndex,
    event wiring and the "Age: -" label beside the birth date are unchanged.
  - Forms\PackageForm (.Designer.cs): unchanged in this task (460 x 440, same dialog settings,
    lblClient + lblClientName, lblService + cboService (DropDownList), lblPrice + lblPriceValue,
    lblSessions + nudTotalSessions (1..30), lblPurchase + dtpPurchaseDate, lblExpiry +
    dtpExpiryDate (ShowCheckBox, unchecked), the muted lblHint and the footer). The preview was
    taken again to confirm it is still correct.
  - Fix rounds from the previews (2 used): (1) the toolbar controls were measured against the real
    619 px left panel - the four filter controls plus the count label and the three buttons cannot
    fit on one 619 px line without overlapping, so the buttons were moved to a second toolbar row
    (the toolbar is 92 px tall now) and the details card was re-laid out to start at 16,16 with
    48 px text boxes; (2) the ClientEditForm fields still ran past the footer, so the whole form
    was re-spaced (see above) and the AppBack details panel got a 1 px border so it reads as a card.
  - LittleEreseSystemV3.csproj was NOT edited (all four files already existed and were listed).
- Standard clean build (bin, obj and .vs deleted first): 0 errors, 0 warnings.
  --smoketest: 38 passed, 0 failed, 0 skipped.
  --preview checked for ucClients (split 62/38, toolbar and count on one card, 7 grid columns,
  details card complete with the packages grid and "Add Package", nothing overlapping or
  truncated), ClientEditForm (all fields plus the consent tick above the footer, nothing hidden
  under the scrollbar) and PackageForm (unchanged, unchecked expiry box, hint and footer visible).
  The app was started once and closed again.

- Step 12 (this task): Layout only (Designer work) for the Clients and Packages dialogs.
  No logic, no SQL, no file or Session access: every wired event got an empty handler with
  "// TODO(logic prompt)", waiting for the logic prompt.
  - Forms\ClientEditForm (.Designer.cs + .cs): the dialog is now 560 x 640 client (the prompt's
    560 x 760 was shrunk to satisfy the "dialogs at most 1000 x 640" rule of Additions (Step 3)),
    FixedDialog, CenterParent, no minimize/maximize, AcceptButton btnSave, CancelButton +
    DialogResult.Cancel on btnCancel, errorProvider1. White header with lblTitle "Client"
    (HeadingFont) + 1 px Border line, AppBack content and a white footer with btnSave (primary)
    and btnCancel (secondary). pnlContent has AutoScroll = true, so the fields that do not fit
    on a 768 px screen can be reached with the scrollbar. Fields, top to bottom = TabIndex order:
    lblFirstName + txtFirstName, lblLastName + txtLastName, lblBirthDate + dtpBirthDate
    (ShowCheckBox true, Format Short) with lblAge ("Age: -") to its right, lblGender + cboGender
    (DropDownList, Female / Male / Other), lblContact + txtContact, lblAddress + txtAddress,
    lblAllergies + txtAllergies (multiline 60 px, vertical scrollbar), lblMedicalHistory +
    txtMedicalHistory (multiline 60 px) and lblTreatmentHistory ("Treatments received in the last
    6 months") + txtTreatmentHistory (multiline 60 px), then chkConsentSigned
    "Consent / waiver form signed (paper form)". Events wired: Load, btnSave_Click,
    btnCancel_Click, dtpBirthDate_ValueChanged.
  - Forms\PackageForm (.Designer.cs + .cs): client 460 x 440 (as in the prompt), same dialog
    settings. lblTitle "Add Package", lblClient + lblClientName (read-only text, bold, "-" until
    the logic prompt), lblService + cboService (DropDownList), lblPrice + lblPriceValue (bold,
    "-"), lblSessions + nudTotalSessions (Min 1, Max 30, value 1), lblPurchase + dtpPurchaseDate,
    lblExpiry + dtpExpiryDate (ShowCheckBox true, Checked = false in the Designer so the preview
    shows the unchecked state) and the muted lblHint "The package price is charged on the first
    session." above the footer. Events wired: Load, btnSave_Click, btnCancel_Click,
    cboService_SelectedIndexChanged.
  - Fix rounds from the previews (2 used): (1) the smoke test failed with "Unable to cast
    DateTimePicker to ISupportInitialize" - the Designer called BeginInit/EndInit on the two
    DateTimePicker controls, which do not implement ISupportInitialize; those four lines were
    removed (NumericUpDown keeps its BeginInit/EndInit). (2) In ClientEditForm the 528 px wide
    fields and labels overlapped the AutoScroll vertical scrollbar, so they were narrowed to
    520 px (and lblAge to 304 px); the "Treatments received in the last 6 months" label is now
    fully visible at 560 x 640.
  - LittleEreseSystemV3.csproj was NOT edited (both files already existed and were listed).
- Standard clean build (bin, obj and .vs deleted first): 0 errors, 0 warnings.
  --smoketest: 38 passed, 0 failed, 0 skipped.
  --preview checked for ClientEditForm (scrollable content, no overlap, "Age: -" beside the birth
  date, nothing truncated) and PackageForm (all rows inside 460 x 440, unchecked expiry box,
  hint and footer visible). The app was started once and closed again.

- Step 11 (this task): the LOGIC for promotion-to-services (repository + the two screens + schema
  docs). No SQL was run by me: the PromotionServices table already existed in LittleEresesDB.
  - Data\PromotionRepository.cs: every old method still works (RefreshExpired, GetPromotions,
    GetById, GetActiveValid, PromoNameExists, Add, Update, SetStatus). Added:
    * ComputeDiscount(discountType, value, eligibleSubtotal) - the ONLY discount formula.
      Percentage = ROUND(subtotal x value / 100, 2); Fixed = MIN(value, subtotal); never negative
      and never above the subtotal.
    * GetServiceIds(promoId) - the linked service ids; empty list = applies to all (promoId 0 =
      empty).
    * GetPickList(promoId) - every Active service with service_id, service_name, category_name,
      price, is_selected (true when linked to promoId), ordered by category then name.
    * SavePromotion(...) - add/update the promotion AND replace its links in ONE transaction built
      privately in this class with its own SqlConnection + SqlTransaction (no other transaction
      helper). The unique-name check also runs inside that transaction; duplicate ids are skipped
      so the UNIQUE constraint is never hit. Add/Update keep their signatures and still work.
    * CalculateDiscount(promoId, lines, out message) - 0 + a readable message when the promo is
      not Active, not valid today, or has linked services that none of the lines match; otherwise
      the linked lines (or all lines when nothing is linked) are summed and ComputeDiscount runs.
    * GetPromotions / GetActiveValid now also return applies_to ("All services", or the first 3
      service names + " +N more"), built in C# (no STRING_AGG) by a private helper.
  - Forms\PromotionEditForm.cs: a HashSet<int> of ticked service ids (ticks survive the search and
    the category filter). Load fills cboServiceCategory ("All categories" + categories), loads
    GetPickList(EditId) (0 for a new promo), chkApplyAll is ticked when a new promo or an existing
    promo has no links. Ticked = the picker, search and category are disabled, the preview shows
    "Applies to every service on the bill." and the totals block is hidden. Ticking a row goes
    through CurrentCellDirtyStateChanged (CommitEdit) -> CellValueChanged, which updates the set,
    lblSelectedCount and the preview. The preview recalculates on every selection / type / value
    change through ComputeDiscount: Percentage shows regular, promo price = price - ROUND(price x
    value / 100, 2) and the saving per service; Fixed leaves the per-service promo columns empty
    and shows the totals, with "A fixed discount is taken off once per appointment." (the promo
    price is filled in the row when exactly one service is selected). Money uses the peso format
    (set in the Load event, the Designer keeps N2 only). New validation: at least one service when
    chkApplyAll is unticked, and a Fixed value may not exceed the regular total of the selected
    services. Save calls SavePromotion (one transaction) and logs "Add Promotion" / "Edit
    Promotion" with the number of linked services. A duplicate name from the repository is shown
    with Messages.Warn, not as a database error.
  - Controls\ucPromotions.cs: SelectionChanged (and every reload) fills lblPromoSummary. With
    linked services: "Applies to: A, B, C. Regular total P1 -> promo total P2 (save P3)" for a
    Percentage, or "P3 off the combined P1 of: A, B, C" for Fixed. With no links: "Applies to every
    service. 15 % off the whole bill." / "Applies to every service. PHP 100.00 off the whole bill.".
    Staff see the same summary; no selection clears it. The money comes from ComputeDiscount, so
    the formula is still in one place.
  - Database\LittleEreses_Database.sql: the CREATE TABLE PromotionServices was added right after
    Promotions (the same definition as the guarded script: two foreign keys + UNIQUE (promo_id,
    service_id)) and the header comment now says 18 tables.
  - Database\Migration_01_PromotionServices.sql (NEW FILE, already covered by the existing
    <None Include="Database\*.sql" /> in the .csproj): the guarded script
    (IF OBJECT_ID('dbo.PromotionServices') IS NULL BEGIN CREATE TABLE ... END). Text only, not run.
  - README.md: the setup step now mentions the migration for a database created before this
    feature and the 18 tables. PROGRESS.md: this entry.
  - Helpers\SmokeTest.cs: the three new read-only checks (ComputeDiscount Percentage 15 / 1000 =
    150.00, Fixed 500 / 300 = 300.00, GetServiceIds(0) is empty). ExpectedTables was already 18.
  - LittleEreseSystemV3.csproj was NOT edited.
- Standard clean build (bin and obj deleted first): 0 errors, 0 warnings.
  --smoketest: 38 passed, 0 failed, 0 skipped (the 3 new promotion checks all PASS).
  --preview checked for PromotionEditForm (two columns, picker locked while "Apply to all services"
  is ticked, preview note "Applies to every service on the bill.", footer visible) and ucPromotions
  ("Applies to" column filled, summary strip "Applies to every service. 15 % off the whole bill.").
  No layout fix was needed. The app was started once and closed again.

- Step 10 (this task): Layout only (Designer work) for the promotion-to-services feature.
  No logic, no SQL, no file or Session access: every wired event got an empty handler with
  "// TODO(logic prompt)", waiting for the logic prompt.
  - PROJECT_RULES.md: the "- Promos:" business rule was REPLACED with the new text (a promo may be
    linked to services through PromotionServices; no linked services = whole bill; with linked
    services only those appointment lines are eligible and the appointment must contain at least one
    of them; Percentage = eligible subtotal x value / 100; Fixed = value taken off ONCE per
    appointment and never above the eligible subtotal; ALWAYS compute the discount with
    PromotionRepository.CalculateDiscount; full payment upfront, Payments row of type Final,
    Billing.payment_status 'Paid'). One sentence was added to the Database value lists section:
    "Table PromotionServices (promo_service_id, promo_id, service_id) links a promotion to the
    services it applies to."
  - Forms\PromotionEditForm (.Designer.cs + .cs): the dialog is now 980 x 640 client (FixedDialog,
    CenterParent, AcceptButton btnSave, CancelButton btnCancel, errorProvider1 kept). The client was
    460 x 545 before; the prompt's 980 x 640 is inside the "dialogs at most 1000 x 640" rule.
    pnlHeader / pnlFooter are unchanged in look. pnlContent (16 px padding, AutoScroll just in case)
    holds the new tlpMain (2 columns 42% / 58%, the 16 px gap is the left margin of the right
    column). Every existing control name, event and field order was kept.
    LEFT (tlpLeft, one control per row, TabIndex order = visual order): lblName + txtName,
    lblConditions + txtConditions (70 px), lblType + cboType, lblValue + txtValue, lblHint
    "Promos need full payment upfront.", lblStart + dtpStart, lblEnd + dtpEnd, lblStatus + cboStatus.
    RIGHT (tlpRight, 56% / 44%): grpServices "Services in this promotion" with tlpServices -
    chkApplyAll "Apply to all services (no specific services)", lblServiceSearch + txtServiceSearch,
    lblServiceCategory + cboServiceCategory (DropDownList), dgvServicePick (AutoGenerateColumns =
    false; colPickCheck a DataGridViewCheckBoxColumn with an empty header, width 40, the ONLY
    editable column, DataPropertyName is_selected; colPickServiceId hidden / service_id;
    colPickService "Service" / service_name; colPickCategory "Category" / category_name;
    colPickPrice "Price" / price, N2, right aligned), lblSelectedCount "0 services selected" (muted);
    grpPreview "Price preview" with dgvPreview (colPrevService "Service" / service_name,
    colPrevRegular "Regular price" / regular_price, colPrevPromo "Promo price" / promo_price,
    colPrevSaving "You save" / saving, all N2 and read only) and the totals block lblRegularTitle +
    lblRegularTotal, lblDropTitle + lblDropTotal (Success green), lblPromoTitle + lblPromoTotal
    (BoldFont) and lblPreviewNote (muted, wraps).
    New empty handlers in the .cs (each with "// TODO(logic prompt)"):
    chkApplyAll_CheckedChanged, txtServiceSearch_TextChanged,
    cboServiceCategory_SelectedIndexChanged, dgvServicePick_CurrentCellDirtyStateChanged,
    dgvServicePick_CellValueChanged, cboType_SelectedIndexChanged, txtValue_TextChanged.
    btnSave and btnCancel keep their working logic - no existing behaviour was touched.
    Small Designer details: both grids get the Theme.StyleGrid look as literals
    (PrimaryDark header, PrimaryLight selection, Border grid lines).
  - Controls\ucPromotions (.Designer.cs): new column colAppliesTo "Applies to" / applies_to (130 px)
    between colStatus and colConditions; colConditions still AutoSizeMode.Fill (the remaining width).
    New summary strip pnlSummary (Dock Bottom, 56 px, Surface with a Border top line, 16 px side
    padding) with lblPromoSummary (Dock Fill, BodyFont, wraps - placeholder text "No promotion
    selected" until the logic prompt). Nothing else was changed; the .cs file was not touched.
  - Helpers\SmokeTest.cs: ExpectedTables 17 -> 18, because the database now really has 18 tables
    (PromotionServices exists in LittleEresesDB, verified with sys.tables). Without this the smoke
    test failed on correct data. Nothing else changed there.
  - LittleEreseSystemV3.csproj was NOT edited (no new file in this task).
  - Fix rounds from the previews (2 used): (1) the left column was 15 px taller than the row, so
    cboStatus was pushed under the footer - the label top margins went from 10 px to 8 px, which
    brought the whole column inside 980 x 640; (2) in dgvPreview the headers "Regular price" /
    "Promo price" were cut, so the three money columns were widened to 112 / 104 / 88 px
    (the Service column fills the rest).
- Standard clean build (bin, obj and .vs deleted first): 0 errors, 0 warnings.
  --smoketest: 35 passed, 0 failed, 0 skipped.
  --preview checked for PromotionEditForm (two columns, both cards complete, no truncated label,
  nothing under the footer, 980 x 640) and ucPromotions (new "Applies to" column visible, summary
  strip at the bottom, toolbar unchanged). The app was started once and closed again.
- Bug fix (this task): truncated toolbar labels, wrong field order in StaffEditForm and
  unreadable status colors on selected rows.
  - STEP 1 (truncated "Searc"): in the toolbars of ucServices, ucPromotions and ucStaffAccounts the
    labels lblSearch / lblCategory / lblStatus and lblCount now use AutoSize = true (the fixed
    48 px width was cutting the last letter of "Search"), and the neighbours were re-spaced so no
    control overlaps: lblSearch x 16, txtSearch x 72 (110 px), lblCategory x 190 / cboCategory x 260
    (110 px), lblStatus x 378 / cboStatus x 432 (services) or x 244 (promotions) or x 274
    (staff accounts). lblCount is AutoSize and right-aligned near the button block (591 / 534 / 406).
  - STEP 2 (StaffEditForm order): the content panel was re-laid out so the visual top-to-bottom
    order equals the TabIndex order - Full name (Tab 0/1), Username (2/3), Role (4/5),
    Contact number (6/7), Password (8/9), Confirm password (10/11), the muted password hint (12),
    Status (13/14), then the footer Cancel / Save buttons. The empty gap under the title is gone
    (the first row now starts at y = 16) and the dialog is still 620 px tall (max 640). Every
    control name and event wiring is unchanged; PasswordOnly mode still looks right (it locks the
    other controls in code, so only the two password boxes stay usable).
  - STEP 3 (selected row readability): in the CellFormatting handlers of ucServices, ucPromotions
    and ucStaffAccounts the Status cell now uses the grid's selection foreground (White) when the
    row is selected instead of the status color, so the text stays readable on the highlight.
  - STEP 4 (duration header): ucServices column header "Duration (min)" is now "Duration"
    (the cells still show "30 min").
  - STEP 5: PROJECT_RULES.md - added to the Layout prompt rules: "Toolbar and form labels use
    AutoSize = true; no text may be truncated in the previews. In dialogs, visual top-to-bottom
    order must equal TabIndex order. Status colors do not apply to selected grid rows."
  - STEP 6: Helpers\SmokeTest.cs - every EXACT count check is now an "at least" check:
    Services >= 77 (expectedMinServices already existed) and GetServices("", 0, "Active") >= 77.
    No other check changed.
  - LittleEreseSystemV3.csproj was NOT edited (no new file in this task; GridHelper.cs was already
    registered in the previous task).
- Standard clean build (bin, obj and .vs deleted first): 0 errors, 0 warnings.
  --smoketest: 35 passed, 0 failed, 0 skipped.
  --preview checked (1 fix round): ucServices (Search label complete, "Duration" header, 77
  services, selected row readable), ucPromotions (labels complete, 1 promotion), ucStaffAccounts
  (labels complete, 9 staff accounts) and StaffEditForm (fields in the requested order, no gap
  under the title, hint visible, footer complete). The app was started once and closed again.
- Bug fix (this task): "Current cell cannot be set to an invisible cell" after Save in the edit forms.
  - Helpers\GridHelper.cs (NEW FILE, registered in LittleEreseSystemV3.csproj as
    <Compile Include="Helpers\GridHelper.cs" />): internal static class GridHelper with
    SelectRowById(grid, idColumnName, id) (finds the row by id, selects it and puts CurrentCell in the
    FIRST VISIBLE column, scrolls it into view inside a try/catch, returns false when the id is not
    found or there is no visible column) and GetSelectedId(grid, idColumnName) (id of the selected or
    current row, 0 when nothing is selected). Neither method ever throws.
  - Controls\ucServices.cs: SelectService and CurrentServiceId now use GridHelper (was
    dgvServices.CurrentCell = row.Cells[0], the hidden id column).
  - Controls\ucStaffAccounts.cs: SelectUser and CurrentUserId now use GridHelper (same Cells[0] bug).
  - Controls\ucPromotions.cs: SelectPromotion and CurrentPromoId now use GridHelper (it used
    Cells[1], which breaks as soon as the first visible column changes).
  - PROJECT_RULES.md: added to the Logic prompt rules: "Never set DataGridView.CurrentCell to a cell
    of a hidden column. Re-select rows with GridHelper.SelectRowById(grid, idColumnName, id) and read
    the selected id with GridHelper.GetSelectedId(grid, idColumnName)."
  - Helpers\SmokeTest.cs: the two exact "Services rows = 77" checks were changed to "at least 77"
    (like the Users check), because the owner has since added a real service through the app
    ("BOTOX Test"), so the smoke test was failing on correct data. No other check changed.
- Standard clean build (bin, obj and .vs deleted first): 0 errors, 0 warnings.
  --smoketest: 35 passed, 0 failed, 0 skipped.
  --preview checked for ucStaffAccounts (9 rows selected/highlighted, no crash), ucServices (78 rows,
  selected row highlighted) and ucPromotions. The app was started once and closed again.
- Project scaffold: 9 Forms, 12 UserControls (WinForms), Data and Helpers classes created empty.
- Database script (Database/LittleEreses_Database.sql) and price list (Database/SeedData.sql) prepared.
- Fix pack applied: WinForms UserControls, clean .csproj, App.config connection string, DesignTime helper, Db helper.
- Step 1: Helpers filled in - Theme (colors, fonts, button/grid styles, status colors),
  IconHelper (Assets\Images loading, scaling, tinting, cached, plus MDL2 Glyph codes),
  Messages, Session, PasswordHasher (PBKDF2-SHA256), Validators (regex rules + ErrorProvider helpers).
- Program.cs catches UI-thread and AppDomain exceptions, shows Messages.Error(Db.Describe(ex))
  and appends the full exception to error.log next to the exe.
- PROJECT_RULES.md got the "Additions (Step 1)" and "Additions (Step 2)" sections.
- Step 2: UserRepository (Authenticate / UsernameExists / Create / SetPassword),
  ActivityRepository.Log (never throws), DemoDataSeeder (gives the placeholder admin a real
  password, creates 7 demo staff accounts, safe to run many times).
- Program.cs: startup order is --smoketest, Db.CanConnect, DemoDataSeeder, LoginForm.
- Helpers\SmokeTest.cs: "LittleEreseSystemV3.exe --smoketest" checks the UI classes, the tables,
  the service count and the demo logins, writes smoketest.log and exits with 0 or 1.
- Helpers\PreviewTool.cs: "LittleEreseSystemV3.exe --preview <ClassName> [--role Admin|Staff]"
  signs in a preview user, shows the form (or hosts a UserControl in a 1180x720 form), waits
  ~800 ms, writes previews\<ClassName>.png next to the exe and exits 0; on any error it writes
  previews\<ClassName>.error.txt and exits 1. Startup order: --smoketest, --preview, normal flow.
- Step 3: LoginForm layout (920x580, FixedSingle, CenterScreen) and MainForm shell
  (1280x720, MinimumSize 1100x650) done, plus a role aware sidebar. Previews checked.
- Step 4 (this task): LoginForm and MainForm are now working screens, not shells.
  - LoginForm fix: the username and the password field are now identical. The border lives on a
    host panel (new pnlUsernameBox + the existing pnlPassword), both 340 x 34 px with BackColor
    Surface and BorderStyle FixedSingle, holding a borderless BodyFont text box with 8 px left
    padding; btnShowPassword (36 px) stays at the right edge inside pnlPassword. The login card is
    340 px wide (same width as the Login button) and its rows are fixed heights, so nothing is
    cut off. Every control name, TabIndex and event wiring is unchanged.
  - LoginForm logic: images (ImageSpaLogoMiddle, BusinessLogoIcon) and Theme.StylePrimaryButton;
    btnShowPassword uses Theme.IconFont + Glyph.View and toggles UseSystemPasswordChar and the
    glyph (View/Hide); empty fields, wrong password and the lock out (5 failed tries in a row,
    then 30 seconds, shown as a countdown in lblError) are all reported in lblError, no popup.
    On success: Session.Start, ActivityRepository.Log("Login"), MainForm shown and the login
    form hidden. When MainForm closes: LoggedOut -> Session.End and the login form comes back
    with empty boxes, otherwise Application.Exit().
  - MainForm logic: public bool LoggedOut; Load sets WindowState Maximized, loads picBrand and
    every sidebar icon through IconHelper.Tinted(..., Theme.SidebarText, 22) (missing files just
    stay blank), sets the header icons/pictures and lblUserName/lblUserRole from Session, hides
    the five admin-only navigation buttons for Staff and opens Dashboard.
    public void Navigate(key) accepts Dashboard, Appointments, Clients, Services, Promotions,
    Billing, Commissions, Reports, Staff, FileMaintenance, ActivityLog and Settings: it builds a
    NEW UserControl, docks it Fill, disposes the previous page, sets lblPageTitle and paints the
    active nav button Theme.Primary. Admin pages refuse to open for a non-admin. Page creation is
    wrapped in try/catch (Messages.Error) so a broken page cannot close the app.
    btnMenuToggle collapses the sidebar between 240 and 68 px (texts kept in a dictionary, brand
    and version hidden while collapsed); btnSettings opens Settings; btnLogout asks for
    confirmation, logs "Logout" and closes with LoggedOut = true.
  - Sidebar layout fix found in the preview: WinForms centers a button's image + text as one
    group, so the 56 px left padding pushed every caption to the middle of the button. The nav
    buttons now use Padding 16 px with ImageAlign MiddleLeft and ImageBeforeText (set in code with
    the other runtime styling), so the icons line up in one column and the captions start right
    after them.
- Step 5 (this task): previews are never cropped and the sidebar icons got their gap.
  - PROJECT_RULES.md got the "Additions (Step 3) - Screen size" section (16:9 monitors, 1280x720
    logical minimum, content area about 1240 x 600, dialogs at most 1000 x 640, previews are
    rendered off screen so the monitor must not matter).
  - Helpers\PreviewTool.cs: the preview now always shows the whole window.
    * The thread asks Windows for "unaware" DPI (SetThreadDpiAwarenessContext), so a 150% monitor
      no longer lays the screens out 50% bigger. Previews are now the real logical sizes.
    * The window is shown off screen (Location -32000,-32000, no task bar button) and a form that
      maximizes itself (MainForm) is put back to its normal size before the picture, so the PNG is
      the whole window: LoginForm 932x604, MainForm 1296x759.
    * The PNG is drawn with Control.DrawToBitmap (whole window, title bar included). If that comes
      out blank (checked with a coarse color count) it falls back to copying the screen, and only
      then is the window shrunk to the working area, because a screen copy cannot be bigger.
    * The host form for a UserControl is now 1240 x 600 (the real content area). Usage
      (--preview <ClassName> [--role Admin|Staff]) and previews\<ClassName>.png are unchanged.
  - MainForm: SetNavIcon puts each 22 px icon on a transparent bitmap 10 px wider, so a visible gap
    sits between the icon and the caption (WinForms draws image and text with no space of its own,
    and widening the button padding moved both). Missing icon files still just leave a blank spot.
  - Forms\LoginForm.Designer.cs was rewritten for this, with every control name, TabIndex and event
    wiring kept: the login card container was restructured (new pnlCardCenter plus one small row
    panel per line) because a TableLayoutPanel with AutoSize = true kept squashing the rows - that is
    the real cause of the earlier "the two input boxes look different" problem (a 1 px high subtitle
    and a 16 px high field). Every row now has a fixed height, so the two input hosts are exactly
    340 x 34 and their text boxes exactly 322 x 18 (username) and 286 x 18 (password) at y = 7.
    The form is 916 x 558 client (932 x 604 window), keeps its 340 px card and 2:1 side photo.
  - The card is 426 px tall (sum of its fixed rows), so nothing is stretched and nothing is cut off.
- Standard clean build (bin and obj deleted first): 0 errors, 0 warnings.
  --smoketest: 29 passed, 0 failed, 0 skipped.
- Previews checked: LoginForm (both fields identical, logo and photo loaded, nothing cut off) and
  MainForm as Admin and as Staff (correct button order, visible icon/caption gap, header complete).

- Step 6 (this task): Layout only (Designer work) for the Services and Promotions module.
  No logic, no SQL, no file or Session access: every wired event got an empty handler with
  "// TODO(logic prompt)", waiting for the logic prompt.
  - Controls\ucServices (.cs + .Designer.cs): pnlToolbar (Dock Top, 56 px, Surface) with
    lblSearch / txtSearch / lblCategory / cboCategory (DropDownList) / lblStatus / cboStatus
    (All, Active, Archived) on the left, a right-aligned lblCount ("0 services", SmallFont,
    TextMuted) and btnAdd (primary) / btnEdit (secondary) / btnArchive (danger) / btnRefresh
    (secondary) on the right. Below it a card pnlGrid (Dock Fill, Surface, BorderStyle
    FixedSingle, 1 px padding) holds dgvServices (Dock Fill, Theme.StyleGrid look copied as
    literals, AutoGenerateColumns = false). Columns: colServiceId (hidden, service_id),
    colCategory, colService, colPrice (N2, right aligned), colDuration, colSessions, colStatus.
    Fill weights 20/30/14/16/10/14 make the price fit at 1240 px width.
  - Controls\ucPromotions (.cs + .Designer.cs): same toolbar, with btnToggle (danger, "Deactivate")
    instead of Archive, cboStatus items All, Active, Inactive, Expired and lblCount
    "0 promotions". Columns: colPromoId (hidden, promo_id), colName (fixed 220 px),
    colType / colValue (N2, right aligned) / colStart / colEnd (MMM dd, yyyy) / colStatus
    (fixed 90-120 px) and colConditions (DataPropertyName description, AutoSizeMode Fill, so it
    takes the remaining width). Fixed widths were chosen so the header "Promotion" is never cut.
  - Forms\ServiceEditForm (.cs + .Designer.cs): client 460 x 521 (the 480 x 560 in the prompt was
    shrunk to satisfy the "dialogs at most 1000 x 640" rule of Additions (Step 3) and to fit the
    content in one screen), FixedDialog, CenterParent, no minimize/maximize, AcceptButton btnSave,
    CancelButton + DialogResult.Cancel on btnCancel, errorProvider1. White header with
    lblTitle "Service" (HeadingFont) + 1 px Border line, AppBack content (cboCategory, txtName,
    txtDescription multi-line 90 px with vertical scrollbars, txtPrice, muted lblHint "For packages,
    enter the price of the whole package.", nudDuration 5-480 step 5 = 60, nudSessions 1-30 = 1,
    cboStatus Active/Archived) and a white footer with btnSave (primary) and btnCancel (secondary).
  - Forms\PromotionEditForm (.cs + .Designer.cs): client 460 x 545 (same dialog settings),
    lblTitle "Promotion", txtName, txtConditions multi-line 90 px, cboType Percentage/Fixed,
    txtValue, muted lblHint "Promos need full payment upfront.", dtpStart / dtpEnd (Format Short,
    side by side), cboStatus Active/Inactive, btnSave / btnCancel.
  - Layout fixes found in the previews: ucPromotions "Refresh" was cut at 72 px, so the toolbar
    buttons were re-spaced (Refresh 72 px, Deactivate 84 px, Add Promotion 116 px) so nothing is
    truncated; the Services toolbar was re-spaced the same way (button block and lblCount moved
    left, search/category/status fields slightly narrower) so the count label no longer collides
    with the Add button; both dialogs were re-laid out to 460 px client width.
  - LittleEreseSystemV3.csproj was NOT edited (every file already existed and was listed).
- Standard clean build (bin, obj and .vs deleted first): 0 errors, 0 warnings.
  --smoketest: 29 passed, 0 failed, 0 skipped.
  --preview checked for ucServices, ucPromotions, ServiceEditForm, PromotionEditForm (nothing
  overlaps, no truncated text, no misaligned control).

- Step 7 (this task): the LOGIC for the Services and Promotions module (code-behind + repositories).
  Designer layout untouched; no small Designer fix was needed.
  - Data\ServiceRepository.cs (new content): GetCategories(), GetServices(search, categoryId, status)
    (categoryId 0 = all, status "All" = all, search matches service OR category name; returns
    service_id, category_name, service_name, description, price, duration_minutes,
    default_total_sessions, status), GetById(id) (DataRow or null), GetActiveForBooking(),
    ServiceNameExists(name, categoryId, excludeId), Add(...) (returns the new service_id),
    Update(...), SetStatus(id, status). A service name is unique INSIDE its category; the check
    runs in the form and again in Add/Update (a friendly InvalidOperationException). Services are
    never deleted.
  - Data\PromotionRepository.cs (new content): RefreshExpired() (Active + end_date < today ->
    'Expired'), GetPromotions(search, status), GetById(id), GetActiveValid() (Active and today
    between start_date and end_date, for Billing / Appointments later), PromoNameExists(...),
    Add(...) (returns the new promo_id), Update(...), SetStatus(id, status). Promo names are unique
    (checked in the form and again in Add/Update). Promotions are never deleted.
  - Helpers\Validators.cs: added TryMoney (money parse used by the forms) and IsLengthBetween
    (trimmed length, e.g. service/promo names 2..100) and IsValidPercentage (> 0 and <= 100).
    No existing rule was changed.
  - Controls\ucServices.cs: Load styles the grid + buttons with Theme, hides btnAdd / btnEdit /
    btnArchive for a Staff user (view only), fills cboCategory with "All categories" + the real
    categories and starts cboStatus on "Active". Search and both filters reload (guarded by a
    _loading flag so filling the combos does not double-load). Add / Edit open ServiceEditForm
    (EditId set before ShowDialog) and reload with the saved row selected. Double-click = Edit
    (Admin only). btnArchive flips between "Archive" (Active rows) and "Restore" (Archived rows)
    via CellFormatting-free SelectionChanged, confirms with Messages.Confirm, calls SetStatus and
    logs "Add" / "Edit" / "Archive" / "Restore" with the service name through
    ActivityRepository.Log. CellFormatting colors the Status cell with Theme.StatusColor and
    appends " min" to the duration.
  - Controls\ucPromotions.cs: Staff see only GetActiveValid() and the status filter + all action
    buttons are hidden. Admin gets the full list; Load and Refresh call RefreshExpired() first.
    btnToggle reads "Deactivate" (Active), "Activate" (Inactive) and is disabled for Expired rows.
    Add / Edit open PromotionEditForm; CellFormatting colors the status and shows the value as
    "15 %" for Percentage or as money for Fixed. Every Add / Edit / Activate / Deactivate is logged.
  - Forms\ServiceEditForm.cs / PromotionEditForm.cs: both keep the parameterless constructor and
    expose EditId (0 = new) plus SavedServiceId / SavedPromoId (the caller selects that row after
    saving). Load styles the buttons, fills the category list / defaults, and for EditId > 0 loads
    the row and retitles to "Edit Service" / "Edit Promotion". Save validates with Validators +
    errorProvider1 (category + name 2..100 + unique, price money > 0; promo: name + type + value > 0,
    percentage <= 100, end >= start, and a NEW promo may not end in the past), then Add or Update
    in a try/catch, sets DialogResult.OK and closes. Cancel returns Cancel.
  - Helpers\SmokeTest.cs: three read-only checks (GetCategories = 10 rows,
    GetServices("", 0, "Active") = 77 rows, GetPromotions("", "All") runs without error).
  - LittleEreseSystemV3.csproj was NOT edited (all files already listed).
- Standard clean build (bin, obj and .vs deleted first): 0 errors, 0 warnings.
  --smoketest: 32 passed, 0 failed, 0 skipped (the 3 new repository checks all PASS).
  --preview checked for ucServices (grid + filters populated, count "77 services"), ucPromotions
  (empty until a promo is added - no promo seed data exists yet), ServiceEditForm (category "Botox"
  loaded, hint text visible) and PromotionEditForm (dates default to today / +1 month).

- Step 8 (this task): Layout only (Designer work) for the Staff Accounts module and Settings.
  No logic, no SQL, no file or Session access: every wired event got an empty handler with
  "// TODO(logic prompt)", waiting for the logic prompt.
  - Controls\ucStaffAccounts (.cs + .Designer.cs): same page look as ucServices. pnlToolbar
    (Dock Top, 56 px, Surface) with lblSearch / txtSearch (140 px) / lblStatus / cboStatus
    (All, Active, Inactive) on the left, lblCount ("0 staff accounts", SmallFont, TextMuted)
    right after them and btnAdd (primary, "Add Staff") / btnEdit (secondary) / btnToggleStatus
    (danger, "Deactivate") / btnResetPassword (secondary, 120 px) / btnRefresh (secondary, 72 px)
    right aligned. Below it the card pnlGrid (Dock Fill, Surface, FixedSingle, 1 px padding) holds
    dgvStaff (Dock Fill, Theme.StyleGrid look copied as literals, AutoGenerateColumns = false).
    Columns: colUserId (hidden, user_id), colFullName (fill 26, full_name), colUsername (fill 18,
    username), colRole (fill 10, role), colContact (fill 18, contact_number), colStatus (fill 12,
    status), colCreated (fill 16, created_at, format "MMM dd, yyyy"). The button block starts at
    x = 504 so lblCount never collides with it at 1240 px width (the same trick as ucServices).
  - Forms\StaffEditForm (.cs + .Designer.cs): FixedDialog, CenterParent, no minimize/maximize,
    AcceptButton btnSave, CancelButton + DialogResult.Cancel on btnCancel, errorProvider1.
    The prompt's 480 x 620 was shrunk to a 460 px client width so it matches ServiceEditForm /
    PromotionEditForm and the "dialogs at most 1000 x 640" rule. White header with lblTitle
    "Staff Account" (HeadingFont) + 1 px Border line, AppBack content and a white footer with
    btnSave (primary) and btnCancel (secondary). Fields: cboStatus (Active, Inactive),
    txtFullName, txtUsername, cboRole (Admin, Staff), txtContact, txtPassword and
    txtConfirmPassword (both UseSystemPasswordChar), muted lblPasswordHint "At least 8 characters
    with a letter and a number." under them, leaving some free space above the footer.
  - Controls\ucSettings (.cs + .Designer.cs): a centered 640 px column on AppBack, built with a
    3-column tlpRoot (50% / 640 px / 50%) that holds a tlpColumn (one row per card, 20 px side
    padding). Cards: pnlProfile (Surface, FixedSingle) with picAvatar 96x96 (SizeMode Zoom),
    lblFullName (HeadingFont size 12 semibold, TextDark), lblRole (Primary), lblUsername and
    lblContact (TextMuted); grpPassword (Surface, caption "Change Password") with lblCurrent /
    txtCurrentPassword, lblNew / txtNewPassword, lblConfirmNew / txtConfirmNewPassword (all
    UseSystemPasswordChar), muted lblPasswordHint and btnChangePassword (primary look, 180 px)
    at the bottom right; pnlAbout (Surface, FixedSingle) with lblAppName "Little Erese's
    Appointment and Billing System", lblAppVersion "Version" and lblDatabase "Database" (both
    filled at runtime by the logic prompt). errorProvider1 sits on the UserControl.
    POLISH: the version and database strings are placeholder text until the logic prompt.
  - Layout fixes found in the previews: StaffEditForm had too much empty space under the title
    (the first draft put the rows at y >= 124), so the fields were moved up to a 22 px row rhythm
    starting at y = 60 (cboStatus) - every label/field pair is 22 px apart and the hint line sits
    12 px under the last field, so nothing overlaps or is cut.
  - LittleEreseSystemV3.csproj was NOT edited (every file already existed and was listed).
- Standard clean build (bin, obj and .vs\iolys deleted first): 0 errors, 0 warnings.
  --smoketest: 32 passed, 0 failed, 0 skipped.
  --preview checked for ucStaffAccounts (toolbar fits, all five buttons readable, grid columns
  visible), StaffEditForm (no overlap, hint visible, footer complete) and ucSettings (640 px
  column centered, three cards, nothing cut off). The app was started once and closed again.

- Step 9 (this task): the LOGIC for Staff Accounts and Settings (code-behind + repository).
  Designer layout untouched; no small Designer fix was needed.
  - Data\UserRepository.cs keeps Authenticate / UsernameExists / Create / SetPassword and adds:
    GetUsers(search, status) (search matches full name OR username, status "All" = all; returns
    user_id, full_name, username, role, contact_number, status, created_at), GetById(id),
    Update(userId, fullName, role, contact, status), SetStatus(userId, status),
    CountActiveAdmins(), ChangeOwnPassword(userId, currentPassword, newPassword) -> bool (false
    when the current password is wrong) and GetActiveStaff() (role Staff + status Active, returns
    user_id, full_name for the technician drop-downs later).
    The "do not lock yourself out" rules live in one private helper used by Update AND SetStatus:
    you cannot change your own role, you cannot deactivate your own account, and the LAST Active
    Admin can neither be demoted or deactivated. Each rule throws an InvalidOperationException
    with a plain English message that the forms show as a warning (not as a database error).
    UsernameExists now compares with UPPER() on both sides so the unique username rule is
    case-insensitive whatever collation the database uses.
  - Controls\ucStaffAccounts.cs: Load exits early (and says so) unless Session.IsAdmin; it styles
    the grid + all five buttons with Theme, starts the status filter on "All" and loads the grid.
    Search and status reload with a _loading guard. Add / Edit open StaffEditForm (EditId set first,
    the saved row is selected again through SavedUserId). Reset Password opens the same form with
    PasswordOnly = true. SelectionChanged enables Edit / Deactivate / Reset and flips the danger
    button between "Deactivate" and "Activate"; ToggleStatus confirms first, then calls SetStatus.
    CellFormatting colors the Status cell with Theme.StatusColor. Every Add / Edit / Reset Password /
    Activate / Deactivate is logged with ActivityRepository.Log - the password is NEVER logged.
  - Forms\StaffEditForm.cs: public properties EditId (0 = new) and PasswordOnly, plus
    SavedUserId for the caller. Load styles the buttons, defaults to Active / Staff, loads the row
    when EditId > 0 (title "Edit Staff Account", username box read-only) and locks everything
    except the password boxes when PasswordOnly (title "Reset Password"). A new account needs a
    valid, still-free username; editing leaves the password boxes optional (blank = keep the
    current password). Validation uses Validators + errorProvider1 (name, username, role, mobile
    contact, password rule and the confirmation match) and saving goes through
    UserRepository.Create / Update / SetPassword.
  - Controls\ucSettings.cs: Load styles the button, loads picAvatar with
    IconHelper.Load("UserIconYellowishColorTransparent", 96), fills the profile card from Session
    plus the contact number from UserRepository.GetById, shows the assembly version
    (Assembly.GetName().Version, 3 parts) and the database name read out of the App.config
    connection string with SqlConnectionStringBuilder. btnChangePassword_Click validates (all three
    required, new password rule, confirmation equal, new different from current), calls
    ChangeOwnPassword and shows the success message - or "Current password is incorrect." on the
    box itself - then clears the boxes. "Change Password" is logged.
  - Helpers\SmokeTest.cs: three read-only checks (GetUsers("", "All") >= 8 rows,
    CountActiveAdmins() >= 1, GetActiveStaff() >= 7 rows).
  - LittleEreseSystemV3.csproj was NOT edited (all files already listed).
- Standard clean build (bin, obj and .vs deleted first): 0 errors, 0 warnings.
  --smoketest: 35 passed, 0 failed, 0 skipped (the 3 new Staff Accounts checks all PASS:
  GetUsers = 8 rows, CountActiveAdmins = 1, GetActiveStaff = 7 rows).
  --preview checked for ucStaffAccounts (8 rows with green Active status, count "8 staff accounts",
  toolbar complete and nothing truncated), StaffEditForm (no overlap, hint visible, footer complete)
  and ucSettings (avatar loaded, name / role / username / contact filled, "Version 1.0.0",
  "Database: LittleEresesDB"). The app was started once and closed again.

## Next
- The Clients module is COMPLETE (ucClients, ClientEditForm, PackageForm + ClientRepository).
  Next: the Billing / Appointments modules, which reuse ClientRepository.GetForCombo() to pick the
  guest and ClientRepository.HasActivePackage / AddPackage for the package rules (the package price
  is charged on the first session, later sessions of that package are 0.00).
- Promotions are complete (services pick list, price preview, one-transaction save, applies_to and
  the summary strip). What is left for the owner: the Billing / Appointments screens must call
  PromotionRepository.CalculateDiscount(promoId, lines, out message) when a promo is applied, and
  save the full payment upfront (Payments row of type Final, Billing.payment_status 'Paid').
- The Dashboard screen (ucDashboard with ucCalendarMonth) and the remaining modules
  (Appointments, Billing, Commissions, Reports, File Maintenance, Activity Log) still need their
  layout prompts; ucClients has a layout now but no logic yet, so it shows an empty grid and the
  placeholder details card ("Select a client").
- POLISH items left for the owner: the lock out limits (5 tries / 30 seconds) in LoginForm,
  the "Log out now?" wording, the PhoneRegex (mobile numbers only), the "-" placeholder for an
  empty contact number in Settings and the version format (3 parts) in Settings.
- POLISH item from the Clients logic: PackageForm lists the services whose
  default_total_sessions is greater than 1 first and only falls back to every Active service when
  none is flagged as a package. Confirm with the owner which services may be sold as packages.

## Known issues
- The Clients grid has no rows yet because Database\SeedData.sql has no Clients rows, so the
  details card shows its placeholder ("Select a client") until the first client is added with
  "Add Client". This is expected, not a bug.
- "Last Visit" is empty for every client for the same reason (it needs a Completed appointment).
- A client with a birth date but no gender chosen cannot be saved (gender is required by the
  validation), so old rows without a gender must be completed when they are edited.
- The summary strip and the price preview show the FULL service price (Services.price) of the
  linked services, because a promotion is not tied to one appointment there. When Billing applies a
  promo it must use PromotionRepository.CalculateDiscount on the real appointment lines instead.
- None blocking. The remaining page UserControls are still empty shells, so switching to them
  shows an empty content area (this is expected until each page is built).
- The Promotions list is empty at first because the seed data has no Promotions rows; add one with
  "Add Promotion" to see the grid, the status colors and the "15 %" / money formatting. POLISH:
  the owner may want 2-3 sample promos in Database\SeedData.sql.
- Demo staff accounts still have the placeholder names "Aesthetician 1".."Nurse 1" (the schema
  has no job-title column) - rename them in Staff Accounts (now possible: Staff Accounts is live).
- Demo staff accounts have no contact number yet, so the Contact column is empty until the numbers
  are filled in through Staff Accounts / Edit.
- Every account (New or Edit) needs a phone-shaped contact number (09XXXXXXXXX), so an account
  without one must be given one when it is edited. POLISH: make the contact number optional if the
  owner prefers.
- The smoke test only constructs the forms (no Show), so it does not cover the new event handlers;
  they were checked with --preview and by starting the app.
- ucStaffAccounts refuses to run for a Staff user on purpose (it shows a short warning), and
  UserRepository blocks deactivating or demoting your own account and the last Active Admin, so a
  single-Admin shop can never lock itself out.
