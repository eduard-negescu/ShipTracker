using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShipTracker.Server.Database;
using ShipTracker.Server.Models;
using ShipTracker.Server.Models.Entities;

namespace ShipTracker.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VoyagesController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;

        public VoyagesController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public async Task<ActionResult<List<Voyage>>> GetAllVoyages()
        {
            var voyages = await _dbContext.Voyages
                .Include(v => v.DeparturePort)
                .Include(v => v.ArrivalPort)
                .Include(v => v.Ship)
                .OrderBy(v => v.VoyageDate)
                .ToListAsync();

            return Ok(voyages);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Voyage>> GetVoyage(int id)
        {
            var voyage = await _dbContext.Voyages
                .Include(v => v.DeparturePort)
                .Include(v => v.ArrivalPort)
                .Include(v => v.Ship)
                .FirstOrDefaultAsync(v => v.Id == id);

            if (voyage == null)
            {
                return NotFound();
            }

            return Ok(voyage);
        }

        [HttpPost]
        public async Task<IActionResult> AddVoyage(AddVoyageDto addVoyageDto)
        {
            if (!await _dbContext.Ports.AnyAsync(p => p.Id == addVoyageDto.DeparturePortId))
                return BadRequest("Departure port not found");

            if (!await _dbContext.Ports.AnyAsync(p => p.Id == addVoyageDto.ArrivalPortId))
                return BadRequest("Arrival port not found");

            if (!await _dbContext.Ships.AnyAsync(s => s.Id == addVoyageDto.ShipId))
                return BadRequest("Ship not found");

            var voyage = new Voyage
            {
                Name = addVoyageDto.Name,
                VoyageDate = addVoyageDto.VoyageDate,
                VoyageStart = addVoyageDto.VoyageStart,
                VoyageEnd = addVoyageDto.VoyageEnd,
                DeparturePortId = addVoyageDto.DeparturePortId,
                ArrivalPortId = addVoyageDto.ArrivalPortId,
                ShipId = addVoyageDto.ShipId
            };

            _dbContext.Voyages.Add(voyage);
            await _dbContext.SaveChangesAsync();

            return Created();
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<Voyage>> UpdateVoyage(int id, UpdateVoyageDto updateVoyageDto)
        {
            var voyage = await _dbContext.Voyages.FindAsync(id);

            if (voyage == null)
            {
                return NotFound();
            }

            if (updateVoyageDto.Name != null)
                voyage.Name = updateVoyageDto.Name;

            if (updateVoyageDto.VoyageDate.HasValue)
                voyage.VoyageDate = updateVoyageDto.VoyageDate.Value;

            if (updateVoyageDto.VoyageStart.HasValue)
                voyage.VoyageStart = updateVoyageDto.VoyageStart.Value;

            if (updateVoyageDto.VoyageEnd.HasValue)
                voyage.VoyageEnd = updateVoyageDto.VoyageEnd.Value;

            if (updateVoyageDto.DeparturePortId.HasValue)
            {
                if (!await _dbContext.Ports.AnyAsync(p => p.Id == updateVoyageDto.DeparturePortId))
                    return BadRequest("Departure port not found");
                voyage.DeparturePortId = updateVoyageDto.DeparturePortId.Value;
            }

            if (updateVoyageDto.ArrivalPortId.HasValue)
            {
                if (!await _dbContext.Ports.AnyAsync(p => p.Id == updateVoyageDto.ArrivalPortId))
                    return BadRequest("Arrival port not found");
                voyage.ArrivalPortId = updateVoyageDto.ArrivalPortId.Value;
            }

            if (updateVoyageDto.ShipId.HasValue)
            {
                if (!await _dbContext.Ships.AnyAsync(s => s.Id == updateVoyageDto.ShipId))
                    return BadRequest("Ship not found");
                voyage.ShipId = updateVoyageDto.ShipId.Value;
            }

            await _dbContext.SaveChangesAsync();
            return Ok(voyage);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteVoyage(int id)
        {
            var voyage = await _dbContext.Voyages.FindAsync(id);

            if (voyage == null)
            {
                return NotFound();
            }

            _dbContext.Voyages.Remove(voyage);
            await _dbContext.SaveChangesAsync();

            return Ok();
        }
    }
}