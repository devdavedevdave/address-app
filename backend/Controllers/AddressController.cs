using Microsoft.AspNetCore.Mvc;
using address_management.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq.Dynamic.Core;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace address_management.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AddressController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<AddressController> _logger;

        public AddressController(ApplicationDbContext context, ILogger<AddressController> logger)
        {
            _context = context;
            _logger = logger;
        }

        /* // GET: /Address
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Address>>> Get([FromQuery] string sortBy = "Id", [FromQuery] string sortDirection = "asc")
        {
            var query = _context.Addresses.AsQueryable();

            switch (sortBy.ToLower())
            {
                case "street":
                    query = (sortDirection.ToLower() == "asc") ? query.OrderBy(a => a.Street) : query.OrderByDescending(a => a.Street);
                    break;
                case "city":
                    query = (sortDirection.ToLower() == "asc") ? query.OrderBy(a => a.City) : query.OrderByDescending(a => a.City);
                    break;
                case "state":
                    query = (sortDirection.ToLower() == "asc") ? query.OrderBy(a => a.State) : query.OrderByDescending(a => a.State);
                    break;
                case "postalcode":
                    query = (sortDirection.ToLower() == "asc") ? query.OrderBy(a => a.PostalCode) : query.OrderByDescending(a => a.PostalCode);
                    break;
                case "country":
                    query = (sortDirection.ToLower() == "asc") ? query.OrderBy(a => a.Country) : query.OrderByDescending(a => a.Country);
                    break;
                default:
                    query = (sortDirection.ToLower() == "asc") ? query.OrderBy(a => a.Id) : query.OrderByDescending(a => a.Id);
                    break;
            }

            return await query.ToListAsync();
        } */


        // GET: /Address/
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Address>>> Get(
            [FromQuery] string sortBy = "Id",
            [FromQuery] string sortDirection = "asc",
            [FromQuery] string street = null,
            [FromQuery] string city = null,
            [FromQuery] string state = null,
            [FromQuery] string postalCode = null,
            [FromQuery] string country = null,
            [FromQuery] string searchQuery = null)
        {
            sortDirection = sortDirection.Equals("asc", StringComparison.OrdinalIgnoreCase) ? "asc" : "desc";

            string ConvertToPascalCase(string s) =>
                Char.ToUpperInvariant(s[0]) + s.Substring(1);

            sortBy = ConvertToPascalCase(sortBy);

            var propertyInfo = typeof(Address).GetProperty(sortBy);
            if (propertyInfo == null)
            {
                return BadRequest($"Invalid sort parameter: '{sortBy}'. Valid parameters include 'Id', 'Street', 'City', 'State', 'PostalCode', 'Country'.");
            }

            var query = _context.Addresses.AsQueryable();

            // General Search Filtering
            if (!string.IsNullOrWhiteSpace(searchQuery))
            {
                query = query.Where(a => a.Street.Contains(searchQuery)
                                            || a.City.Contains(searchQuery)
                                            || a.State.Contains(searchQuery)
                                            || a.PostalCode.Contains(searchQuery)
                                            || a.Country.Contains(searchQuery));
            }

            // Filtering
            if (!string.IsNullOrWhiteSpace(street))
            {
                query = query.Where(a => a.Street.Contains(street));
            }
            if (!string.IsNullOrWhiteSpace(city))
            {
                query = query.Where(a => a.City.Contains(city));
            }
            if (!string.IsNullOrWhiteSpace(state))
            {
                query = query.Where(a => a.State.Contains(state));
            }
            if (!string.IsNullOrWhiteSpace(postalCode))
            {
                query = query.Where(a => a.PostalCode.Contains(postalCode));
            }
            if (!string.IsNullOrWhiteSpace(country))
            {
                query = query.Where(a => a.Country.Contains(country));
            }

            // Sorting
            query = query.OrderBy($"{sortBy} {sortDirection}");

            return await query.ToListAsync();
        }



        // GET: /Address/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Address>> Get(int id)
        {
            var address = await _context.Addresses.FirstOrDefaultAsync(a => a.Id == id);
            if (address == null)
            {
                return NotFound();
            }
            return address;
        }

        // POST: /Address
        [HttpPost]
        public async Task<ActionResult<Address>> Post([FromBody] Address address)
        {
            _context.Addresses.Add(address);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(Get), new { id = address.Id }, address);
        }

        // PUT: /Address/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] Address updatedAddress)
        {
            if (id != updatedAddress.Id)
            {
                return BadRequest();
            }

            _context.Entry(updatedAddress).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Addresses.Any(e => e.Id == id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: /Address/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var address = await _context.Addresses.FindAsync(id);
            if (address == null)
            {
                return NotFound();
            }

            _context.Addresses.Remove(address);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
