Select * From TicketCategoryId

INSERT INTO TicketCategory (Name) 
VALUES
('Movie'), ('Train'), ('Flight'), ('Ship'), ('Hotel')

INSERT INTO Tickets (TicketCode, TicketName, TicketCategoryId, Price, Quota, EventDate)
VALUES
    ('M001', 'Avengers: Endgame', 6, 6, 100, '2025-04-10 19:00:00'),
    ('M002', 'Spider-Man: No Way Home', 6, 7, 120, '2025-04-15 20:00:00'),
    ('M003', 'The Batman', 6, 7, 300, '2025-04-18 18:30:00'),
    ('M004', 'Fast & Furious 10', 6, 8, 150, '2025-04-20 21:00:00'),
    ('M005', 'Dune: Part Two', 6, 9, 220, '2025-04-22 19:30:00'),
    
    ('TR001', 'Kereta Argo Parahyangan - Jakarta ke Bandung', 7, 150, 200, '2025-04-05 08:00:00'),
    ('TR002', 'Kereta Taksaka - Yogyakarta ke Jakarta', 7, 175, 180, '2025-04-08 09:30:00'),
    ('TR003', 'Kereta Malabar - Bandung ke Malang', 7, 160, 190, '2025-04-12 10:00:00'),
    
    ('FL001', 'Penerbangan Jakarta ke Bali', 8, 1200, 150, '2025-04-15 06:00:00'),
    ('FL002', 'Penerbangan Surabaya ke Singapore', 8, 2500, 130, '2025-04-18 08:45:00'),
    ('FL003', 'Penerbangan Medan ke Kuala Lumpur', 8, 1800, 120, '2025-04-20 07:30:00'),
    
    ('SH001', 'Kapal Laut Jakarta ke Surabaya', 9, 300, 250, '2025-04-25 15:00:00'),
    ('SH002', 'Kapal Laut Makassar ke Balikpapan', 9, 350, 220, '2025-04-28 14:00:00'),
    ('SH003', 'Kapal Laut Batam ke Tanjung Pinang', 9, 250, 180, '2025-05-01 13:00:00'),
    
    ('H001', 'Hotel Grand Indonesia - Jakarta (1 Malam)', 10, 100, 50, '2025-04-10 14:00:00'),
    ('H002', 'Hotel Merlion - Singapore (1 Malam)', 10, 150, 40, '2025-04-12 15:00:00'),
    ('H003', 'Hotel Bali Resort - Bali (1 Malam)', 10, 120, 60, '2025-04-14 13:00:00'),
    ('H004', 'Hotel JW Marriott - Surabaya (1 Malam)', 10, 110, 45, '2025-04-16 14:00:00'),
    ('H005', 'Hotel Shangri-La - Jakarta (1 Malam)', 10, 200, 30, '2025-04-18 12:00:00');

