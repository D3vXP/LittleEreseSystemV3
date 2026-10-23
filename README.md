# Little Erese's Appointment and Billing System

Capstone project - STI College Tarlac. Offline Windows Forms app (.NET Framework 4.8) with SQL Server LocalDB.

## Setup
1. Install Visual Studio with the ".NET desktop development" workload and SQL Server LocalDB.
2. Run `Database/LittleEreses_Database.sql`, then `Database/SeedData.sql` (SSMS or any SQL client, server `(localdb)\MSSQLLocalDB`).
   An existing database that was created before the promotion-to-services feature only needs
   `Database/Migration_01_PromotionServices.sql` once (the schema has 18 tables now).
3. Open `LittleEreseSystemV3.slnx`, build, and run.

See `PROJECT_RULES.md` for the coding rules and `PROGRESS.md` for the current status.
