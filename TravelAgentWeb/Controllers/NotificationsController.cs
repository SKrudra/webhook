using Microsoft.AspNetCore.Mvc;
using TravelAgentWeb.Data;
using TravelAgentWeb.Models;
using Microsoft.EntityFrameworkCore;

namespace TravelAgentWeb.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class NotificationsController : ControllerBase
    {
        private readonly TravelAgentDbContext _dbContext;

        public NotificationsController(TravelAgentDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpPost]
        public async Task<IActionResult> FlightChanged([FromBody] UpdateFlightDetailsDto payload)
        {
            Console.WriteLine($"Received webhook from publisher: {payload.Publisher}");
            // Validate the webhook secret
            var webhookSecret = await _dbContext.WebhookSecrets.FirstOrDefaultAsync(ws => ws.Publisher == payload.Publisher && ws.Secret == payload.Secret);
            if (webhookSecret == null)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Invalid webhook secret.");
                Console.ResetColor();
                return Unauthorized("Invalid webhook secret.");
            }
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Webhook secret validated successfully.");
            Console.ResetColor();

            // Process the webhook payload (this is just a placeholder for actual processing logic)
            // ...

            return Ok("Webhook received and processed.");
        }

        [HttpGet("test")]
        public IActionResult Test()
        {
            return Ok("Test endpoint is working.");
        }
    }

}