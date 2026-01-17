namespace AirlineWeb.Dtos
{
    public class FlightDetailsDto
    {
        public int Id { get; set; }
        public string FlightNumber { get; set; } = string.Empty;
        public decimal Price { get; set; }
    }
}