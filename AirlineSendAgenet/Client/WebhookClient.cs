using AirlineSendAgenet.Dtos;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace AirlineSendAgenet.Client
{

    public class WebhookClient : IWebhookClient
    {
        private readonly HttpClient _httpClient;

        public WebhookClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task SendNotificationAsync(FlightDetailsChangePayloadDto payload)
        {
            Console.WriteLine($"Sending webhook to {payload.WebhookUrl} for flight {payload.FlightNumber}");
            var jsonPayload = JsonSerializer.Serialize(payload);
            var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

            try
            {
                using var response = await _httpClient.PostAsync(payload.WebhookUrl, content);
                response.EnsureSuccessStatusCode();
                Console.WriteLine($"Webhook sent successfully to {payload.WebhookUrl}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error preparing webhook request: {ex.Message}");
                throw;
            }

        }
    }
}