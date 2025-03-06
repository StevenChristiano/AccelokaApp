using MediatR;
using Microsoft.EntityFrameworkCore;

namespace StevenAccelokaAPI.Application.Queries
{ // buat halaman get-booked-ticket
    public class GetBookedTicketIdsQuery: IRequest<List<int>> { }

    public class GetBookedTicketIdsQueryHandler : IRequestHandler<GetBookedTicketIdsQuery, List <int>>
    {
        private readonly ApplicationDbContext _context;
        public GetBookedTicketIdsQueryHandler (ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<List<int>> Handle(GetBookedTicketIdsQuery request, CancellationToken cancellationToken)
        {
            return await _context.BookedTickets
                .Select(bt => bt.BookingId)
                .Distinct()
                .ToListAsync(cancellationToken);
        }
    }
}
