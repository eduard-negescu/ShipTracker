using Microsoft.AspNetCore.Http;
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
            var countries = await dbContext.Countries.ToListAsync();

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
        public IActionResult UpdateCountry()
        {
            return Ok();
        }
    }
}
