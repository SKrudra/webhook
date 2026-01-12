namespace AirlineWeb.Models
{
    public class WebhookSubscriptionDto
    {
        public int Id { get; set; }
        public string WebhookUrl { get; set; } = string.Empty;
        public string Secret { get; set; } = string.Empty;
        public string WebhookType { get; set; } = string.Empty;
        public string WebhookPublisher { get; set; } = string.Empty;
    }
}