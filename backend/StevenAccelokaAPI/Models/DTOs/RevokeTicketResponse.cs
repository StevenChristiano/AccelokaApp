namespace StevenAccelokaAPI.Models.DTOs
{
    public class RevokeTicketResponse
    {
        public int BookedTicketId { get; set; }
        public List<RevokeTicketDto> Tickets { get; set; } = new();
    }
}
