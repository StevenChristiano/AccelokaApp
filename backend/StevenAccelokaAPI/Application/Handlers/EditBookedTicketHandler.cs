using Hellang.Middleware.ProblemDetails;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StevenAccelokaAPI.Application.Commands;
using StevenAccelokaAPI.Models;
using StevenAccelokaAPI.Models.DTOs;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace StevenAccelokaAPI.Application.Handlers
{
    public class EditBookedTicketHandler : IRequestHandler<EditBookedTicketCommand, object>
    {
        private readonly ApplicationDbContext _context;

        public EditBookedTicketHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<object> Handle(EditBookedTicketCommand request, CancellationToken cancellationToken)
        {
            var bookedTickets = await _context.BookedTickets
                .Include(bt => bt.Ticket)
                .ThenInclude(t => t.TicketCategory)
                .Where(bt => bt.BookingId == request.BookedTicketId)
                .ToListAsync(cancellationToken);

            if (!bookedTickets.Any())
            {
                throw new ProblemDetailsException(new ProblemDetails
                {
                    Title = "Not Found",
                    Detail = $"Booked Ticket ID {request.BookedTicketId} not found.",
                    Status = 404
                });
            }

            var validationErrors = new Dictionary<string, List<string>>();

            foreach (var item in request.Tickets)
            {
                var bookedTicket = bookedTickets.FirstOrDefault(bt => bt.Ticket.TicketCode == item.TicketCode);
                if (bookedTicket == null)
                {
                    throw new ProblemDetailsException(new ProblemDetails
                    {
                        Title = "Not Found",
                        Detail = $"Ticket Code {item.TicketCode} not found in booked tickets.",
                        Status = 404
                    });
                }

                var ticket = await _context.Tickets.FirstOrDefaultAsync(t => t.Id == bookedTicket.TicketId, cancellationToken);
                if (ticket == null)
                {
                    throw new ProblemDetailsException(new ProblemDetails
                    {
                        Title = "Not Found",
                        Detail = $"Ticket Code {item.TicketCode} does not exist.",
                        Status = 404
                    });
                }

                if (item.Quantity < 1)
                {
                    if (!validationErrors.ContainsKey("Quantity"))
                        validationErrors["Quantity"] = new List<string>();

                    validationErrors["Quantity"].Add("Quantity must be at least 1.");
                }

                if (item.Quantity > ticket.Quota + bookedTicket.Quantity)
                {
                    if (!validationErrors.ContainsKey("Quantity"))
                        validationErrors["Quantity"] = new List<string>();

                    validationErrors["Quantity"].Add($"Quantity exceeds available quota for {item.TicketCode}.");
                }
            }

            // Jika ada validation errors, hentikan proses dan return error
            if (validationErrors.Any())
            {
                var failures = validationErrors
                    .SelectMany(kv => kv.Value.Select(msg => new FluentValidation.Results.ValidationFailure(kv.Key, msg)))
                    .ToList();

                throw new FluentValidation.ValidationException(failures);
            }

            // Update jumlah tiket yang dibooking jika semua validasi lolos
            foreach (var item in request.Tickets)
            {
                var bookedTicket = bookedTickets.First(bt => bt.Ticket.TicketCode == item.TicketCode);
                var ticket = await _context.Tickets.FirstAsync(t => t.Id == bookedTicket.TicketId, cancellationToken);

                ticket.Quota += bookedTicket.Quantity - item.Quantity;
                bookedTicket.Quantity = item.Quantity;
            }

            await _context.SaveChangesAsync(cancellationToken);

            var updatedgroupedTickets = await _context.BookedTickets
                .Where(bt => bt.BookingId == request.BookedTicketId)
                .Select(bt => new RevokeTicketDto
                {
                    TicketCode = bt.Ticket.TicketCode,
                    TicketName = bt.Ticket.TicketName,
                    CategoryName = bt.Ticket.TicketCategory.Name,
                    RemainingQuantity = bt.Quantity
                }).ToListAsync(cancellationToken);

            return new RevokeTicketResponse
            {
                BookedTicketId = request.BookedTicketId,
                Tickets = updatedgroupedTickets
            };
        }
    }
}
