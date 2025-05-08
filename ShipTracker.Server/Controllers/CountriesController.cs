using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShipTracker.Server.Database;
using ShipTracker.Server.Models;
using ShipTracker.Server.Models.Entities;

namespace ShipTracker.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountriesController : ControllerBase
    {
        private readonly ApplicationDbContext dbContext;

        public CountriesController(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet]
        public async Task<ActionResult<List<Country>>> GetAllCountries()
        {
            // Fixed the incorrect usage of OrderBy and corrected the property name  
            var countries = await dbContext.Countries.OrderBy(c => c.Name).ToListAsync();

            return Ok(countries);
        }

        [HttpPost]
        public async Task<IActionResult> AddCountry(AddCountryDto addCountryDto)
        {
            Country countryEntity = new Country()
            {
                Name = addCountryDto.Name
            };

            dbContext.Countries.Add(countryEntity);
            await dbContext.SaveChangesAsync();

            return Created();
        }

        [HttpPut]
        [Route("{id:int}")]
        public async Task<ActionResult<Country>> UpdateCountry(int id, UpdateCountryDto updateCountryDto)
        {
            var country = await dbContext.Countries.FindAsync(id);

            if (country == null)
            {
                return NotFound();
            }

            country.Name = updateCountryDto.Name;
            await dbContext.SaveChangesAsync();
            return Ok(country);
        }

        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IActionResult> DeleteCountry(int id)
        {
            var country = await dbContext.Countries.FindAsync(id);

            if (country == null)
            {
                return NotFound();
            }

            dbContext.Remove(country);
            await dbContext.SaveChangesAsync();

            return Ok();
        }
    }
}
