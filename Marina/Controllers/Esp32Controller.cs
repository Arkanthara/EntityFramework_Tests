using Microsoft.AspNetCore.Mvc;
using Marina.DAL;
using Marina.DTO;
using System.Data.Entity;
using System.Net.NetworkInformation;

namespace Marina.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Esp32Controller : ControllerBase
    {
        [HttpGet]
        public async Task<List<Esp32>> Get()
        {
            List<Esp32> result;
            using (var context = new Esp32Context())
            {
                result =  await context.temp_sensor_table.ToListAsync();
            }
            return result;
        }
        [HttpPost]
        public async Task<IActionResult> Post(string data)
        {
            string help = "\n\nHelp\n---------------\nYou must give data in the form MAC;temperature\nMAC: X:X:X:X:X:X with X = two hexadecimal number\ntemperature: X,X with X an integer";
            if (string.IsNullOrWhiteSpace(data))
            {
                return BadRequest("Error: data aren't in the good form !" + help);
            }
            var item = data.Split(';');
            if (item.Length != 2)
            {
                return BadRequest("Error: data aren't in the good form !" + help);
            }
            using(var context = new Esp32Context())
            {
                try
                {
                    Esp32 esp32 = new Esp32();
                    PhysicalAddress.Parse(item[0]);
                    esp32.mac = item[0];
                    esp32.temp = Double.Parse(item[1]);
                    esp32.date = DateTime.Now;
                    context.temp_sensor_table.Add(esp32);
                    await context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message + help);
                }
            }
            return Ok("Data where added successfully");
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id = 0, bool cleanDataBase = false)
        {
            string help = "\n\nHelp\n----------\nEither you give to the function an id (integer) for delete the line with this id\nor you give true for destroy the database";
            if (cleanDataBase)
            {
                using (var context = new Esp32Context())
                {
                    context.Database.Delete();
                }
                return Ok("Database has been destroyed successfully");
            }
            Esp32 item_to_delete;
            using (var context = new Esp32Context())
            {
                item_to_delete = await context.temp_sensor_table.FindAsync(id);
                if (item_to_delete != null)
                {
                    context.temp_sensor_table.Remove(item_to_delete);
                    await context.SaveChangesAsync();
                }

            }
            if (item_to_delete == null)
            {
                return BadRequest("Error when trying to delete this element... Perhaps he doesn't exist in the database..." + help);
            }
            return Ok("Item deleted successfully");
        }
    }
}
