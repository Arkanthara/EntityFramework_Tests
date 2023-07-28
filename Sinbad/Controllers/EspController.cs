using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sinbad.Context;
using Sinbad.Models;

namespace Sinbad.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EspController : ControllerBase
    {
        private readonly EspContext _context;

        public EspController(EspContext context)
        {
            _context = context;
            _context.Database.EnsureCreated();
        }

        [HttpGet]
        public async Task<List<Esp32>> Get()
        {
            var result = await _context.temp_sensor_table.ToListAsync();
            return result;
        }

        [HttpPost]
        public async Task<IActionResult> Post(Esp32 newesp32)
        {
            _context.temp_sensor_table.Add(newesp32);
            await _context.SaveChangesAsync();
            return Ok(newesp32);
        }


    }
}
