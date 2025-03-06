using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StevenAccelokaAPI.Models
{
    public class BookedTicket
    {
        [Key]
        public int Id { get; set; } // Auto-incremented primary key

        [ForeignKey("Booking")]
        public int BookingId {  get; set; }
        public Booking Booking { get; set; } // Navigation Property

        [Required]
        public int TicketId { get; set; }

        // ✅ Navigation Property to Ticket
        [ForeignKey("TicketId")]
        public virtual Ticket Ticket { get; set; } = null!;


        //public int TicketCategoryId { get; set; }
        //public TicketCategory TicketCategory { get; set; } // ✅ Navigation Property


        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1")]
        public int Quantity { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        // ✅ Computed TotalPrice (Quantity * Price)
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalPrice { get; set; }

        // ✅ Event date (linked to the ticket)
      

        // ✅ Booking date (set automatically)
        [Required]
        public DateTime BookingDate { get; set; } = DateTime.UtcNow;
    }
}
