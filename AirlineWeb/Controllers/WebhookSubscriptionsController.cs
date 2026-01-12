using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AirlineWeb.Data;
using AirlineWeb.Models;

namespace AirlineWeb.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WebhookSubscriptionsController : ControllerBase
    {
        private readonly AirlineDbContext _db;

        public WebhookSubscriptionsController(AirlineDbContext db)
        {
            _db = db;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateWebhookSubscriptionDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var entity = new WebhookSubscription
            {
                WebhookUrl = dto.WebhookUrl,
                WebhookType = dto.WebhookType,
            };

            _db.WebhookSubscriptions.Add(entity);
            await _db.SaveChangesAsync();

            var responseDto = new WebhookSubscriptionDto
            {
                Id = entity.Id,
                WebhookUrl = entity.WebhookUrl,
                Secret = entity.Secret,
                WebhookType = entity.WebhookType,
                WebhookPublisher = entity.WebhookPublisher
            };

            return CreatedAtAction(nameof(GetById), new { id = entity.Id }, responseDto);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var entity = await _db.WebhookSubscriptions.FirstOrDefaultAsync(x => x.Id == id);
            if (entity == null) return NotFound();

            var dto = new WebhookSubscriptionDto
            {
                Id = entity.Id,
                WebhookUrl = entity.WebhookUrl,
                Secret = entity.Secret,
                WebhookType = entity.WebhookType,
                WebhookPublisher = entity.WebhookPublisher
            };

            return Ok(dto);
        }
    }
}