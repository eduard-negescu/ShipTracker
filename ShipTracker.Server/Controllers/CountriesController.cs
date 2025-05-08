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
            if (addCountryDto == null)
            {
                return BadRequest("Country data cannot be null");
            }

            Country countryEntity = new()
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
            if (updateCountryDto == null)
            {
                return BadRequest("Update data cannot be null");
            }

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

        [HttpGet("get_by_num_ports")]
        public async Task<ActionResult<List<GetCountriesByNumPortsDto>>> GetCountriesByNumberOfPorts()
        {
            var countriesWithPortCounts = await dbContext.Countries
                .GroupJoin(
                    dbContext.Ports,
                    country => country.Id,
                    port => port.CountryId,
                    (country, ports) => new GetCountriesByNumPortsDto
                    {
                        CountryId = country.Id,
                        CountryName = country.Name,
                        NumberOfPorts = ports.Count()
                    })
                .OrderByDescending(c => c.NumberOfPorts)
                .ToListAsync();

            return Ok(countriesWithPortCounts);
        }
    }
}
