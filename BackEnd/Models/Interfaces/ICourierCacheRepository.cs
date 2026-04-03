using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Interfaces
{
    public interface ICourierCacheRepository
    {
        Task UpdateLocationAsync(int courierId, double lat, double lon);
        Task RemoveLocationAsync(int courierId);
        Task<List<string>> GetNearbyCouriersAsync(double lat, double lon, double radiusKm);
    }
}
