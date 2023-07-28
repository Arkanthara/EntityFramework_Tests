using Microsoft.EntityFrameworkCore;
using Sinbad.Models;
using System.Runtime.CompilerServices;

namespace Sinbad.Context
{
    public class EspContext: DbContext
    {
        public EspContext(DbContextOptions<EspContext> options) : base(options) { }


        public DbSet<Esp32> temp_sensor_table { get; set; }

 
    }
}
