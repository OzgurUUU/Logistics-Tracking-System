
namespace Models.DTOs
{
    public class CourierCreateDto
    {
        public string Name { get; set; } = string.Empty;
        public string VehicleType { get; set; } = string.Empty;
        public double LastLatitude { get; set; }
        public double LastLongitude { get; set; }
    }
}
