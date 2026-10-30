-- 007: optional middle name of a client. Safe to run twice.
IF COL_LENGTH('dbo.Clients', 'middle_name') IS NULL
    ALTER TABLE dbo.Clients ADD middle_name VARCHAR(50) NULL;
GO
