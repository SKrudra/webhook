namespace AirlineWeb.Dtos
{
    public class NotificationMessageDto
    {
        public string Publisher { get; set; } = null!;
        public string Secret { get; set; } = null!;
        public string FlightNumber { get; set; } = string.Empty;
        public decimal OldPrice { get; set; }
        public decimal NewPrice { get; set; }
        public string WebhookType { get; set; } = string.Empty;
    }
}