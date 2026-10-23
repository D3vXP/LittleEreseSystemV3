/* =====================================================================
   Migration 01 - PromotionServices
   Appointment and Billing System for Little Erese's Aesthetics and Spa
   Database: LittleEresesDB   |   Target: Microsoft SQL Server (SSMS)

   Adds the table that links a promotion to the services it applies to.
   A promotion with no rows here applies to the whole bill.

   Safe to run more than once: the table is only created when it does not
   exist yet, so an existing database is never touched twice.
   ===================================================================== */

USE LittleEresesDB;
GO

IF OBJECT_ID('dbo.PromotionServices') IS NULL
BEGIN
    CREATE TABLE PromotionServices (
        promo_service_id INT IDENTITY(1,1) PRIMARY KEY,
        promo_id         INT NOT NULL,
        service_id       INT NOT NULL,
        CONSTRAINT FK_PS_Promotion FOREIGN KEY (promo_id)   REFERENCES Promotions(promo_id),
        CONSTRAINT FK_PS_Service   FOREIGN KEY (service_id) REFERENCES Services(service_id),
        CONSTRAINT UQ_PS UNIQUE (promo_id, service_id)
    );
END
GO
