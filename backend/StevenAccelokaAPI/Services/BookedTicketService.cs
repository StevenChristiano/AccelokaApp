//using Microsoft.EntityFrameworkCore;
//using StevenAccelokaAPI.Models;
//using StevenAccelokaAPI.Models.DTOs;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;

//namespace StevenAccelokaAPI.Services
//{
//    public class BookedTicketService : IBookedTicketService
//    {
//        private readonly ApplicationDbContext _dbContext;

//        public BookedTicketService(ApplicationDbContext dbContext)
//        {
//            _dbContext = dbContext;
//        }

//        public async Task<int> BookTicketAsync(List<BookTicketRequestDto> request)
//        {
//            if (request == null || !request.Any())
//            {
//                throw new ArgumentException("Booking request cannot be empty.");
//            }
//            //. 1 Ambil daftar TicketCode dari request
//            var ticketCodes = request.Select(t => t.TicketCode).ToList();

//            // 🔹 2. Cari tiket yang cocok berdasarkan TicketCode
//            var ticketsInDb = await _dbContext.Tickets
//                .Where(t => ticketCodes.Contains(t.TicketCode))
//                .ToListAsync();

//            if (ticketsInDb.Count != ticketCodes.Count)
//            {
//                throw new ArgumentException("Some ticket codes do not exist.");
//            }
//            // 🔹 3. Buat BookingId Baru
//            var newBooking = new Booking
//            {
//                BookingDate = DateTime.UtcNow
//            };

//            _dbContext.Bookings.Add(newBooking);
//            await _dbContext.SaveChangesAsync(); // 🔹 Pastikan BookingId Terbuat di Database


//            // 🔹 4. Masukkan tiket yang dibooking dengan BookingId yang baru dibuat
//            var bookedTickets = request.Select(ticketRequest =>
//            {
//                var ticket = ticketsInDb.FirstOrDefault(t => t.TicketCode == ticketRequest.TicketCode);
//                if (ticket == null)
//                {
//                    throw new ArgumentException($"Ticket with code {ticketRequest.TicketCode} not found.");
//                }

//                return new BookedTicket
//                {
//                    BookingId = newBooking.Id, // 🔹 Gunakan BookingId yang baru saja dibuat
//                    TicketId = ticket.Id,      // 🔹 Cari TicketId dari database
//                    Quantity = ticketRequest.Quantity
//                };
//            }).ToList();

//            _dbContext.BookedTickets.AddRange(bookedTickets);
//            await _dbContext.SaveChangesAsync();

//            return newBooking.Id; // 🔹 Return BookingId agar bisa digunakan untuk pengecekan
//        }


//        public async Task<List<BookedTicketCategoryDto>> GetBookedTicketDetailsAsync(int bookingId)
//        {
//            var bookedTickets = await _dbContext.BookedTickets
//                .Where(bt => bt.BookingId == bookingId)
//                .Include(bt => bt.Ticket) // ✅ Include Ticket first
//                    .ThenInclude(t => t.TicketCategory) // ✅ Ensure Category is included
//                .ToListAsync();

//            if (!bookedTickets.Any())
//            {
//                return null; //tangani di controller
//            }

//            var groupedTickets = bookedTickets
//        .GroupBy(bt => bt.Ticket.TicketCategory?.Name ?? "Uncategorized") // 🛠 Handle jika kategori null
//        .Select(group => new BookedTicketCategoryDto
//        {
//            CategoryName = group.Key,
//            QtyPerCategory = group.Sum(bt => bt.Quantity),
//            Tickets = group.Select(bt => new BookedTicketDto
//            {
//                TicketCode = bt.Ticket.TicketCode,
//                TicketName = bt.Ticket.TicketName,
//                CategoryName = bt.Ticket.TicketCategory.Name,
//                Quantity = bt.Quantity,
//                EventDate = bt.Ticket.EventDate // 🛠 Format datetime
//            }).ToList()
//        }).ToList();

//            return groupedTickets;
//        }

//        // 🔹 API DELETE: Revoke Ticket
//        public async Task<ServiceResponse<List<BookedTicketDetailDto>>> RevokeTicketAsync(int bookedTicketId, string ticketCode, int quantity)
//        {
//            var response = new ServiceResponse<List<BookedTicketDetailDto>>();

//            // Step 1: Find the booked ticket
//            var bookedTicket = await _dbContext.BookedTickets
//                .Include(bt => bt.Ticket)
//                .ThenInclude(t => t.TicketCategory)
//                .FirstOrDefaultAsync(bt => bt.BookingId == bookedTicketId && bt.Ticket.TicketCode == ticketCode);

//            if (bookedTicket == null)
//            {
//                response.Success = false;
//                response.StatusCode = 404;
//                response.Message = "Booked ticket ID or ticket code not found.";
//                return response;
//            }

//            // Step 2: Validate the quantity
//            if (quantity > bookedTicket.Quantity)
//            {
//                response.Success = false;
//                response.StatusCode = 400;
//                response.Message = "Revoked quantity cannot exceed booked quantity.";
//                return response;
//            }

//            // Step 3: Update or remove the ticket
//            bookedTicket.Quantity -= quantity;

//            if (bookedTicket.Quantity == 0)
//            {
//                _dbContext.BookedTickets.Remove(bookedTicket);
//            }

//            // Step 4: Check if the entire booking is empty
//            await _dbContext.SaveChangesAsync(); // Save first to update DB

//            bool hasRemainingTickets = await _dbContext.BookedTickets.AnyAsync(bt => bt.BookingId == bookedTicketId);
//            if (!hasRemainingTickets)
//            {
//                var booking = await _dbContext.Bookings.FindAsync(bookedTicketId);
//                if (booking != null)
//                {
//                    _dbContext.Bookings.Remove(booking);
//                    await _dbContext.SaveChangesAsync(); // Save after removing booking
//                }

//                response.Success = true;
//                response.StatusCode = 200;
//                response.Message = "All tickets under this Booking ID have been revoked.";
//                response.Data = new List<BookedTicketDetailDto>(); // Empty list since all tickets are removed
//                return response;
//            }

//            // Step 5: Fetch remaining tickets under the same BookedTicketId
//            var remainingTickets = await _dbContext.BookedTickets
//                .Where(bt => bt.BookingId == bookedTicketId)
//                .Include(bt => bt.Ticket)
//                .ThenInclude(t => t.TicketCategory)
//                .Select(bt => new BookedTicketDetailDto
//                {
//                    TicketCode = bt.Ticket.TicketCode,
//                    TicketName = bt.Ticket.TicketName,
//                    CategoryName = bt.Ticket.TicketCategory != null ? bt.Ticket.TicketCategory.Name : "Uncategorized",
//                    Quantity = bt.Quantity,
//                     EventDate = bt.Ticket.EventDate
//                })
//                .ToListAsync();

//            // Step 6: Return success with remaining tickets
//            response.Success = true;
//            response.StatusCode = 200;
//            response.Data = remainingTickets;
//            return response;
//        }

//        public async Task<ServiceResponse<List<BookedTicketDetailDto>>> EditBookedTicketAsync(int bookedTicketId, EditBookedTicketDto updatedTickets)
//        {
//            var response = new ServiceResponse<List<BookedTicketDetailDto>>();

//            // 1. Validasi apakah BookedTicketId ada di database
//            var existingBookings = await _dbContext.BookedTickets
//                .Where(bt => bt.BookingId == bookedTicketId)
//                .Include(bt => bt.Ticket)
//                .ThenInclude(t => t.TicketCategory)
//                .ToListAsync();

//            if (!existingBookings.Any())
//            {
//                response.Success = false;
//                response.StatusCode = 400;
//                response.Message = $"No booking found for ID {bookedTicketId}";
//                return response;
//            }

//            // 2. Looping setiap tiket yang ingin diupdate
//            foreach (var ticketUpdate in updatedTickets.Tickets)
//            {
//                var bookedTicket = existingBookings.FirstOrDefault(bt => bt.Ticket.TicketCode == ticketUpdate.TicketCode);

//                // 3. Validasi apakah tiket ada dalam booking ini
//                if (bookedTicket == null)
//                {
//                    response.Success = false;
//                    response.StatusCode = 404;
//                    response.Message = $"Ticket code {ticketUpdate.TicketCode} is not booked under this booking ID.";
//                    return response;
//                }

//                // 4. Validasi quantity minimal 1
//                if (ticketUpdate.Quantity < 1)
//                {
//                    response.Success = false;
//                    response.StatusCode = 400;
//                    response.Message = "Quantity must be at least 1.";
//                    return response;
//                }

//                // 5. Validasi apakah sisa quota mencukupi
//                var ticketInDb = await _dbContext.Tickets.FirstOrDefaultAsync(t => t.Id == bookedTicket.TicketId);
//                if (ticketInDb == null || ticketUpdate.Quantity > ticketInDb.Quota)
//                {
//                    response.Success = false;
//                    response.StatusCode = 400;
//                    response.Message = $"Requested quantity {ticketUpdate.Quantity} exceeds available quota for ticket {ticketUpdate.TicketCode}.";
//                    return response;
//                }

//                // 6. Update quantity tiket yang sudah dibooking
//                bookedTicket.Quantity = ticketUpdate.Quantity;
//            }

//            // 7. Simpan perubahan ke database
//            await _dbContext.SaveChangesAsync();

//            // 8. Ambil semua tiket dalam booking ID ini
//            response.Data = existingBookings.Select(bt => new BookedTicketDetailDto
//            {
//                TicketCode = bt.Ticket.TicketCode,
//                TicketName = bt.Ticket.TicketName,
//                CategoryName = bt.Ticket.TicketCategory?.Name ?? "Uncategorized",
//                Quantity = bt.Quantity,
//                EventDate = bt.Ticket.EventDate
//            }).ToList();

//            return response;
//        }
//    }
//}
