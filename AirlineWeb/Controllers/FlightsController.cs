namespace AirlineWeb.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using AirlineWeb.Data;
    using AirlineWeb.Models;
    using AirlineWeb.Dtos;

    [ApiController]
    [Route("api/[controller]")]
    public class FlightsController : ControllerBase
    {
        private readonly AirlineDbContext _db;
        private readonly AutoMapper.IMapper _mapper;

        public FlightsController(AirlineDbContext db, AutoMapper.IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateFlightDetailsDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var entity = _mapper.Map<FlightDetails>(dto);

            _db.FlightDetails.Add(entity);
            await _db.SaveChangesAsync();

            var responseDto = _mapper.Map<FlightDetailsDto>(entity);

            return CreatedAtAction(nameof(GetByFlightNumber), new { flightNumber = entity.FlightNumber }, responseDto);
        }

        [HttpGet("{flightNumber}")]
        public async Task<IActionResult> GetByFlightNumber(string flightNumber)
        {
            var entity = await _db.FlightDetails.FirstOrDefaultAsync(x => x.FlightNumber == flightNumber);
            if (entity == null) return NotFound();

            var dto = _mapper.Map<FlightDetailsDto>(entity);
            return Ok(dto);
        }
        // update flight details
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateFlightDetailsDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var entity = await _db.FlightDetails.FirstOrDefaultAsync(x => x.Id == id);
            if (entity == null) return NotFound();

            _mapper.Map(dto, entity);
            await _db.SaveChangesAsync();

            return NoContent();
        }
    }
}