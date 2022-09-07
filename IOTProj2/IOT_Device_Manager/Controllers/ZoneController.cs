using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using IOT_Device_Manager.Repository;
using IOT_Device_Manager.Repository.Models;
using Microsoft.AspNetCore.Authorization;

namespace IOT_Device_Manager.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class ZoneController : ControllerBase
    {
        private readonly IOTManagerDbContext _context;

        public ZoneController(IOTManagerDbContext context)
        {
            _context = context;
        }

        // GET: api/Zone
        [HttpGet("Retrieve all Zones")]
        public async Task<ActionResult<IEnumerable<Zone>>> GetZone()
        {
            return await _context.Zone.ToListAsync();
        }

        // GET: api/Zone/5
        [HttpGet("Retrieve Zone via ID")]
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
        [HttpPost("Post new Zone")]
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

        // PATCH: api/Zone/5
        [HttpPatch("Patch Zone via ID")]
        public async Task<ActionResult<Zone>> UpdateZone(Guid ZoneId, [FromBody] string ZoneName, string ZoneDescription, DateTime DateCreated)
        {
            var zone = await _context.Zone.FindAsync(ZoneId);

            zone.ZoneName        = ZoneName;
            zone.ZoneDescription = ZoneDescription;
            zone.DateCreated     = DateCreated;

            return zone;
        }

        // DELETE: api/Zone/5
        [HttpDelete("Delete Zone via ID")]
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
        [HttpGet("Retrieve Zone defined Devices")]
        public async Task<List<Device>> GetZoneDevices(Guid id)
        {

            var query = from zone in _context.Zone
                        join device in _context.Device
                        on zone.ZoneId equals device.ZoneId
                        where zone.ZoneId == id
                        select new
                        {
                            device.DeviceId,
                            device.DeviceName,
                            device.ZoneId
                        };

            var ZoneDevices = await query.ToListAsync().ConfigureAwait(false);

            return ZoneDevices
                    .Select(ZoneDevices => new Device()
                    {
                         DeviceId = ZoneDevices.DeviceId,
                         DeviceName = ZoneDevices.DeviceName,
                         ZoneId = ZoneDevices.ZoneId
                    })
                        .ToList();
        }
    }
}
