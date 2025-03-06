using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StevenAccelokaAPI.Models
{
    public class Booking
    {
        [Key] // ✅ Menandai sebagai Primary Key
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // ✅ Auto-increment
        public int Id { get; set; } // BookingId otomatis

        public DateTime BookingDate { get; set; }

        // Relasi ke tabel BookedTickets
        public ICollection<BookedTicket> BookedTickets { get; set; }
    }
}

