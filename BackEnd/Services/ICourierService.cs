using Models.DTOs;
using Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public interface ICourierService
    {
        Task<IEnumerable<Courier>> GetAllCouriersAsync();
        Task CreateCourierAsync(CourierCreateDto courier);
        Task DeleteCourierAsync(int id);
        Task UpdateLocationAsync(int courierId, double lat, double lon);
    }
}
