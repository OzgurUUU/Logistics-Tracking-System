
using Models.Entities;

namespace Repository.Interfaces
{
    public interface ICourierCacheRepository
    {
        Task UpdateLocationAsync(int courierId, double lat, double lon);
        Task RemoveLocationAsync(int courierId);
        Task<List<string>> GetNearbyCouriersAsync(double lat, double lon, double radiusKm);
    }

}
