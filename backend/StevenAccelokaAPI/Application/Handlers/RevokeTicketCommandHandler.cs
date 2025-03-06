using Hellang.Middleware.ProblemDetails;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StevenAccelokaAPI.Application.Commands;
using StevenAccelokaAPI.Models.DTOs;

namespace StevenAccelokaAPI.Application.Handlers
{
    public class RevokeTicketCommandHandler : IRequestHandler<RevokeTicketCommand, RevokeTicketResponse>
    {
        private readonly ApplicationDbContext _context;

        public RevokeTicketCommandHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<RevokeTicketResponse> Handle(RevokeTicketCommand request, CancellationToken cancellationToken)
        {
            var bookedTicket = await _context.BookedTickets
                .Include(bt => bt.Ticket)
                .Where(bt => bt.BookingId == request.BookedTicketId && bt.Ticket.TicketCode == request.TicketCode)
                .FirstOrDefaultAsync(cancellationToken);

            if (bookedTicket == null)
            {
                throw new ProblemDetailsException(new ProblemDetails
                {
                    Type = "https://example.com/probs/booked-ticket-not-found",
                    Title = "Booked Ticket Not Found",
                    Status = StatusCodes.Status404NotFound,
                    Detail = $"No booked ticket found with ID {request.BookedTicketId}",
                    Instance = $"/api/v1/revoke-ticket/{request.BookedTicketId}/{request.TicketCode}/{request.Quantity}"
                });
            }

            if (request.Quantity > bookedTicket.Quantity)
            {
                throw new ProblemDetailsException(new ProblemDetails
                {
                    Type = "https://example.com/probs/invalid-quantity",
                    Title = "Invalid Quantity",
                    Status = StatusCodes.Status400BadRequest,
                    Detail = "Quantity cannot be greater than booked quantity.",
                    Instance = $"/api/v1/revoke-ticket/{request.BookedTicketId}/{request.TicketCode}/{request.Quantity}"
                });
            }
            if (request.Quantity <= 0)
            {
                throw new ProblemDetailsException(new ProblemDetails
                {
                    Type = "https://example.com/probs/invalid-quantity",
                    Title = "Invalid Quantity",
                    Status = StatusCodes.Status400BadRequest,
                    Detail = "Quantity cannot be lesser than 1.",
                    Instance = $"/api/v1/revoke-ticket/{request.BookedTicketId}/{request.TicketCode}/{request.Quantity}"
                });
            }

            bookedTicket.Quantity -= request.Quantity;

            if (bookedTicket.Quantity == 0)
            {
                _context.BookedTickets.Remove(bookedTicket);
            }

            await _context.SaveChangesAsync(cancellationToken);

            var remainingTickets = await _context.BookedTickets
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
                Tickets = remainingTickets
            };
        }
    }

}
