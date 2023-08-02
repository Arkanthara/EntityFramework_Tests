using Marina.DTO;
using System.Data.Entity;

namespace Marina.DAL
{
    public class Esp32Context : DbContext
    {
        public Esp32Context() :base("name=TestDocker"){ }

        public DbSet<Esp32> temp_sensor_table { get; set; }
    }
}
