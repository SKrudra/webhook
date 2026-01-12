using System.ComponentModel.DataAnnotations;

namespace AirlineWeb.Models
{
    public class CreateWebhookSubscriptionDto
    {
        [Required]
        [Url]
        public string WebhookUrl { get; set; } = string.Empty;

        [Required]
        public string WebhookType { get; set; } = string.Empty;
    }
}