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
    public class DeviceController : ControllerBase
    {
        private readonly IOTManagerDbContext _context;

        public DeviceController(IOTManagerDbContext context)
        {
            _context = context;
        }

        // GET: api/Device
        [HttpGet("Retrieve all Devices")]
        public async Task<ActionResult<IEnumerable<Device>>> GetDevice()
        {
            return await _context.Device.ToListAsync();
        }

        // GET: api/Device/5
        [HttpGet("Retrieve Device via ID")]
        public async Task<ActionResult<Device>> GetDevice(Guid id)
        {
            var device = await _context.Device.FindAsync(id);

            if (device == null)
            {
                return NotFound();
            }

            return device;
        }

        // POST: api/Device
        [HttpPost("Post new Device")]
        public async Task<ActionResult<Device>> PostDevice(Device device)
        {
            _context.Device.Add(device);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (DeviceExists(device.DeviceId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetDevice", new { id = device.CategoryId }, device);
        }

        // PATCH: api/Device/5
        [HttpPatch("Patch Device via ID")]
        public async Task<ActionResult<Device>> UpdateDevice(Guid DeviceId, [FromBody] string DeviceName, Guid CategoryId, Guid ZoneId, string Status, bool IsActvie, DateTime DateCreated)
        {
            var device = await _context.Device.FindAsync(DeviceId);

            device.DeviceName   = DeviceName;
            device.CategoryId   = CategoryId;
            device.ZoneId       = ZoneId;
            device.Status       = Status;
            device.IsActvie     = IsActvie;
            device.DateCreated  = DateCreated;

            return device;
        }

        // DELETE: api/Device/5
        [HttpDelete("Delete Device via ID")]
        public async Task<ActionResult<Device>> DeleteDevice(Guid id)
        {
            var device = await _context.Device.FindAsync(id);
            if (device == null)
            {
                return NotFound();
            }

            _context.Device.Remove(device);
            await _context.SaveChangesAsync();

            return device;
        }

        private bool DeviceExists(Guid id)
        {
            return _context.Device.Any(e => e.DeviceId == id);
        }
    }
}
