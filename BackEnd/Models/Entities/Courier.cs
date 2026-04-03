using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Entities
{
    public class Courier
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string VehicleType { get; set; } = string.Empty; // Motor, Araba vb.
        public bool IsActive { get; set; } = true;

        public double LastLatitude { get; set; }
        public double LastLongitude { get; set; }
    }
}
