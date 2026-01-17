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
        private const string AIRLINE_NAME = "PAN America";
        private readonly AirlineDbContext _db;
        private readonly AutoMapper.IMapper _mapper;

        public WebhookSubscriptionsController(AirlineDbContext db, AutoMapper.IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateWebhookSubscriptionDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Check if record already exists with the same URL and type
            var existingRecord = await _db.WebhookSubscriptions
                .FirstOrDefaultAsync(x => x.WebhookUrl == dto.WebhookUrl && x.WebhookType == dto.WebhookType);

            if (existingRecord != null)
            {
                return NoContent();
            }

            var entity = _mapper.Map<WebhookSubscription>(dto);
            entity.Secret = Guid.NewGuid().ToString();
            entity.WebhookPublisher = AIRLINE_NAME;

            _db.WebhookSubscriptions.Add(entity);
            await _db.SaveChangesAsync();

            var responseDto = _mapper.Map<WebhookSubscriptionDto>(entity);

            return CreatedAtAction(nameof(GetBySecret), new { secret = entity.Secret }, responseDto);
        }

        [HttpGet("{secret}")]
        public async Task<IActionResult> GetBySecret(string secret)
        {
            var entity = await _db.WebhookSubscriptions.FirstOrDefaultAsync(x => x.Secret == secret);
            if (entity == null) return NotFound();

            var dto = _mapper.Map<WebhookSubscriptionDto>(entity);
            return Ok(dto);
        }
    }
}