using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using IOT_Device_Manager.Repository;
using IOT_Device_Manager.Repository.Models;

namespace IOT_Device_Manager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly IOTManagerDbContext _context;

        public CategoryController(IOTManagerDbContext context)
        {
            _context = context;
        }

        // GET: api/Category
        [HttpGet("Retrieve all Categories")]
        public async Task<ActionResult<IEnumerable<Category>>> GetCategory()
        {
            return await _context.Category.ToListAsync();
        }

        // GET: api/Category/5
        [HttpGet("Retrieve Category via ID")]
        public async Task<ActionResult<Category>> GetCategory(Guid id)
        {
            var category = await _context.Category.FindAsync(id);

            if (category == null)
            {
                return NotFound();
            }

            return category;
        }

        // POST: api/Category
        [HttpPost("Post Category")]
        public async Task<ActionResult<Category>> PostCategory(Category category)
        {
            _context.Category.Add(category);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (CategoryExists(category.CategoryId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetCategory", new { id = category.CategoryId }, category);
        }

        // PATCH: api/Category/5
        [HttpPatch("Patch Category via ID")]
        public async Task<ActionResult<Category>> UpdateCategory(Guid CategoryId, [FromBody] string CategoryName, string CategoryDescription, DateTime DateCreated)
        {
            var category = await _context.Category.FindAsync(CategoryId);

            category.CategoryName        = CategoryName;
            category.CategoryDescription = CategoryDescription;
            category.DateCreated         = DateCreated;

            return category;
        }

        // DELETE: api/Category/5
        [HttpDelete("Delete Category via ID")]
        public async Task<ActionResult<Category>> DeleteCategory(Guid id)
        {
            var category = await _context.Category.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }

            _context.Category.Remove(category);
            await _context.SaveChangesAsync();

            return category;
        }

        private bool CategoryExists(Guid id)
        {
            return _context.Category.Any(e => e.CategoryId == id);
        }

        // GET: api/CategoryDevices/5
        [HttpGet("Retrieve Category defined Devices")]
        public async Task<List<Device>> GetCategoryDevices(Guid id)
        {

            var query = from category in _context.Category
                        join device in _context.Device
                        on category.CategoryId equals device.CategoryId
                        where category.CategoryId == id
                        select new
                        {
                            device.DeviceId,
                            device.DeviceName,
                            device.CategoryId
                        };

            var CategoryDevices = await query.ToListAsync().ConfigureAwait(false);

            return CategoryDevices
                    .Select(CategoryDevices => new Device()
                    {
                        DeviceId = CategoryDevices.DeviceId,
                        DeviceName = CategoryDevices.DeviceName,
                        CategoryId = CategoryDevices.CategoryId
                    })
                        .ToList();
        }

        // GET: api/CountZones/5
        [HttpGet("Count Zones per Categories")]
        public int CountZonesPerCategories()
        {

            var query = from device in _context.Device
                        join category in _context.Category
                        on device.CategoryId equals category.CategoryId
                        join zone in _context.Zone
                        on device.ZoneId equals zone.ZoneId
                        where category.CategoryId == device.CategoryId & device.ZoneId == zone.ZoneId
                        select new
                        {
                            zone.ZoneId,
                            zone.ZoneName,
                        };

            int Count = query.Count();

            return Count;
        }
    }
}
