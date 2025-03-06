namespace StevenAccelokaAPI.Models.DTOs
{
    public class RevokeTicketDto
    {
        public string TicketCode { get; set; } = string.Empty;
        public string TicketName { get; set; } = string.Empty;
        public string CategoryName { get; set; } = string.Empty;
        public int RemainingQuantity { get; set; }
    }
}