using MediatR;
using System.Collections.Generic;

namespace StevenAccelokaAPI.Application.Commands
{
    public class EditBookedTicketCommand : IRequest<object>
    {
        public int BookedTicketId { get; set; }
        public List<EditBookedTicketItemDto> Tickets { get; set; }
    }

    public class EditBookedTicketItemDto
    {
        public string TicketCode { get; set; }
        public int Quantity { get; set; }
    }
}
