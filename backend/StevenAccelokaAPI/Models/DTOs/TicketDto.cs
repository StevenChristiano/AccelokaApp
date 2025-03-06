
using StevenAccelokaAPI.Helpers;
using System.Text.Json.Serialization;

namespace StevenAccelokaAPI.Models.DTOs
{
    public class TicketDto
    {
        [JsonConverter(typeof(CustomDateTimeConverter))]
        public DateTime EventDate { get; set; }
        public int Quota { get; set; }
        public string TicketCode { get; set; } = string.Empty;
        public string TicketName { get; set; } = string.Empty;
        public string CategoryName { get; set; } = string.Empty;
        public decimal Price { get; set; }

    }

}

