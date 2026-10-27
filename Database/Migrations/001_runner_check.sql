-- 001_runner_check.sql
-- The first numbered update file. It changes nothing: it only proves that the migration runner
-- in DatabaseBootstrapper.ApplyMigrations() finds the folder, splits on GO, runs the file inside
-- one transaction and writes its name into SchemaVersion.
-- Every later schema change is a new file here, numbered, safe to run twice.
SELECT 1;
