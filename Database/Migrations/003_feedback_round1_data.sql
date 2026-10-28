-- 003: standard durations, Tirzepatide packages, rooms and their links. Safe to run twice.
-- 1. Durations on the 30-minute grid (round up), then two longer services
UPDATE dbo.Services SET duration_minutes = CEILING(duration_minutes / 30.0) * 30
WHERE duration_minutes % 30 <> 0;
GO
UPDATE dbo.Services SET duration_minutes = 90
WHERE service_name IN ('Full Legs Hair Removal', 'Lash Lift w/ Tint') AND duration_minutes < 90;
GO
-- 2. A gluta drip keeps the chair for an hour but the aesthetician is only busy for 15 minutes
UPDATE s SET staff_busy_minutes = 15
FROM dbo.Services s INNER JOIN dbo.ServiceCategories c ON c.category_id = s.category_id
WHERE c.category_name = 'Glutathione' AND s.duration_minutes >= 60 AND s.staff_busy_minutes IS NULL;
GO
-- 3. Tirzepatide: 4, 6 and 8 weekly sessions, 30 minutes each (the price is for the whole package)
UPDATE dbo.Services
SET service_name = 'Tirzepatide (4 sessions)', default_total_sessions = 4, duration_minutes = 30
WHERE service_name = 'Tirzepatide';
GO
INSERT INTO dbo.Services (category_id, service_name, description, price, duration_minutes, default_total_sessions)
SELECT c.category_id, v.service_name, 'Weekly injection. The price is for the whole package.', v.price, 30, v.sessions
FROM (VALUES ('Tirzepatide (4 sessions)', 5000.00, 4), ('Tirzepatide (6 sessions)', 7500.00, 6), ('Tirzepatide (8 sessions)', 10000.00, 8)) AS v(service_name, price, sessions)
INNER JOIN dbo.ServiceCategories c ON c.category_name = 'Slimming'
WHERE NOT EXISTS (SELECT 1 FROM dbo.Services s WHERE s.service_name = v.service_name);
GO
-- 4. Rooms: 10 chairs, 3 diode rooms, 3 facial beds
UPDATE dbo.Rooms SET capacity = 10 WHERE room_name = 'Aesthetic Lobby';
UPDATE dbo.Rooms SET capacity = 3 WHERE room_name = 'Diode Machine Lobby';
UPDATE dbo.Rooms SET capacity = 3 WHERE room_name = 'Facial Room';
UPDATE dbo.Rooms SET status = 'Unavailable' WHERE room_name = 'Diode Machine Room';
GO
INSERT INTO dbo.RoomServiceCategories (room_id, category_id)
SELECT r.room_id, c.category_id
FROM (VALUES ('Aesthetic Lobby','Botox'),('Aesthetic Lobby','Glutathione'),('Aesthetic Lobby','Slimming'),('Aesthetic Lobby','Nails'),
             ('Diode Machine Lobby','Hair Removal and Whitening'),
             ('Facial Room','Facial'),('Facial Room','Face Beautification'),
             ('Massage Room','Massage - Feet'),('Massage Room','Massage - Hand'),('Massage Room','Massage - Full Body')) AS m(room_name, category_name)
INNER JOIN dbo.Rooms r ON r.room_name = m.room_name
INNER JOIN dbo.ServiceCategories c ON c.category_name = m.category_name
WHERE NOT EXISTS (SELECT 1 FROM dbo.RoomServiceCategories x WHERE x.room_id = r.room_id AND x.category_id = c.category_id);
GO
