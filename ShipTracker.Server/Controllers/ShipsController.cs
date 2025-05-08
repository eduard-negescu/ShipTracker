using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShipTracker.Server.Database;
using ShipTracker.Server.Models;
using ShipTracker.Server.Models.Entities;

namespace ShipTracker.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShipsController : ControllerBase
    {
        private readonly ApplicationDbContext dbContext;

        public ShipsController(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

      

        [HttpGet]
        public async Task<ActionResult<List<Ship>>> GetAllShips()
        {
            var ships = await dbContext.Ships
                .OrderBy(s => s.Name)
                .ToListAsync();

            return Ok(ships);
        }

        [HttpPost]
        public async Task<IActionResult> AddShip(AddShipDto addShipDto)
        {
            var ship = new Ship
            {
                Name = addShipDto.Name,
                MaximumSpeed = addShipDto.MaximumSpeed
            };

            dbContext.Ships.Add(ship);
            await dbContext.SaveChangesAsync();

            return Created();
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateShip(int id, UpdateShipDto updateShipDto)
        {
            var ship = await dbContext.Ships.FindAsync(id);

            if (ship == null)
            {
                return NotFound();
            }

            if (updateShipDto.Name != null)
            {
                ship.Name = updateShipDto.Name;
            }

            if (updateShipDto.MaximumSpeed.HasValue)
            {
                ship.MaximumSpeed = updateShipDto.MaximumSpeed.Value;
            }

            await dbContext.SaveChangesAsync();

            return Ok();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteShip(int id)
        {
            var ship = await dbContext.Ships.FindAsync(id);

            if (ship == null)
            {
                return NotFound();
            }

            dbContext.Ships.Remove(ship);
            await dbContext.SaveChangesAsync();

            return Ok();
        }
    }
}