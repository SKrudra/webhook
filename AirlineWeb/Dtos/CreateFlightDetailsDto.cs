namespace AirlineWeb.Dtos
{
    public class CreateFlightDetailsDto
    {
        public string FlightNumber { get; set; } = string.Empty;
        public decimal Price { get; set; }
    }
}