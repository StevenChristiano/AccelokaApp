using System.ComponentModel.DataAnnotations.Schema;

namespace StevenAccelokaAPI.Models
{
    public class Ticket
    {
        public int Id { get; set; }
        public string TicketCode { get; set; } = string.Empty;
        public string TicketName { get; set; } = string.Empty;
        public DateTime EventDate { get; set; }
        public int Quota { get; set; }
        public decimal Price { get; set; }


        public int TicketCategoryId { get; set; }

        // ✅ Navigation Property to TicketCategory
        [ForeignKey("TicketCategoryId")]
        public TicketCategory TicketCategory { get; set; }
    }
    

}
