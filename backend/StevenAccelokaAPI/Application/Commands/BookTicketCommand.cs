using MediatR;
using Microsoft.EntityFrameworkCore;
using Hellang.Middleware.ProblemDetails;
using StevenAccelokaAPI.Models;
using StevenAccelokaAPI.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace StevenAccelokaAPI.Application.Commands
{
    public class BookTicketCommand : IRequest<object>
    {
        public List<BookTicketRequestDto> Tickets { get; set; }
    }

    public class BookTicketCommandHandler : IRequestHandler<BookTicketCommand, object>
    {
        private readonly ApplicationDbContext _context;

        public BookTicketCommandHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<object> Handle(BookTicketCommand request, CancellationToken cancellationToken)
        {
            if (request.Tickets == null || !request.Tickets.Any())
            {
                throw new ProblemDetailsException(new ProblemDetails
                {
                    Title = "Invalid Request",
                    Detail = "Request must contain at least one ticket booking.",
                    Status = (int)HttpStatusCode.BadRequest,
                    Type = "https://httpstatuses.com/400"
                });
            }

            var ticketCodes = request.Tickets.Select(t => t.TicketCode).ToList();
            var ticketsInDb = await _context.Tickets.Include(t => t.TicketCategory)
                .Where(t => ticketCodes.Contains(t.TicketCode))
                .ToListAsync(cancellationToken);

            if (!ticketsInDb.Any())
            {
                throw new ProblemDetailsException(new ProblemDetails
                {
                    Title = "Tickets Not Found",
                    Detail = "None of the provided ticket codes exist in the database.",
                    Status = (int)HttpStatusCode.NotFound,
                    Type = "https://httpstatuses.com/404"
                });
            }

            decimal totalPrice = 0;
            var groupedByCategory = new Dictionary<string, decimal>();
            var bookings = new List<BookedTicket>();

            var booking = new Booking { BookingDate = DateTime.UtcNow };
            _context.Bookings.Add(booking);
            await _context.SaveChangesAsync(cancellationToken);

            foreach (var bookingRequest in request.Tickets)
            {
                var ticket = ticketsInDb.FirstOrDefault(t => t.TicketCode == bookingRequest.TicketCode);
                if (ticket == null)
                {
                    throw new ProblemDetailsException(new ProblemDetails
                    {
                        Title = "Ticket Not Found",
                        Detail = $"Ticket with code '{bookingRequest.TicketCode}' does not exist.",
                        Status = (int)HttpStatusCode.NotFound,
                        Type = "https://httpstatuses.com/404"
                    });
                }

                if (ticket.Quota < bookingRequest.Quantity)
                {
                    throw new ProblemDetailsException(new ProblemDetails
                    {
                        Title = "Insufficient Ticket Quota",
                        Detail = $"Requested {bookingRequest.Quantity} tickets, but only {ticket.Quota} are available.",
                        Status = (int)HttpStatusCode.BadRequest,
                        Type = "https://httpstatuses.com/400"
                    });
                }

                if (ticket.EventDate <= DateTime.UtcNow)
                {
                    throw new ProblemDetailsException(new ProblemDetails
                    {
                        Title = "Invalid Booking Date",
                        Detail = "The event date must be in the future.",
                        Status = (int)HttpStatusCode.BadRequest,
                        Type = "https://httpstatuses.com/400"
                    });
                }

                decimal ticketTotalPrice = ticket.Price * bookingRequest.Quantity;
                totalPrice += ticketTotalPrice;
                groupedByCategory[ticket.TicketCategory.Name] = groupedByCategory.GetValueOrDefault(ticket.TicketCategory.Name, 0) + ticketTotalPrice;

                ticket.Quota -= bookingRequest.Quantity;

                bookings.Add(new BookedTicket
                {
                    BookingId = booking.Id,
                    TicketId = ticket.Id,
                    Quantity = bookingRequest.Quantity,
                    Price = ticket.Price,
                    TotalPrice = ticketTotalPrice
                });
            }

            await _context.BookedTickets.AddRangeAsync(bookings);
            await _context.SaveChangesAsync(cancellationToken);

            var groupedTickets = bookings
                .Join(ticketsInDb, b => b.TicketId, t => t.Id, (b, t) => new
                {
                    CategoryName = t.TicketCategory.Name,
                    TicketCode = t.TicketCode,
                    TicketName = t.TicketName,
                    Price = b.Price,
                    TotalPrice = b.TotalPrice
                })
                .GroupBy(t => t.CategoryName)
                .Select(g => new
                {
                    categoryName = g.Key,
                    summaryPrice = g.Sum(t => t.TotalPrice),
                    tickets = g.Select(t => new
                    {
                        ticketCode = t.TicketCode,
                        ticketName = t.TicketName,
                        price = t.Price
                    }).ToList()
                }).ToList();

            var response = new
            {
                priceSummary = groupedTickets.Sum(g => g.summaryPrice),
                ticketsPerCategories = groupedTickets
            };

            return response;
        }
    }
}