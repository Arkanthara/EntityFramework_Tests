using Microsoft.EntityFrameworkCore;
using Sinbad.Models;

namespace Sinbad.Context
{
    // It's the contexto of our database
    public class EspContext: DbContext
    {
        // This method is used for add context to services at the begining of the program
        public EspContext(DbContextOptions<EspContext> options) : base(options) { }

        // This is a table with column like the attributes of Esp32
        public DbSet<Esp32> temp_sensor_table { get; set; }

 
    }
}
