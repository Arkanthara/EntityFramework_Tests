using Microsoft.EntityFrameworkCore;

namespace Sinbad.Models
{
    [Keyless]
    public class Esp32
    {
        public int id { get; set; }
        public string mac { get; set; }
        public double temp { get; set; }
        public DateTime date { get; set; }
    }
}
