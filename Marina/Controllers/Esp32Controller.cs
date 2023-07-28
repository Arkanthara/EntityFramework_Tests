using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Marina.DAL;
using Marina.DTO;

namespace Marina.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Esp32Controller : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Post(Esp32 esp32)
        {
            using(var context = new Esp32Context())
            {
                context.Temp_Table.Add(esp32);
                await context.SaveChangesAsync();
            }
            return Ok(esp32);
        }
    }
}
