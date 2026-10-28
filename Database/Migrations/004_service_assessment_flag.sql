-- 004: which services need a nurse assessment first. Safe to run twice.
IF COL_LENGTH('dbo.Services', 'requires_assessment') IS NULL
    ALTER TABLE dbo.Services ADD requires_assessment BIT NOT NULL
        CONSTRAINT DF_Services_RequiresAssessment DEFAULT 0;
GO
UPDATE s SET requires_assessment = 1
FROM dbo.Services s INNER JOIN dbo.ServiceCategories c ON c.category_id = s.category_id
WHERE c.category_name IN ('Botox', 'Glutathione', 'Slimming') AND s.requires_assessment = 0;
GO
UPDATE dbo.Services SET requires_assessment = 1
WHERE (service_name LIKE '%PRP%' OR service_name LIKE '%Vampire%') AND requires_assessment = 0;
GO
