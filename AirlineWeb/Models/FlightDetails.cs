using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AirlineWeb.Models
{
    public class FlightDetails
    {
        [Key]
        [Required]
        public int Id { get; set; }
        [Required]
        public string FlightNumber { get; set; } = string.Empty;
        [Column(TypeName = "decimal(6,2)")]
        [Required]
        public decimal Price { get; set; }
       
    }
}