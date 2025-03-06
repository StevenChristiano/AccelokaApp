using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StevenAccelokaAPI.Models
{
    public class TicketCategory
    {
        [Key]
        public int Id { get; set; } // Primary Key

        [Required]
        [StringLength(50)]
        public string Name { get; set; } = string.Empty; // Category Name

        // Navigation Property to Ticket
        public virtual ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
    }
}
