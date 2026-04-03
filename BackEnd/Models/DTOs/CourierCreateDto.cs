using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DTOs
{
    public class CourierCreateDto
    {
        // Id göndermiyoruz! Veritabanı bunu halledecek.
        public string Name { get; set; } = string.Empty;
        public string VehicleType { get; set; } = string.Empty;
        public double LastLatitude { get; set; }
        public double LastLongitude { get; set; }
    }
}
