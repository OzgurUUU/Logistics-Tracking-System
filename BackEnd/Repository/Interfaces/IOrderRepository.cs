using Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Interfaces
{
    public interface IOrderRepository
    {
        Task<IEnumerable<Order>> GetAllAsync();
        Task AddAsync(Order order);

        Task DeleteAsync(int id);
        Task<Order?> GetByIdAsync(int id);
        Task UpdateAsync(Order order);
    }
}
