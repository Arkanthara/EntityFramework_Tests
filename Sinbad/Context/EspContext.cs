using Microsoft.EntityFrameworkCore;
using Sinbad.Models;
using System.Runtime.CompilerServices;

namespace Sinbad.Context
{
    public class EspContext: DbContext
    {
        // public EspContext() : base("name=myDbConnectionString") { }

        public EspContext(DbContextOptions<EspContext> options) : base(options) { }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        // {
        //     optionsBuilder.UseSqlServer("name=ConnectionStrings:myDbConnectionString");
        // }
        public DbSet<Esp32> temp_sensor_table { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DbSet<Esp32>>().ToTable("temp_sensor_table");
        }
    }
}
