using Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Interfaces
{
    public interface ICourierRepository
    {
        Task<IEnumerable<Courier>> GetAllAsync();
        Task AddAsync(Courier courier);
        // Mevcut metodların altına ekle:
        Task DeleteAsync(int id);
        Task<Courier?> GetByIdAsync(int id);
        Task UpdateAsync(Courier courier);
    }
}
