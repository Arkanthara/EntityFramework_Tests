﻿using Microsoft.EntityFrameworkCore;


namespace Sinbad.Models
{
    public class Esp32
    {
        public int Id { get; set; }
        public string mac { get; set; }
        public double temp { get; set; }
        public DateTime date { get; set; }
    }
}