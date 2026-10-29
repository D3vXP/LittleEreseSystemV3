-- 005: a bill with nothing to pay (a later session of a package costs 0.00) is 'Paid',
-- because nothing is left to collect on it. Safe to run twice.
UPDATE dbo.Billing SET payment_status = 'Paid', updated_at = GETDATE()
WHERE total_amount = 0 AND payment_status = 'Unpaid';
GO
