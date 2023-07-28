using Microsoft.EntityFrameworkCore;
using System.Net.NetworkInformation;

namespace Sinbad.Models
{
    public class Esp32
    {
        public int Id { get; set; }
        public string mac { get; set; }
        public float temp { get; set; }
        public DateTime date { get; set; }
    }
}
