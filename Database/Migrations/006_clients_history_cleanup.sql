-- 006_clients_history_cleanup.sql
-- Clients.treatment_history now means ONLY "treatments received at other clinics in the past
-- 6 months" (typed once at intake). The shop's own work is read from the completed visits.
-- The old lines the app itself appended ("MMM dd, yyyy - Service name (Staff name)") are removed
-- ONCE, in C# (ClientRepository.RemoveGeneratedHistoryLines), right after this file is applied.
-- WHY this file changes nothing itself: the removal has to read one line at a time, which is easy
-- in C# and awkward in SQL. This file only proves the migration ran, exactly like 001.
SELECT 1;
