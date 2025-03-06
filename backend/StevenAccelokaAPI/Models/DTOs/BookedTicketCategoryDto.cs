namespace StevenAccelokaAPI.Models.DTOs
{ // ini buat
    public class BookedTicketCategoryDto
    {
        public int QtyPerCategory { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public List<BookedTicketDetailDto> Tickets { get; set; }
    }
}
