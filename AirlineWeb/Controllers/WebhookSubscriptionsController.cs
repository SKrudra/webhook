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

            var entity = _mapper.Map<WebhookSubscription>(dto);

            _db.WebhookSubscriptions.Add(entity);
            await _db.SaveChangesAsync();

            var responseDto = _mapper.Map<WebhookSubscriptionDto>(entity);

            return CreatedAtAction(nameof(GetById), new { id = entity.Id }, responseDto);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var entity = await _db.WebhookSubscriptions.FirstOrDefaultAsync(x => x.Id == id);
            if (entity == null) return NotFound();

            var dto = _mapper.Map<WebhookSubscriptionDto>(entity);
            return Ok(dto);
        }
    }
}