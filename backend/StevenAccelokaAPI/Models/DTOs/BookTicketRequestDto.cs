using System.ComponentModel.DataAnnotations;

namespace StevenAccelokaAPI.Models.DTOs
{
    public class BookTicketRequestDto
    {
        public string TicketCode { get; set; } = string.Empty;
        //[Required(ErrorMessage = "TicketId is required.")]
        //public int TicketId { get; set; } // ID Tiket yang ingin dipesan

        [Required(ErrorMessage = "Quantity is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1.")]
        public int Quantity { get; set; } // Jumlah tiket yang ingin dipesan
    }
}

