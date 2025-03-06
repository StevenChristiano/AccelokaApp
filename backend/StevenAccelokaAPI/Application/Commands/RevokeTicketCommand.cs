using MediatR;
using StevenAccelokaAPI.Models.DTOs;

namespace StevenAccelokaAPI.Application.Commands
{
    public class RevokeTicketCommand : IRequest<RevokeTicketResponse>
    {
        public int BookedTicketId { get; set; }
        public string TicketCode { get; set; } = string.Empty;
        public int Quantity { get; set; }
    }

}
