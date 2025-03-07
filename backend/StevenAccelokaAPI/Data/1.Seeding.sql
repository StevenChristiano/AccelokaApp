Select * from Tickets
Select * From BookedTickets
Select * From Bookings

INSERT INTO TicketCategory (Name) VALUES 
('Concert'), ('Sports'), ('Theater'), ('Conference'), ('Festival');

INSERT INTO Tickets (TicketCode, TicketName, EventDate, Quota, Price, TicketCategoryId) VALUES
('C001', 'Coldplay Live', '2025-06-15 19:30:00', 500, 120.00, 1),
('C002', 'Taylor Swift Eras Tour', '2025-07-10 20:00:00', 600, 150.00, 1),
('C003', 'Ed Sheeran Divide Tour', '2025-08-20 18:45:00', 400, 100.00, 1),
('C004', 'BTS World Tour', '2025-09-05 21:00:00', 700, 130.00, 1),
('C005', 'Justin Bieber Justice Tour', '2025-10-10 19:15:00', 550, 110.00, 1),

('S001', 'FIFA World Cup Final', '2025-12-18 17:00:00', 800, 200.00, 2),
('S002', 'NBA Finals Game 7', '2025-06-25 19:30:00', 750, 180.00, 2),
('S003', 'Wimbledon Men’s Final', '2025-07-14 15:00:00', 500, 160.00, 2),
('S004', 'Formula 1 Grand Prix', '2025-08-02 14:00:00', 600, 175.00, 2),
('S005', 'UFC Championship Fight', '2025-09-12 22:00:00', 450, 190.00, 2),

('T001', 'The Lion King Musical', '2025-05-20 18:00:00', 300, 90.00, 3),
('T002', 'Hamilton Broadway Show', '2025-06-10 19:00:00', 350, 95.00, 3),
('T003', 'Phantom of the Opera', '2025-07-05 20:30:00', 280, 85.00, 3),
('T004', 'Les Misérables', '2025-08-22 19:45:00', 320, 88.00, 3),
('T005', 'Wicked the Musical', '2025-09-15 18:15:00', 400, 92.00, 3),

('F001', 'Tomorrowland Music Festival', '2025-07-22 12:00:00', 1000, 300.00, 5),
('F002', 'Coachella Weekend Pass', '2025-04-12 10:00:00', 1200, 280.00, 5),
('F003', 'Glastonbury Festival', '2025-06-26 13:00:00', 800, 250.00, 5),
('F004', 'Rock in Rio', '2025-09-04 16:30:00', 900, 270.00, 5),
('F005', 'Burning Man Festival', '2025-08-30 09:00:00', 500, 350.00, 5);
