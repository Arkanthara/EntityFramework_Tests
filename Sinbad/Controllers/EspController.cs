using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sinbad.Context;
using Sinbad.Models;
using System.Net.NetworkInformation;

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
        public async Task<IActionResult> Post(string data)
        {
            string help = "\n\nHelp\n------------\nYou must give data in the form MAC;temperature\nMAC is in the form: xx:xx:xx:xx:xx:xx with x, an hexadecimal number\ntemperature is in the form float: x,x or x with x an integer";
            if (string.IsNullOrWhiteSpace(data))
            {
                return BadRequest("No data given" + help);
            }
            var data_given = data.Split(';');
            if (data_given.Length != 2)
            {
                return BadRequest("Data isn't in good form..." + help);
            }
            Esp32 newdata = new Esp32();
            try
            {
                newdata.temp = float.Parse(data_given[1]);
                PhysicalAddress.Parse(data_given[0]);
                newdata.mac = data_given[0];
                newdata.date = DateTime.Now;
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message + help);
            }
            _context.temp_sensor_table.Add(newdata);
            await _context.SaveChangesAsync();
            return Ok("Datas have been inserted successfully to the table");
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id = 0, bool cleanDataBase = false)
        {
            if (cleanDataBase)
            {
                _context.Database.EnsureDeleted();
                await _context.SaveChangesAsync();
                return Ok("Data Base cleaned successfully");
            }
            var item_to_remove = await _context.temp_sensor_table.FindAsync(id);
            if (item_to_remove == null)
            {
                return BadRequest("Bad Request...\n\nPrehaps, the item with the id given doesn't exist in the database\n\nHelp\n----------\nEither you give the id of item to delete (int)\nor you give true value for delete Database(it cleans all datas)");
            }
            else
            {
                _context.temp_sensor_table.Remove(item_to_remove);
                await _context.SaveChangesAsync();
            }
            return Ok("Line deleted successfully");
        }


    }
}
