using Models.DTOs;
using Models.Entities;


namespace Services.Interfaces
{
    public interface ICourierService
    {
        Task<IEnumerable<Courier>> GetAllCouriersAsync();
        Task CreateCourierAsync(CourierCreateDto courier);
        Task DeleteCourierAsync(int id);
        Task UpdateLocationAsync(int courierId, double lat, double lon);
    }
}
