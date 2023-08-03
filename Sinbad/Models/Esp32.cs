using Microsoft.EntityFrameworkCore;
using System.Net.NetworkInformation;

namespace Sinbad.Models
{
    // Here we have the column of our table temp_sensor_table
    public class Esp32
    {
        public int Id { get; set; }
        public string mac { get; set; }
        public float temp { get; set; }
        public DateTime date { get; set; }
    }
}
