using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sinbad.Context;
using Sinbad.Models;
using System.Net.NetworkInformation;

// GET - Single data per (paramezer = Id) 
// GET - Data per device (parameter = MAC)
// GET - Data per period (parameter = start date ; end date)
// GET - Time series (per year (parameter = year). per month, per day)
// GET - Numbers of disctinct ESP32 

namespace Sinbad.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EspController : ControllerBase
    {
        // This attribute will be used for stock temporarily the DbContext
        private readonly EspContext _context;

        // The constructor is used for stock the DbContext and creade Database if it isn't created...
        public EspController(EspContext context)
        {
            _context = context;
            _context.Database.EnsureCreated();
        }

        // Function for return the line with the id given
        [HttpGet("Id")]
        public async Task<IActionResult> GetId(int id)
        {
            var result = await _context.temp_sensor_table.FindAsync(id);
            if (result != null)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest("Line with id " + id.ToString() + " not found");
            }
        }

        // Function for convert string to DateTime... Used for test functions like GetPeriod
        [HttpGet("DateTime")]
        public DateTime GetDateTime(string date)
        {
            return DateTime.Parse(date);
        }

        // Function for return all lines which have the mac address given
        [HttpGet("MAC")]
        public async Task<List<Esp32>> GetMAC(string mac)
        {
            return await _context.temp_sensor_table
                .Where(item => item.mac == mac)
                .ToListAsync();
        }

        // Function for return all lines which have their date between start and end (include)
        [HttpGet("Period")]
        public async Task<List<Esp32>> GetPeriod(DateTime start, DateTime end)
        {
            return await _context.temp_sensor_table
                .Where(item => item.date >= start && item.date <= end)
                .ToListAsync();
        }

        // Function for return the number of MAC address used
        [HttpGet("MACNumber")]
        public int GetMACNumber()
        {
            return _context.temp_sensor_table.Select(item => item.mac).Distinct().Count();
        }

        // Function for return all lines of our table
        [HttpGet("All")]
        public async Task<List<Esp32>> GetAll()
        {
            // Here we take all data from table temp_sensor_table, and we print result
            return await _context.temp_sensor_table.ToListAsync();
        }

        // Function for return all datas received in the specific year
        [HttpGet("Year")]
        public async Task<IActionResult> GetYear(int year = -1)
        {
            if (year == -1)
            {
                year = DateTime.Now.Year;
            }
            return Ok(await _context.temp_sensor_table
                .Where(item => item.date >= new DateTime(year, 1, 1) && item.date < new DateTime(year + 1, 1, 1))
                .ToListAsync());
        }

        // Function for return all datas received in the specific month
        [HttpGet("Month")]
        public async Task<IActionResult> GetMonth(int month = -1, int year = -1)
        {
            if (year == -1)
            {
                year = DateTime.Now.Year;
            }
            if (month == -1)
            {
                month = DateTime.Now.Month;
            }
            if (month == 12)
            {
                return Ok(await _context.temp_sensor_table
                    .Where(item => item.date >= new DateTime(year, month, 1) && item.date < new DateTime(year + 1, 1, 1))
                    .ToListAsync());
            }
            return Ok(await _context.temp_sensor_table
                .Where (item => item.date >= new DateTime(year, month, 1) && item.date < new DateTime(year, month + 1, 1))
                .ToListAsync());
        }

        // Function for return all datas received in the specific day
        [HttpGet("Day")]
        public async Task<IActionResult> GetDay(int day = -1, int month = -1, int year = -1)
        {
            if (month == -1)
            {
                month = DateTime.Now.Month;
            }
            if (year == -1)
            {
                year = DateTime.Now.Year;
            }
            if (day == -1)
            {
                day = DateTime.Now.Day;
            }
            DateTime date;
            try
            {
                date = new DateTime(year, month, day);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok(await _context.temp_sensor_table
                .Where(item => item.date >= new DateTime(year, month, day, 0, 0, 0) && item.date <= new DateTime(year, month, day, 23, 59, 59, 999))
                .ToListAsync());
        }

        // Function for add new line to our table
        [HttpPost]
        public async Task<IActionResult> Post(string data)
        {
            // Here we define the instruction message
            string help = "\n\nHelp\n------------\nYou must give data in the form MAC;temperature\nMAC is in the form: xx:xx:xx:xx:xx:xx with x, an hexadecimal number\ntemperature is in the form float: x,x or x with x an integer";

            // We verify that parameter given is not null.
            if (string.IsNullOrWhiteSpace(data))
            {
                return BadRequest("No data given" + help);
            }
            // We split parameter (we consider that user enter good parameter)
            var data_given = data.Split(';');
            // If the result is not the result expected, we return an error
            if (data_given.Length != 2)
            {
                return BadRequest("Data isn't in good form..." + help);
            }
            // We create new Esp32 instance, which would be used for insert new line in our database
            Esp32 newdata = new Esp32();
            // We try to parse float and mac address. If it didn't work, it means that the data aren't in good form...
            // And we complete the instance of Esp32 if no problem comes
            try
            {
                newdata.temp = float.Parse(data_given[1]);
                PhysicalAddress.Parse(data_given[0]);
                newdata.mac = data_given[0];
                newdata.date = DateTime.Now;
            }
            // Here we catch and return exceptions to http client
            catch (Exception ex)
            {
                return BadRequest(ex.Message + help);
            }

            // We add new line to our table
            _context.temp_sensor_table.Add(newdata);

            // We update our database
            await _context.SaveChangesAsync();

            // We indicate that's ok
            return Ok("Datas have been inserted successfully to the table");
        }

        // Function for delete the database or a specified line
        [HttpDelete]
        public async Task<IActionResult> Delete(int id = 0, bool cleanDataBase = false)
        {
            // If value true is given, it means that the user want to delete the database. So we delete the database.
            if (cleanDataBase)
            {
                _context.Database.EnsureDeleted();
                await _context.SaveChangesAsync();
                return Ok("Data Base cleaned successfully");
            }
            // Else, we search an item of id like id given in our table. (Note that id are set from 1 to ?)
            var item_to_remove = await _context.temp_sensor_table.FindAsync(id);

            // If we don't find item with id given, we indicate it to the client
            if (item_to_remove == null)
            {
                return BadRequest("Bad Request...\n\nPrehaps, the item with the id given doesn't exist in the database\n\nHelp\n----------\nEither you give the id of item to delete (int)\nor you give true value for delete Database(it cleans all datas)");
            }

            // Else we remove our item
            else
            {
                _context.temp_sensor_table.Remove(item_to_remove);
                await _context.SaveChangesAsync();
            }

            // We indicate to the client that's all right
            return Ok("Line deleted successfully");
        }


    }
}
