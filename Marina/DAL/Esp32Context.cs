using Marina.DTO;
using System.Data.Entity;

namespace Marina.DAL
{
    public class Esp32Context : DbContext
    {
        public Esp32Context() :base("Test"){ }

        public DbSet<Esp32> Temp_Table { get; set; }
    }
}
