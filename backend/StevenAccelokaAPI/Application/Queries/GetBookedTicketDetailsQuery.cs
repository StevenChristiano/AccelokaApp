using MediatR;
using Microsoft.EntityFrameworkCore;
using StevenAccelokaAPI.Models;
using StevenAccelokaAPI.Models.DTOs;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Hellang.Middleware.ProblemDetails;

namespace StevenAccelokaAPI.Application.Queries
{
    public class GetBookedTicketDetailsQuery : IRequest<List<BookedTicketCategoryDto>>
    {
        public int BookedTicketId { get; set; }
    }

    public class GetBookedTicketQueryHandler : IRequestHandler<GetBookedTicketDetailsQuery, List<BookedTicketCategoryDto>>
    {
        private readonly ApplicationDbContext _context;

        public GetBookedTicketQueryHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<BookedTicketCategoryDto>> Handle(GetBookedTicketDetailsQuery request, CancellationToken cancellationToken)
        {
            var bookedTickets = await _context.BookedTickets
                .Where(bt => bt.BookingId == request.BookedTicketId)
                .Include(bt => bt.Ticket)
                .ThenInclude(t => t.TicketCategory)
                .ToListAsync(cancellationToken);

            if (!bookedTickets.Any())
            {
                throw new ProblemDetailsException(404, $"Booked Ticket ID {request.BookedTicketId} not found.");
            }

            var groupedTickets = bookedTickets
                .GroupBy(bt => bt.Ticket.TicketCategory.Name)
                .AsEnumerable()
                .Select(g => new BookedTicketCategoryDto
                {
                    CategoryName = g.Key,
                    QtyPerCategory = g.Sum(bt => bt.Quantity),
                    Tickets = g.Select(bt => new BookedTicketDetailDto
                    {
                        TicketCode = bt.Ticket.TicketCode,
                        TicketName = bt.Ticket.TicketName,
                        EventDate = bt.Ticket.EventDate
                    }).ToList()
                }).ToList();

            return groupedTickets;
        }
    }
}
