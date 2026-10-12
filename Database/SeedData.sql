/* =====================================================================
   SeedData.sql - Service price list from Little Erese's proposal (Table 1)
   Run AFTER LittleEreses_Database.sql. Safe to run once: it does nothing
   if the Services table already has rows.
   NOTE: duration_minutes values are reasonable placeholders; edit as needed.
   ===================================================================== */
USE LittleEresesDB;
GO

IF NOT EXISTS (SELECT 1 FROM Services)
BEGIN
    INSERT INTO Services (category_id, service_name, price, duration_minutes, default_total_sessions)
    SELECT c.category_id, v.service_name, v.price, v.duration_minutes, v.sessions
    FROM (VALUES
        ('Botox', 'Traptox', 5000.00, 30, 1),
        ('Botox', 'Sweatox', 5000.00, 30, 1),
        ('Botox', 'Barbie Arms Botox', 5000.00, 30, 1),
        ('Botox', 'Jawtox', 5000.00, 30, 1),
        ('Botox', 'Palmartox', 5000.00, 30, 1),
        ('Botox', 'Face Botox', 8000.00, 30, 1),
        ('Glutathione', 'Gluta Push', 400.00, 15, 1),
        ('Glutathione', 'Glutax 2MGx Drip', 600.00, 60, 1),
        ('Glutathione', 'NAD IV Therapy', 800.00, 60, 1),
        ('Glutathione', 'Ultimate Whitening Gluta Drip', 1000.00, 60, 1),
        ('Glutathione', 'Cindyrella Magical Drip', 1300.00, 60, 1),
        ('Glutathione', 'Lumin Seoul Gluta Drip', 1300.00, 60, 1),
        ('Glutathione', 'Sakura Kiyoshi Drip', 1300.00, 60, 1),
        ('Facial', 'Pimple Inject', 100.00, 15, 1),
        ('Facial', 'Diamond Peel', 400.00, 45, 1),
        ('Facial', 'Hydrafacial', 600.00, 60, 1),
        ('Facial', 'Korean BB Glow + Blush Facial', 1000.00, 60, 1),
        ('Facial', 'Anti Melasma Facial', 1300.00, 60, 1),
        ('Facial', 'Meso Acne Facial', 1500.00, 60, 1),
        ('Facial', 'Rejuvenating Facial', 1500.00, 60, 1),
        ('Facial', 'PRP/Vampire Facial', 3000.00, 60, 3),
        ('Facial', 'Fractional CO2 Laser', 3000.00, 60, 1),
        ('Facial', 'Microtox', 4000.00, 60, 1),
        ('Facial', 'Rejuran', 5000.00, 60, 1),
        ('Facial', 'Mesonanomax', 5000.00, 60, 1),
        ('Slimming', 'Emsculpt', 800.00, 60, 1),
        ('Slimming', 'Mesolipo', 2500.00, 60, 1),
        ('Slimming', 'Mesolipo Lemon Bottle', 4000.00, 60, 1),
        ('Slimming', 'Cryolipo', 4000.00, 60, 1),
        ('Slimming', 'Tirzepatide', 5000.00, 15, 1),
        ('Slimming', 'Barbie Arms w/ Botox', 7000.00, 60, 1),
        ('Slimming', '9D Hifu', 7000.00, 60, 1),
        ('Slimming', '9D Hifu Body', 10000.00, 90, 1),
        ('Hair Removal and Whitening', 'Upper Lip Hair Removal', 400.00, 15, 1),
        ('Hair Removal and Whitening', 'Lower Lip Hair Removal', 400.00, 15, 1),
        ('Hair Removal and Whitening', 'Full Lips Hair Removal', 600.00, 20, 1),
        ('Hair Removal and Whitening', 'Full Arms Hair Removal', 800.00, 45, 1),
        ('Hair Removal and Whitening', 'Lower Legs Hair Removal', 800.00, 45, 1),
        ('Hair Removal and Whitening', 'Full Legs Hair Removal', 1000.00, 60, 1),
        ('Hair Removal and Whitening', 'Laser Hair Removal w/ Underarm Whitening', 3000.00, 45, 1),
        ('Hair Removal and Whitening', 'Underarm Whitening', 5000.00, 45, 1),
        ('Face Beautification', 'Lash Lift', 250.00, 45, 1),
        ('Face Beautification', 'Lash Lift w/ Tint', 350.00, 60, 1),
        ('Face Beautification', 'Eyelash Extensions', 500.00, 90, 1),
        ('Face Beautification', 'Brow Tint', 600.00, 30, 1),
        ('Face Beautification', 'Eyeliner Tattoo', 2000.00, 90, 1),
        ('Face Beautification', 'Microblading', 3000.00, 120, 1),
        ('Face Beautification', 'Microshading', 3000.00, 120, 1),
        ('Face Beautification', 'Combrows', 3000.00, 120, 1),
        ('Face Beautification', 'Hairstroke', 3000.00, 120, 1),
        ('Face Beautification', 'Ombre', 3000.00, 120, 1),
        ('Nails', 'Classic Manicure', 150.00, 30, 1),
        ('Nails', 'Classic Pedicure', 150.00, 45, 1),
        ('Nails', 'Nail Removal', 200.00, 20, 1),
        ('Nails', 'Gel Manicure (Classic)', 300.00, 45, 1),
        ('Nails', 'Gel Manicure (Basic Design)', 400.00, 60, 1),
        ('Nails', 'Gel Manicure (Full Design)', 500.00, 75, 1),
        ('Nails', 'Gel Pedicure', 350.00, 60, 1),
        ('Nails', 'Nail Extension (Classic)', 600.00, 90, 1),
        ('Nails', 'Nail Extension (Full Design)', 1000.00, 120, 1),
        ('Nails', 'Nail Extension (Full Design w/ Gems)', 1500.00, 150, 1),
        ('Nails', 'Polygel Extension (Classic)', 1000.00, 90, 1),
        ('Nails', 'Polygel Extension (Full Design)', 1300.00, 120, 1),
        ('Nails', 'Polygel Extension (Full Design w/ Gems)', 1500.00, 150, 1),
        ('Massage - Feet', 'Foot Spa', 400.00, 45, 1),
        ('Massage - Feet', 'Foot Massage', 300.00, 30, 1),
        ('Massage - Feet', 'Foot Spa w/ Massage', 500.00, 60, 1),
        ('Massage - Hand', 'Hand Massage', 300.00, 30, 1),
        ('Massage - Hand', 'Hand Massage w/ Spa', 400.00, 45, 1),
        ('Massage - Full Body', 'Full Body Massage w/ Scrub (2 hours)', 800.00, 120, 1),
        ('Massage - Full Body', 'Swedish Massage (1 hour)', 500.00, 60, 1),
        ('Massage - Full Body', 'Swedish Massage (2 hours)', 600.00, 120, 1),
        ('Massage - Full Body', 'Full Body Massage w/ Ventosa & Hotstone (1.5 hours)', 600.00, 90, 1),
        ('Massage - Full Body', 'Full Body Massage w/ Ventosa & Hotstone (2 hours)', 800.00, 120, 1),
        ('Massage - Full Body', 'Thai Herbal Massage (1.5 hours)', 600.00, 90, 1),
        ('Massage - Full Body', 'Thai Herbal Massage (2 hours)', 800.00, 120, 1),
        ('Massage - Full Body', 'Full Body Massage w/ Sauna (2 hours)', 1500.00, 120, 1)
    ) AS v(category_name, service_name, price, duration_minutes, sessions)
    JOIN ServiceCategories c ON c.category_name = v.category_name;
END
GO
