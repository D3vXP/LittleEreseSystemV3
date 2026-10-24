# PROGRESS.md

Update this file at the end of every task.

- Step 18 (this task): the LOGIC for the Billing module (repository, receipt printer, the page and
  the payment dialog). The Designer layouts were NOT restructured; the only Designer touch was
  temporarily setting tabBilling.SelectedIndex = 1 to take the Transactions preview, which is back
  to 0.
  - Data\BillingRepository.cs (the Step 16 methods are unchanged; everything below is new):
    READ: GetQueue(date) (appointment_id, time_range, client_name, services "Name x2", status,
    payment_status, balance_due; the non-cancelled appointments of that day, past days allowed on
    purpose), GetBillHeader(appointmentId) (client_name, appointment_date, start_time, end_time,
    appointment_status, billing_id, promo_id, promo_name, subtotal, discount_amount,
    total_amount, paid_so_far = every payment that is not 'Rejected', balance_due,
    payment_status; null when there is no bill), GetBillLines(appointmentId)
    (appointment_service_id, service_name, quantity, price, session_label "2 / 8" or empty,
    technician_id 0 when none was requested, line_status), GetTransactions(from, to, search,
    method) (billing_id, payment_date = the latest payment, client_name, services, total_amount,
    amount_paid = the sum of the non-rejected payments, payment_method of the latest payment,
    reference_number, staff_names, payment_status; only bills with at least one non-rejected
    payment; "All" = every method; the whole "to" day belongs to the range) and
    GetReceiptData(billingId) (the header fields repeated on every service line plus
    staff_names; empty table when the bill has no lines). Every comma list is built in C# with ONE
    query per list (no STRING_AGG), as in AppointmentRepository.
    ApplyPromo(appointmentId, promoId) returns an empty text or a readable sentence. It is refused
    when the bill already has a promo, when anything was paid (a promo is never an installment) or
    when the appointment is completed; the discount ALWAYS comes from
    PromotionRepository.CalculateDiscount, and discount / total / balance are written in one
    transaction. It logs "Apply Promo".
    Checkout(appointmentId, technicianByLine, method, reference, verified, tendered) returns the
    billing_id and does everything in ONE Db.InTransaction. It validates first (not Cancelled /
    Completed, date today or earlier, a technician on every line, and while balance > 0 a method,
    a reference + the tick for GCash / Bank Transfer, tendered >= balance for Cash), then
    (a) a 'Final' Payments row ('Verified') when money is still due, (b) the bill reads
    balance 0 / 'Paid' / downpayment 'Held' -> 'Applied', (c) AppointmentStaff per line (user_id
    replaced by the real technician, 'Completed', inserted when the line had none),
    (d) AppointmentServices 'Completed', (e) Appointments 'Completed', (f)
    ClientPackages.sessions_completed + 1 and 'Completed' at total_sessions, (g) one Commissions
    row per line (10 %, the proportional discount share taken off, package lines use
    price / total_sessions per the business rules, payout 'Unpaid', payment_id = the new payment or
    NULL), (h) the clients.treatment_history line "MMM dd, yyyy - Service name (Staff full name)"
    appended, (i) an AppointmentHistory 'Status Change' row and (j) ActivityLog "Checkout".
  - Helpers\ReceiptPrinter.cs (NEW FILE, registered in LittleEreseSystemV3.csproj as
    <Compile Include="Helpers\ReceiptPrinter.cs" />): internal static Print(billingId, preview)
    draws an 80 mm style receipt (Consolas, 280 printer units wide) with the business name and
    address, "OFFICIAL RECEIPT" + OR-yyyyMMdd-<billing_id>, the date, the client, each service line
    (name, "qty x price", amount), subtotal, discount, paid upfront, total, the method with its
    reference, the attending staff and "Thank you for trusting Little Erese's.". It shows a
    PrintPreviewDialog when preview is true and prints otherwise, and it never throws (Messages +
    Db.Describe on failure). POLISH marks on the header text.
  - Helpers\Validators.cs: added IsValidReference (letters and digits, 6 to 30 characters) with a
    // POLISH note. No existing rule changed.
  - Controls\ucBilling.cs: Load styles the three grids and the six buttons, dtpQueueDate = today,
    the Transactions range = this month (Admin) or today and locked (Staff), cboMethod "All", the
    technician column = "Select staff" + UserRepository.GetActiveStaff() and cboPromo = "No promo"
    + PromotionRepository.GetActiveValid() with the discount in the caption. A DataError handler
    keeps the staff drop-down from showing a raw .NET dialog.
    Selecting a queue row loads the bill (client, "Schedule: date  start - end (status)", the lines
    with the requested technician preselected, the five totals) and sets the buttons: the promo box
    only while the bill has no promo, nothing was paid and the appointment is not finished; the
    lines grid is locked once the appointment is completed / cancelled.
    cboPromo_SelectedIndexChanged calls ApplyPromo, warns with the message when it is refused and
    restores the previous choice. btnCollect validates the appointment and every line's staff,
    opens PaymentForm when a balance is left (or asks Messages.Confirm when nothing is due), calls
    Checkout, shows the change for cash, reloads the queue and the bill and offers the receipt with
    ReceiptPrinter.Print(billingId, true). btnPrintReceipt prints the selected bill.
    dgvQueue_CellFormatting colors Status and Payment with Theme.StatusColor (white on a selected
    row). Staff cannot search a range (the two pickers are disabled), btnSearch validates the range,
    lblTotals shows "<n> transactions - <sum of amount_paid>", btnReprint prints the selected row's
    receipt and dgvTransactions_CellFormatting colors the Status cell.
  - Forms\PaymentForm.cs: public AmountDue / ClientName in, Method / Reference / Verified /
    Tendered out. Load styles the two buttons, shows the name and the amount, picks Cash and fills
    the change row. Cash enables the tendered box; GCash / Bank Transfer enable the reference box
    and the tick and clear the other one. txtTendered_TextChanged keeps lblChange at "never
    negative". btnConfirm validates through Validators + errorProvider1 (cash needs a number
    >= AmountDue, GCash / bank need a valid reference and the tick) and then sets DialogResult.OK;
    btnCancel returns Cancel.
  - Helpers\SmokeTest.cs: the three read-only Billing checks (GetQueue(today) runs,
    GetTransactions(first day of month, today, "", "All") runs, GetBillHeader(0) returns no row).
    Nothing is written.
  - Self-test (temporary, REMOVED again - no --billingtest switch and no BillingSelfTest.cs are
    left in Program.cs or the .csproj): booked one walk-in appointment today for the first demo
    client with one normal service (Traptox, 5,000.00) and checked it out as Cash. Every check
    PASSED: Billing = balance 0.00 / Paid, one Payments row 'Final' / Cash / 5,000.00 / 'Verified',
    AppointmentServices 'Completed', Appointments 'Completed', Commissions = 500.00 (10 %) / rate
    10.00 / 'Unpaid' for the technician, and Clients.treatment_history gained
    "Oct 24, 2026 - Traptox (Aesthetician 1)". Every row was deleted again (Commissions, Payments,
    Billing, AppointmentHistory, AppointmentStaff, AppointmentRooms, AppointmentServices,
    Appointments, the treatment_history line and the ActivityLog rows) and the cleanup check
    reported nothing left behind; the database is back to 0 appointments.
  - Assumptions: a refund / rejected-payment screen is not part of this step (the Payments history
    stays as it is); Checkout stores the final payment as 'Verified' because the counter only
    confirms money it has seen (the tick is still required for GCash / the bank); the receipt shows
    the current date, which is what the counter hands to the client.
  - LittleEreseSystemV3.csproj WAS edited: one line added,
    <Compile Include="Helpers\ReceiptPrinter.cs" /> (the temporary self-test file was added and
    removed again).
- Standard clean build (bin and obj deleted first, MSBuild /t:Rebuild): 0 errors, 0 warnings.
  --smoketest: 47 passed, 0 failed, 0 skipped.
  --preview checked for ucBilling (Checkout: the queue date / Today / Refresh and "0 in the queue",
  the 7-column queue grid and the bill card with "Select an appointment", the 7-column lines grid,
  the promo row, the five totals and the two buttons; Transactions, previewed by temporarily
  selecting tab 1: From / To / Client / Method, Search + Print Receipt,
  "0 transactions - P0.00" and the 9-column payments grid - nothing overlapping or truncated) and
  PaymentForm (the name, the amount due, the method, reference, tendered, change, the verification
  tick, the note and the footer, all inside 460 x 560). The app was started once and closed again
  (no error.log).

- Step 17: Layout only (Designer work) for the Billing page and the payment dialog.
  No logic, no SQL, no file or Session access: every wired event got an empty handler with
  "// TODO(logic prompt)", waiting for the logic prompt. Both files were empty shells before.
  - Controls\ucBilling (.Designer.cs + .cs): AppBack page, 1000 x 640, holding tabBilling
    (Dock Fill) with two tabs.
    Tab "Checkout" (tabCheckout) = splitMain (Vertical, SplitterDistance 394 = about 40 % of the
    986 px, Panel1MinSize 300, Panel2MinSize 340). LEFT panel: pnlQueueToolbar (Dock Top, 76 px,
    Surface, FixedSingle) with flpQueueTop (lblQueueDate "Date" AutoSize, dtpQueueDate Short
    132 px, btnQueueToday "Today", btnRefresh "Refresh", secondary look) and lblQueueCount
    ("0 in the queue", SmallFont, TextMuted); below it the card pnlQueueGrid (Surface,
    FixedSingle, 1 px padding) with dgvQueue (Dock Fill, Theme.StyleGrid look as literals,
    AutoGenerateColumns = false): colQueueAppointmentId (hidden, appointment_id), colQueueTime
    "Time" / time_range, colQueueClient "Client" / client_name, colQueueServices "Services" /
    services, colQueueStatus "Status" / status, colQueuePayment "Payment" / payment_status,
    colQueueBalance "Balance" / balance_due (N2, right aligned). Fill weights 17/22/24/13/12/14.
    RIGHT panel: the scrollable card pnlBill (Surface, FixedSingle, AutoScroll, 16 px padding)
    with the AutoSize tlpBill, one full-width row per block: lblBillClient (HeadingFont 16 pt,
    placeholder "Select an appointment"), lblBillSchedule (TextMuted, "Schedule: -"),
    dgvBillLines (150 px; Theme.StyleGrid literals; the ONLY editable column is the
    DataGridViewComboBoxColumn colLineTech "Attending staff" / technician_id) with colLineId
    (hidden, appointment_service_id), colLineService "Service" / service_name, colLineQty "Qty" /
    quantity (centered), colLinePrice "Price" / price (N2, right), colLineSession "Session" /
    session_label, colLineStatus "Status" / line_status; flpPromo (lblPromo + cboPromo DropDownList
    282 px), lblPromoNote (muted, 2 lines), tlpTotals (5 rows: Subtotal / Discount / "Paid so far" /
    Total / "Balance due" + lblBalance in TitleFont 22 pt Primary gold, right aligned), btnCollect
    "Collect Payment and Complete" (primary look, full width, 48 px) and btnPrintReceipt
    "Print Receipt" (secondary look).
    Tab "Transactions" (tabTransactions): pnlTranToolbar (Dock Top, 92 px, Surface, FixedSingle)
    with two rows - row 1 flpTranFilters (lblFrom + dtpFrom, lblTo + dtpTo, lblSearch "Client" +
    txtSearch 180 px, lblMethod + cboMethod with All / Cash / GCash / Bank Transfer, all labels
    AutoSize); row 2 flpTranButtons (btnSearch "Search" primary, btnReprint "Print Receipt"
    secondary) and the right-aligned lblTotals (BoldFont, "0 transactions - 0.00"). Below it the
    card pnlTranGrid with dgvTransactions (Theme.StyleGrid literals, AutoGenerateColumns = false):
    colTranBillingId (hidden, billing_id), colTranDate "Date" / payment_date (MMM dd, yyyy h:mm tt),
    colTranClient "Client" / client_name, colTranServices "Services" / services, colTranTotal
    "Total" / total_amount (N2, right), colTranPaid "Paid" / amount_paid (N2, right), colTranMethod
    "Method" / payment_method, colTranRef "Reference" / reference_number, colTranStaff "Staff" /
    staff_names, colTranStatus "Status" / payment_status.
    Events wired: Load, dtpQueueDate_ValueChanged, btnQueueToday_Click, btnRefresh_Click,
    dgvQueue_SelectionChanged, dgvQueue_CellFormatting, cboPromo_SelectedIndexChanged,
    dgvBillLines_CellValueChanged, dgvBillLines_CurrentCellDirtyStateChanged, btnCollect_Click,
    btnPrintReceipt_Click, btnSearch_Click, btnReprint_Click, dgvTransactions_CellFormatting.
  - Forms\PaymentForm (.Designer.cs + .cs): 460 x 560 client (the prompt's size, already inside
    the "dialogs at most 1000 x 640" rule), FixedDialog, CenterParent, no minimize/maximize,
    AcceptButton btnConfirm, CancelButton + DialogResult.Cancel on btnCancel, errorProvider1,
    white header with lblTitle "Collect Payment" (HeadingFont) + 1 px Border line, white footer,
    AutoScroll content so nothing can be cut on a 768 px screen.
    Content, top to bottom = TabIndex order: lblClientName (Semibold, "-"), lblAmountDueTitle
    "Amount due", lblAmountDue (TitleFont 22 pt, Primary gold), lblMethod + cboMethod
    (DropDownList, Cash / GCash / Bank Transfer), lblReference + txtReference, lblTendered +
    txtTendered, lblChangeTitle "Change" + lblChange (Success green, 14 pt), chkVerified
    "I verified this payment in the GCash / bank app", lblNote (muted, wraps: the system never
    contacts GCash or a bank), footer btnConfirm "Confirm Payment" (primary look, 144 px) and
    btnCancel "Cancel" (secondary look).
    Events wired: Load, btnConfirm_Click, btnCancel_Click, cboMethod_SelectedIndexChanged,
    txtTendered_TextChanged.
  - Fix rounds from the previews (2 used): (1) the queue date picker was 120 px wide and cut
    "10/24/2026", so dtpQueueDate is 132 px (Today / Refresh moved 12 px right); the Transactions
    toolbar filters were crowded, so dtpFrom / txtSearch / cboMethod were re-spaced to leave a
    gap before every label; (2) the Transactions "Date" column could not show
    "Oct 24, 2026 10:30 AM" at Fill weight 15, so colTranDate now has MinimumWidth 155 and
    Width 160. The Transactions tab was also previewed once by temporarily selecting it
    (Designer value is back to SelectedIndex 0 = Checkout) to confirm its layout.
  - LittleEreseSystemV3.csproj was NOT edited (both files already existed and were listed).
- Standard clean build (bin and obj deleted first, MSBuild /t:Rebuild): 0 errors, 0 warnings.
  --smoketest: 44 passed, 0 failed, 0 skipped.
  --preview checked for ucBilling (Checkout: the queue card with date / Today / Refresh and the
  7-column grid, the bill card with the heading, schedule, the 7-column lines grid, promo, note,
  the five totals and the two buttons; Transactions: the From / To / Client / Method row, Search +
  Print Receipt, the "0 transactions - 0.00" total and the 9-column grid) and PaymentForm (the
  amount due, method, reference, tendered, change, the verification tick, the note and the footer,
  nothing overlapping or truncated).
  The app was started once and closed again (no error.log).

- Step 16 (this task): the LOGIC for the Appointments module (transaction helpers, repositories,
  the day page and the booking / reschedule / cancel dialog). The Designer layout was NOT
  restructured; no small Designer fix was needed.
  - Data\Db.cs (additive only, no existing signature changed): InTransaction(work) opens a
    connection, begins a transaction, runs the work, commits it, rolls it back and rethrows when
    anything inside throws; plus Query / Execute / Scalar overloads that take
    (SqlConnection, SqlTransaction, sql, params) and fall back to the normal helper when the
    connection is null. WHY: booking writes rows in six tables and must never be left half saved.
  - Data\BillingRepository.cs (was an empty class): CreateBilling(conn, tx, ...) (returns the new
    billing_id), AddPayment(conn, tx, ...) (Payments row + the bill is re-balanced),
    GetByAppointment (bill + promo name + the paid total) and RefreshBalance(conn, tx, billingId),
    which is the ONE place the rule "balance = total - every payment that was not rejected" and the
    Unpaid / Partial / Paid status are written down.
  - Data\AppointmentRepository.cs (was an empty class): the booking rules as constants
    (30 per day, 10 massage per day, 3 days notice, massage categories start with "Massage",
    aesthetic 10:00 AM - 8:00 PM, massage 1:00 PM - 10:00 PM).
    READ: GetByDate(date, status, search) (appointment_id, time_range, client_name, services
    "Name x2", staff_names "Unassigned", room_names, booking_source, status, payment_status; the
    comma lists are built in C# so the app does not need STRING_AGG), GetServiceLines,
    GetById, GetTimeSlots (24 slots, 10:00 AM to 9:30 PM), GetRooms, GetRoomsForCategory,
    GetLinesForBooking (the lines in the shape Create expects), GetDayCounts (the toolbar strip),
    TryGetPackagePlan (which package row a booking will use), TotalDuration, Subtotal and
    RequiredNow. RequiredNow is the ONE place the money rule lives: a promotion is paid in FULL
    whatever the source, otherwise Messenger / Text / Phone pay 50% of the subtotal and a walk-in
    without a promotion pays 0 now.
    VALIDATION: ValidateBooking(...) returns a List<string> of readable problems: client chosen,
    at least one service, the date not in the past (today only while the start time is ahead),
    every service inside its category's opening hours and the end time too, the daily capacity
    (30 / 10 massage, ignoring the appointment being rescheduled), a fitting room for every line
    (auto-assigned from RoomServiceCategories, capacity aware), no overlapping service for a
    requested technician, the upfront amount / reference / verified tick for GCash and bank
    payments, and a promotion that is valid AND applies to at least one selected service
    (always through PromotionRepository.CalculateDiscount).
    WRITE (each in ONE transaction): Create (Appointments + AppointmentServices with the package
    rules - a new ClientPackages row when needed, the whole package price on session 1 and 0.00 on
    later sessions - AppointmentStaff 'Assigned', AppointmentRooms 'Reserved', Billing, the
    upfront Payments row (Downpayment, or Final for a promo; Verified only with the tick), the
    'Created' history row and the ActivityLog entry, and it returns the new appointment_id);
    Reschedule (re-validates itself out of the way, applies the 3-day Admin rule, updates the date
    and the times, re-assigns the rooms, keeps the deposit 'Held' and writes the 'Rescheduled'
    history row with the old date, the old time and the reason); Cancel (3-day Admin rule,
    'Cancelled' + cancelled_at, services 'Cancelled', rooms 'Released', the money forfeited
    (downpayment_status 'Forfeited', or payment_status 'Forfeited' for a prepaid promo), a package
    opened for this booking with no completed session set to 'Expired', and the 'Cancelled' history
    row with the reason); SetStatus ('Upcoming' -> 'Ongoing' only on the appointment's own day,
    with a 'Status Change' history row).
  - Controls\ucAppointments.cs: Load styles the two grids and the five buttons with Theme, starts
    the status filter on "All" and the date on today (or on the public StartDate the caller set)
    and loads the day. The date buttons and Today move the date; the status filter and the client
    search reload (a _loading flag stops the events from firing twice). Selecting a row fills the
    card (client + status, "MMM dd, yyyy - time range (source)", the payment line
    "Paid X of Y - Balance Z" with the promo and its discount when there is one, the notes and the
    service lines) and enables the buttons by status: Start only for an Upcoming appointment TODAY,
    Reschedule / Cancel only for Upcoming. CellFormatting colors Status and Payment with
    Theme.StatusColor (and keeps white on a selected row), and the detail grid colors its Status.
    btnNew opens AppointmentForm; Reschedule opens the same form with RescheduleId set and moves
    the page to the new day afterwards; Cancel warns first ("the money paid upfront is forfeited"),
    warns again inside the 3-day window and asks for the reason with AppointmentForm.AskReason;
    a Staff user is stopped by Messages.Warn inside that window. lblSummary shows
    "N appointments - N massage". A double click on a row does nothing (the details are already
    there and a booking must never move by accident). Staff may do everything on this page.
  - Forms\AppointmentForm.cs: Load fills cboClient (ClientRepository.GetForCombo), cboTime
    (GetTimeSlots), cboSource, cboCategoryFilter ("All categories" + the real ones, the catalog is
    filtered in memory), dgvCatalog (GetActiveForBooking), cboPromo ("No promo" + GetActiveValid
    with the discount in the caption), the technician combo column (GetActiveStaff + a blank
    "Any available") and cboPayMethod, and sets dtpDate.MinDate to today. btnNewClient opens
    ClientEditForm and selects the new client. Adding a service appends a line with quantity 1, its
    duration and the package label ("Session 2 of 3", "New package (1 of 3)") and price from
    AppointmentRepository.TryGetPackagePlan (a new package charges the whole price, a later session
    of an existing package is 0.00); a service already in the list only gets +1 quantity when it is
    not a package (a package is refused with a short message, because it has one price and a fixed
    number of sessions). Remove deletes the selected line (looked up by service id, because the
    grid shows a sorted view). Every change recalculates the end time ("Ends 3:00 PM (60 min)"),
    the subtotal, the discount (PromotionRepository.CalculateDiscount; lblPromoNote names the
    eligible services or shows why the promo gives nothing), the total, the amount required now and
    fills txtPayAmount with it; it also runs the live availability check into lblSlotInfo (green
    "This slot is free: ...", amber first problem, muted for the two "your turn" messages). A
    letter typed into Qty is put back to 1 (CellValueChanged, with a DataError handler so no raw
    .NET dialog appears). grpPayment is disabled when nothing is required and always disabled in
    reschedule mode; GCash / Bank Transfer enable the reference box and the verified tick and cash
    clears them. Save validates through errorProvider1 (client, at least one service, a numeric
    amount), then shows EVERY problem from ValidateBooking in one Messages.Warn, then Create +
    a success message + DialogResult.OK. Reschedule mode loads the appointment, shows its promo,
    opens on its own date and locks everything except dtpDate and cboTime, retitles to
    "Reschedule Appointment" / "Save Changes" and asks for a reason (AskReason) when an Admin moves
    it inside the 3-day window. AskReason is a small modal built in code (as the prompt asked),
    with a multiline box, OK and Cancel.
  - Helpers\SmokeTest.cs: the three read-only checks (GetByDate(today, "All", "") runs,
    GetTimeSlots() returns exactly 24 slots, ValidateBooking without a client reports a problem).
    Nothing is written.
  - Self-test (temporary, REMOVED again - no --apptest switch is left in Program.cs or SmokeTest):
    round 1 created a walk-in appointment 10 days ahead at 2:00 PM for an existing demo client with
    one non-package service, checked the Billing row (5000.00 / Unpaid / None), the 'Created'
    history row, the day list ("2:00 PM - 2:30 PM | Cunanan, Jhon Rey | Traptox | Unassigned | - |
    Unpaid"), rescheduled it 2 days later at 3:30 PM (date and times moved, 1 'Rescheduled' row,
    still 'Upcoming'), saw "Start Service" correctly refused for a future day, cancelled it and
    checked 'Cancelled' on the appointment and the service plus 1 'Cancelled' history row: PASS.
    Round 2 checked the money rules: walk-in required now 0.00, Messenger 2500.00, a Messenger
    booking with nothing paid was refused, its bill read downpayment 2500.00 / Held / Partial with
    one 'Downpayment' payment, GCash without a reference and a tick was refused, and the promo
    booking (15% of 5000 = 750 off, 4250 total) read 'Applied' / 'Paid' / balance 0.00 with one
    'Final' payment 'Verified': PASS. Round 3 drove the real dialog without saving (78 catalog rows,
    24 slots, the client's consent + allergy alert, Add produced "Barbie Arms Botox | qty 1 |
    5000.00 | 30 min", "Ends 2:30 PM (30 min)", "This slot is free", the promo note explained that
    the promo applies to other services, and a second Add only raised the quantity to 2 with
    "Ends 3:00 PM"): PASS.
    Every row the self-test created was deleted again (Appointments, Billing, Payments,
    AppointmentHistory, AppointmentRooms, AppointmentStaff, AppointmentServices) and the two
    ActivityLog rows it wrote were removed too. The database now has 0 appointments, 0 billing rows,
    0 payments, 0 history rows, 0 rooms, 0 appointment services. Nothing was left behind.
  - LittleEreseSystemV3.csproj was NOT edited (no new file in this task; Db, BillingRepository and
    AppointmentRepository already existed and were listed).
- Standard clean build (bin, obj and .vs deleted first, MSBuild /t:Rebuild): 0 errors, 0 warnings.
  --smoketest: 44 passed, 0 failed, 0 skipped (the 3 new Appointment checks all PASS).
  --preview checked again for ucAppointments (the toolbar, the 9-column day grid reading
  "0 appointments - 0 massage" and the details card with "Select an appointment") and
  AppointmentForm (client row, schedule, the catalog with prices, the selected-services grid, the
  totals, the Payment group and the footer, nothing overlapping or truncated).
  The app was started once and closed again (no error.log).

- Step 15 (this task): Layout only (Designer work) for the Appointments page and the booking dialog.
  No logic, no SQL, no file or Session access: every wired event got an empty handler with
  "// TODO(logic prompt)", waiting for the logic prompt. Both files were empty shells before.
  - Controls\ucAppointments (.Designer.cs + .cs): AppBack page, 1000 x 640. pnlToolbar (Dock Top,
    92 px, Surface, two rows built with tlpToolbar) -
    row 1 flpNav (left): btnPrevDay + btnNextDay (glyph buttons, Font "Segoe MDL2 Assets" 12 pt,
    Text \uE76B / \uE76C, 34 x 30, secondary look), dtpDate (Format Short, 130 px),
    btnToday "Today", lblStatus + cboStatus (All, Upcoming, Ongoing, Completed, Cancelled,
    132 px), lblSearch "Client" + txtSearch (160 px, client name);
    row 2: flpButtons (btnNew "New Appointment" primary, btnStart "Start Service",
    btnReschedule "Reschedule", btnCancelAppointment "Cancel Appointment" danger, btnRefresh,
    all secondary except New and Cancel Appointment) and, right aligned in the same row,
    lblSummary (SmallFont, TextMuted, "0 appointments - 0 massage").
    Below it splitMain (Dock Fill, Orientation Horizontal, SplitterDistance 328 = about 60% of
    the 548 px body, Panel1MinSize 180, Panel2MinSize 150).
    Panel1: the card pnlGrid (Surface, FixedSingle, 1 px padding) with dgvAppointments (Dock Fill,
    Theme.StyleGrid look copied as literals, AutoGenerateColumns = false). Columns: colAppointmentId
    (hidden, appointment_id), colTime "Time" / time_range, colClient "Client" / client_name,
    colServices "Services" / services, colTech "Technician" / staff_names, colRoom "Room" /
    room_names, colSource "Source" / booking_source, colStatus "Status" / status, colPayment
    "Payment" / payment_status (Fill weights 13/17/27/16/11/9/11/11).
    Panel2: the details card pnlDetails (Surface, FixedSingle, 16 px padding) with tlpDetails
    (2 columns: 356 px fixed + rest) holding tlpDetailInfo (lblDetailClient BoldFont,
    lblDetailSchedule, lblDetailPayment, spanned over both rows), dgvDetailServices
    (colDetailService "Service" / service_name; colDetailQty "Qty" / quantity, centered;
    colDetailPrice "Price" / price N2; colDetailStaff "Technician" / staff_name;
    colDetailStatus "Status" / status) and below it tlpNotes (lblNotesTitle "Notes" +
    txtDetailNotes read-only multiline).
    Events wired: Load, btnPrevDay_Click, btnNextDay_Click, btnToday_Click, dtpDate_ValueChanged,
    cboStatus_SelectedIndexChanged, txtSearch_TextChanged, btnNew_Click, btnStart_Click,
    btnReschedule_Click, btnCancelAppointment_Click, btnRefresh_Click,
    dgvAppointments_SelectionChanged, dgvAppointments_CellFormatting,
    dgvAppointments_CellDoubleClick, dgvDetailServices_CellFormatting.
  - Forms\AppointmentForm (.Designer.cs + .cs): FixedDialog, CenterParent, no minimize/maximize,
    AcceptButton btnSave, CancelButton + DialogResult.Cancel on btnCancel, errorProvider1,
    white header (lblTitle "New Appointment" HeadingFont) + 1 px Border line, white footer and the
    AppBack content inside pnlScroll (AutoScroll = true). Client 980 x 640: the prompt's
    1040 x 720 was shrunk to satisfy the "dialogs at most 1000 x 640, taller content goes into a
    scrollable panel" rule of Additions (Step 3), and everything that does not fit on a
    1366 x 768 screen is reached with the scrollbar.
    Content (tlpContent, one full-width row per block, visual order = TabIndex order):
    flpClient (lblClient + cboClient 340 px with AutoCompleteMode SuggestAppend /
    AutoCompleteSource ListItems + btnNewClient "+ New"), lblClientInfo (muted: consent status and
    medical alerts), flpSchedule (lblDate + dtpDate, lblTime + cboTime DropDownList, lblSource +
    cboSource Walk-in / Messenger / Text / Phone, lblEndTime "Ends 11:30 AM"), lblSlotInfo
    (warning colored, 2 lines: availability), lblCategoryFilter + cboCategoryFilter,
    dgvCatalog (colCatServiceId hidden / service_id; colCatName "Service" / service_name;
    colCatPrice "Price" / price N2; colCatDuration "Min" / duration_minutes),
    btnAddService "Add >" (primary) + btnRemoveService "Remove", dgvSelected
    (colSelServiceId hidden / service_id; colSelName "Service" / service_name; colSelQty "Qty" /
    quantity; colSelPrice "Price" / price N2; colSelDuration "Min" / duration_minutes;
    colSelPackage "Package" / package_label; colSelTechnician a DataGridViewComboBoxColumn
    "Technician" / technician_id) - only Qty and Technician are editable,
    lblPromo + cboPromo (DropDownList), lblPromoNote (muted, 2 lines), lblSubtotal, lblDiscount,
    lblTotal (BoldFont), lblRequired (Success green: the amount required now),
    grpPayment "Payment now" (lblPayMethod + cboPayMethod Cash / GCash / Bank Transfer,
    lblPayReference + txtPayReference, lblPayAmount + txtPayAmount,
    chkPayVerified "I verified this payment in the GCash / bank app", muted lblPayNote),
    lblNotes + txtNotes (multiline, 78 px) and the footer btns (btnSave "Book Appointment" primary,
    btnCancel "Close" secondary).
    Events wired: Load, btnSave_Click, btnCancel_Click, btnNewClient_Click, btnAddService_Click,
    btnRemoveService_Click, cboClient_SelectedIndexChanged, dtpDate_ValueChanged,
    cboTime_SelectedIndexChanged, cboSource_SelectedIndexChanged,
    cboCategoryFilter_SelectedIndexChanged, cboPromo_SelectedIndexChanged,
    cboPayMethod_SelectedIndexChanged, dgvCatalog_CellDoubleClick, dgvSelected_CellValueChanged,
    dgvSelected_CurrentCellDirtyStateChanged.
  - Fix rounds from the previews (2 used): (1) on the page the toolbar TabIndex values 8/9 skipped
    numbers between the two rows - they are now 1 and 2, and lblTotal was 11 pt in the dialog
    (taller than its 26 px row), so it went back to BoldFont 10 pt; (2) "Book Appointment" was cut
    in the footer, so the footer buttons moved left and btnSave was widened to 132 px.
  - LittleEreseSystemV3.csproj was NOT edited (all four files already existed and were listed).
- Standard clean build (bin, obj and .vs deleted first, MSBuild /t:Rebuild): 0 errors, 0 warnings.
  --smoketest: 41 passed, 0 failed, 0 skipped.
  --preview checked for ucAppointments (two toolbar rows with the glyph buttons, date picker,
  Today, the status and client filters and the "0 appointments - 0 massage" summary, the 9-column
  day grid and the details card with "Select an appointment", Schedule, Payment, the 5-column
  service grid and Notes - nothing overlapping or truncated) and AppointmentForm (client row,
  schedule row with "Ends 11:30 AM", the two service grids, promo, totals, Payment now group,
  Notes and the complete footer, scrollbar present for 768 px screens).
  The app was started once and closed again (no error.log).

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
- The Appointments module is COMPLETE for booking, rescheduling, cancelling and "Start Service"
  (AppointmentRepository + the transaction helpers in Db, ucAppointments, AppointmentForm). The
  promo discount and the full-payment-upfront rule are wired through
  PromotionRepository.CalculateDiscount, so the Appointments side of the promotions TODO above is
  done; the same helpers are ready for the Billing screen
  (BillingRepository.GetByAppointment + RefreshBalance).
- The Billing screen still needs its own prompt: BillingRepository currently only creates a bill and
  records a payment INSIDE a booking transaction, so it still needs the read methods of the Billing
  page, the PaymentForm logic and the Completing-a-service step (AppointmentServices 'Completed',
  the package session counter, the commission rows when a line is completed at billing).
- The Dashboard screen (ucDashboard with ucCalendarMonth) and the remaining modules
  (Billing, Commissions, Reports, File Maintenance, Activity Log) still need their layout prompts;
  ucClients has a layout now but no logic yet, so it shows an empty grid and the
  placeholder details card ("Select a client").
- POLISH items left for the owner: the lock out limits (5 tries / 30 seconds) in LoginForm,
  the "Log out now?" wording, the PhoneRegex (mobile numbers only), the "-" placeholder for an
  empty contact number in Settings and the version format (3 parts) in Settings.
- POLISH item from the Clients logic: PackageForm lists the services whose
  default_total_sessions is greater than 1 first and only falls back to every Active service when
  none is flagged as a package. Confirm with the owner which services may be sold as packages.

## Known issues
- The booking dialog cannot assign a room yet because RoomServiceCategories is EMPTY in the current
  database: every category has no room linked, so BuildRoomPlan silently assigns none (a POLISH
  comment says why). The owner must link the rooms of the shop to the categories in File
  Maintenance before the room rule and the "Room" column can show anything. Nothing else in the
  booking is affected (the self-test ran with no rooms and the appointment was created, cancelled
  and listed correctly).
- An appointment can only be started ("Start Service") on its own day, so a booking made for a
  future date shows the button switched off - that is the rule, not a bug.
- There is no way to move an appointment from 'Ongoing' to 'Completed' yet: that happens at
  Billing (the next module), which also completes the service lines, the package sessions and the
  commissions.
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
