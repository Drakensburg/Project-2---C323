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
    public class ZoneController : ControllerBase
    {
        private readonly IOTManagerDbContext _context;

        public ZoneController(IOTManagerDbContext context)
        {
            _context = context;
        }

        // GET: api/Zone
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Zone>>> GetZone()
        {
            return await _context.Zone.ToListAsync();
        }

        // GET: api/Zone/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Zone>> GetZone(Guid id)
        {
            var zone = await _context.Zone.FindAsync(id);

            if (zone == null)
            {
                return NotFound();
            }

            return zone;
        }

        // POST: api/Zone
        [HttpPost]
        public async Task<ActionResult<Category>> PostZone(Zone zone)
        {
            _context.Zone.Add(zone);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (ZoneExists(zone.ZoneId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetZone", new { id = zone.ZoneId }, zone);
        }

        // Patch: api/Zone/5
        [HttpPatch("{id}")]
        public async Task<ActionResult<Zone>> ZoneDevice(Guid id)
        {
            var zone = await _context.Zone.FindAsync(id);

            if (zone == null)
            {
                return NotFound();
            }

            _context.Zone.Update(zone);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (ZoneExists(zone.ZoneId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return zone;
        }

        // DELETE: api/Zone/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Zone>> DeleteZone(Guid id)
        {
            var zone = await _context.Zone.FindAsync(id);
            if (zone == null)
            {
                return NotFound();
            }

            _context.Zone.Remove(zone);
            await _context.SaveChangesAsync();

            return zone;
        }

        private bool ZoneExists(Guid id)
        {
            return _context.Zone.Any(e => e.ZoneId == id);
        }

        // GET: api/ZoneDevices/5
        /*[HttpGet("{id}")]
        public async Task<ActionResult<Device>> GetZoneDevices(Guid id)
        {
            var zoneId   = await _context.Zone.FindAsync(id);

            if (zoneId == null)
            {
                return NotFound();
            }
            else if (zoneId.ZoneId == Device)
            {

            }

            return zoneId;
        }*/
    }
}
