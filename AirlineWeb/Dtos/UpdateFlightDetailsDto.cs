namespace AirlineWeb.Dtos
{
    public class UpdateFlightDetailsDto
    {
        public string FlightNumber { get; set; } = string.Empty;
        public decimal Price { get; set; }
    }
}