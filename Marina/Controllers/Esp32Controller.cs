using Microsoft.AspNetCore.Mvc;
using Marina.DAL;
using Marina.DTO;
using System.Data.Entity;


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
                result =  await context.Temp_Table.ToListAsync();
            }
            return result;
        }
        [HttpPost]
        public async Task<IActionResult> Post(string mac = "GnorfGnorf", double temperature = 37.5)
        {
            using(var context = new Esp32Context())
            {
                Esp32 esp32 = new Esp32();
                esp32.mac = mac;
                esp32.temp = temperature;
                esp32.date = DateTime.Now;
                context.Temp_Table.Add(esp32);
                await context.SaveChangesAsync();
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
                return Ok("Database destroy successfully");
            }
            Esp32 item_to_delete;
            using (var context = new Esp32Context())
            {
                item_to_delete = await context.Temp_Table.FindAsync(id);
                if (item_to_delete != null)
                {
                    context.Temp_Table.Remove(item_to_delete);
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
