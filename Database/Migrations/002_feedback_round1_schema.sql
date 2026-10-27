-- 002: schema for the tester feedback round 1. Safe to run twice.
-- 1. Staff job type
IF COL_LENGTH('dbo.Users', 'staff_type') IS NULL
    ALTER TABLE dbo.Users ADD staff_type VARCHAR(30) NULL;
GO
IF NOT EXISTS (SELECT 1 FROM sys.check_constraints WHERE name = 'CK_Users_StaffType')
    ALTER TABLE dbo.Users ADD CONSTRAINT CK_Users_StaffType
        CHECK (staff_type IS NULL OR staff_type IN ('Aesthetician','Massage Therapist','Nurse','Other'));
GO
UPDATE dbo.Users
SET staff_type = CASE
        WHEN full_name LIKE 'Aesthetician%'      THEN 'Aesthetician'
        WHEN full_name LIKE 'Massage Therapist%' THEN 'Massage Therapist'
        WHEN full_name LIKE 'Nurse%'             THEN 'Nurse'
        ELSE 'Other' END
WHERE role = 'Staff' AND staff_type IS NULL;
GO
-- 2. Services: how long the staff member is busy, and days between sessions of a package
IF COL_LENGTH('dbo.Services', 'staff_busy_minutes') IS NULL
    ALTER TABLE dbo.Services ADD staff_busy_minutes INT NULL
        CONSTRAINT CK_Services_StaffBusy CHECK (staff_busy_minutes IS NULL OR staff_busy_minutes > 0);
GO
IF COL_LENGTH('dbo.Services', 'session_interval_days') IS NULL
    ALTER TABLE dbo.Services ADD session_interval_days INT NOT NULL
        CONSTRAINT DF_Services_Interval DEFAULT 7
        CONSTRAINT CK_Services_Interval CHECK (session_interval_days >= 1);
GO
-- 3. Every service line gets its own times
IF COL_LENGTH('dbo.AppointmentServices', 'start_time') IS NULL
    ALTER TABLE dbo.AppointmentServices ADD start_time TIME NULL, end_time TIME NULL, busy_end_time TIME NULL;
GO
IF COL_LENGTH('dbo.AppointmentServices', 'assessed_by') IS NULL
    ALTER TABLE dbo.AppointmentServices ADD assessed_by INT NULL
        CONSTRAINT FK_AS_AssessedBy FOREIGN KEY REFERENCES dbo.Users(user_id);
GO
;WITH L AS (
    SELECT asv.appointment_service_id,
           a.start_time AS appt_start,
           s.duration_minutes * asv.quantity AS line_minutes,
           SUM(s.duration_minutes * asv.quantity) OVER (
               PARTITION BY asv.appointment_id ORDER BY asv.appointment_service_id
               ROWS BETWEEN UNBOUNDED PRECEDING AND CURRENT ROW) AS running_total
    FROM dbo.AppointmentServices asv
    INNER JOIN dbo.Appointments a ON a.appointment_id = asv.appointment_id
    INNER JOIN dbo.Services s ON s.service_id = asv.service_id
    WHERE asv.start_time IS NULL
)
UPDATE asv
SET start_time    = CAST(DATEADD(MINUTE, L.running_total - L.line_minutes, L.appt_start) AS TIME),
    end_time      = CAST(DATEADD(MINUTE, L.running_total, L.appt_start) AS TIME),
    busy_end_time = CAST(DATEADD(MINUTE, L.running_total, L.appt_start) AS TIME)
FROM dbo.AppointmentServices asv
INNER JOIN L ON L.appointment_service_id = asv.appointment_service_id;
GO
-- 4. Promotions: a promotional price, and a third discount type
IF COL_LENGTH('dbo.Promotions', 'promo_price') IS NULL
    ALTER TABLE dbo.Promotions ADD promo_price DECIMAL(10,2) NULL
        CONSTRAINT CK_Promotions_PromoPrice CHECK (promo_price IS NULL OR promo_price >= 0);
GO
DECLARE @old SYSNAME;
SELECT @old = cc.name FROM sys.check_constraints cc
WHERE cc.parent_object_id = OBJECT_ID('dbo.Promotions')
  AND cc.definition LIKE '%discount_type%' AND cc.definition NOT LIKE '%Promo Price%';
IF @old IS NOT NULL
BEGIN
    DECLARE @sql NVARCHAR(400) = N'ALTER TABLE dbo.Promotions DROP CONSTRAINT ' + QUOTENAME(@old);
    EXEC (@sql);
END
GO
IF NOT EXISTS (SELECT 1 FROM sys.check_constraints WHERE name = 'CK_Promotions_DiscountType')
    ALTER TABLE dbo.Promotions ADD CONSTRAINT CK_Promotions_DiscountType
        CHECK (discount_type IN ('Percentage','Fixed','Promo Price'));
GO
-- 5. Clients: the health checkboxes of the paper form (comma separated)
IF COL_LENGTH('dbo.Clients', 'health_conditions') IS NULL
    ALTER TABLE dbo.Clients ADD health_conditions VARCHAR(MAX) NULL;
GO
