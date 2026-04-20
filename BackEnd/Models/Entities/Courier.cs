
namespace Models.Entities
{
    public class Courier
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string VehicleType { get; set; } = string.Empty; 
        public bool IsActive { get; set; } = true;

        public double LastLatitude { get; set; }
        public double LastLongitude { get; set; }
        public Boolean IsAvailable { get; set; } = true;

        public int? ActiveOrderId { get; set; }
    }
}
