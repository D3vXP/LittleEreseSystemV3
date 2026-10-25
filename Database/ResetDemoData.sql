/* =====================================================================
   ResetDemoData.sql - empties the DEMO tables only.
   Little Erese's Aesthetics and Spa

   WHAT IT DOES
     Deletes every row of the 12 tables that hold demo / business records, in the
     order the foreign keys require (children first, parents last). IDENTITY values
     are NOT reseeded on purpose (your request: "RESEED is not needed").

   WHAT IT NEVER TOUCHES
     Users, ServiceCategories, Services, Rooms, RoomServiceCategories, PromotionServices
     (PromotionServices has no rows once Promotions is empty, so it is left alone).

   WHY YOU MAY NEED IT
     The demo data is loaded once, automatically, only when the Clients table is still
     empty. If the database already has clients, empty these tables with this script and
     start the app again: the demo data is written on that start.

   HOW TO RUN
     Open SSMS, connect to (localdb)\MSSQLLocalDB, make sure the selected database is
     LittleEresesDB, open this file and press F5 (Execute). It is NOT run by the app.

   WARNING: this cannot be undone. Back up first (File Maintenance > Backup Now) if the
   database holds anything you want to keep.
   ===================================================================== */

USE LittleEresesDB;
GO

/* XACT_ABORT ON: if ANY delete fails, the whole transaction is rolled back, so the
   tables are never left half emptied. */
SET XACT_ABORT ON;
GO

BEGIN TRANSACTION;
GO

/* 1. Audit trail (child of Users only, but it points at the bookings it describes). */
DELETE FROM ActivityLog;
GO

/* 2. Commissions -> AppointmentServices, Users, Payments. */
DELETE FROM Commissions;
GO

/* 3. Payments -> Billing. */
DELETE FROM Payments;
GO

/* 4. AppointmentHistory -> Appointments. */
DELETE FROM AppointmentHistory;
GO

/* 5. AppointmentRooms -> Appointments, AppointmentServices, Rooms. */
DELETE FROM AppointmentRooms;
GO

/* 6. AppointmentStaff -> Appointments, AppointmentServices, Users. */
DELETE FROM AppointmentStaff;
GO

/* 7. AppointmentServices -> Appointments, Services, ClientPackages. */
DELETE FROM AppointmentServices;
GO

/* 8. Billing -> Appointments, Promotions. */
DELETE FROM Billing;
GO

/* 9. Appointments -> Clients, Users. */
DELETE FROM Appointments;
GO

/* 10. ClientPackages -> Clients, Services. */
DELETE FROM ClientPackages;
GO

/* 11. Clients (the table the demo seeder checks). */
DELETE FROM Clients;
GO

/* 12. Promotions (last: Billing referenced it). */
DELETE FROM Promotions;
GO

COMMIT TRANSACTION;
GO

/* Proof that the demo tables are empty and the master data is untouched.
   Expected: every "rows" value 0, and Services / ServiceCategories / Rooms / Users unchanged. */
SELECT 'ActivityLog' AS table_name, COUNT(*) AS rows_left FROM ActivityLog
UNION ALL SELECT 'Commissions',         COUNT(*) FROM Commissions
UNION ALL SELECT 'Payments',            COUNT(*) FROM Payments
UNION ALL SELECT 'AppointmentHistory',  COUNT(*) FROM AppointmentHistory
UNION ALL SELECT 'AppointmentRooms',    COUNT(*) FROM AppointmentRooms
UNION ALL SELECT 'AppointmentStaff',    COUNT(*) FROM AppointmentStaff
UNION ALL SELECT 'AppointmentServices', COUNT(*) FROM AppointmentServices
UNION ALL SELECT 'Billing',             COUNT(*) FROM Billing
UNION ALL SELECT 'Appointments',        COUNT(*) FROM Appointments
UNION ALL SELECT 'ClientPackages',      COUNT(*) FROM ClientPackages
UNION ALL SELECT 'Clients',             COUNT(*) FROM Clients
UNION ALL SELECT 'Promotions',          COUNT(*) FROM Promotions
UNION ALL SELECT 'Users (kept)',        COUNT(*) FROM Users
UNION ALL SELECT 'Services (kept)',     COUNT(*) FROM Services
UNION ALL SELECT 'ServiceCategories (kept)', COUNT(*) FROM ServiceCategories
UNION ALL SELECT 'Rooms (kept)',        COUNT(*) FROM Rooms
UNION ALL SELECT 'RoomServiceCategories (kept)', COUNT(*) FROM RoomServiceCategories;
GO
