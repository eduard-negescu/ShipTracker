using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShipTracker.Server.Database;
using ShipTracker.Server.Models;
using ShipTracker.Server.Models.Entities;

namespace ShipTracker.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PortsController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;

        public PortsController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public async Task<ActionResult<List<Port>>> GetAllPorts()
        {
            var ports = await _dbContext.Ports
                .Include(p => p.Country)
                .OrderBy(p => p.Name)
                .ToListAsync();

            return Ok(ports);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Port>> GetPort(int id)
        {
            var port = await _dbContext.Ports
                .Include(p => p.Country)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (port == null)
            {
                return NotFound();
            }

            return Ok(port);
        }

        [HttpPost]
        public async Task<IActionResult> AddPort(AddPortDto addPortDto)
        {
            // Verify country exists
            var countryExists = await _dbContext.Countries
                .AnyAsync(c => c.Id == addPortDto.CountryId);

            if (!countryExists)
            {
                return BadRequest("Country does not exist");
            }

            var port = new Port
            {
                Name = addPortDto.Name,
                CountryId = addPortDto.CountryId
            };

            _dbContext.Ports.Add(port);
            await _dbContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetPort), new { id = port.Id }, port);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdatePort(int id, UpdatePortDto updatePortDto)
        {
            var port = await _dbContext.Ports.FindAsync(id);

            if (port == null)
            {
                return NotFound();
            }

            // Update name if provided
            if (updatePortDto.Name != null)
            {
                port.Name = updatePortDto.Name;
            }

            // Update country if provided and exists
            if (updatePortDto.CountryId.HasValue)
            {
                var countryExists = await _dbContext.Countries
                    .AnyAsync(c => c.Id == updatePortDto.CountryId.Value);

                if (!countryExists)
                {
                    return BadRequest("Country does not exist");
                }

                port.CountryId = updatePortDto.CountryId.Value;
            }

            await _dbContext.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeletePort(int id)
        {
            var port = await _dbContext.Ports.FindAsync(id);

            if (port == null)
            {
                return NotFound();
            }

            _dbContext.Ports.Remove(port);
            await _dbContext.SaveChangesAsync();

            return NoContent();
        }
    }
}