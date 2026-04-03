using Microsoft.EntityFrameworkCore;
using Models.Entities;
using Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class CourierRepository : ICourierRepository
    {
        private readonly AppDbContext _context;
        public CourierRepository(AppDbContext context) => _context = context;

        public async Task AddAsync(Courier courier)
        {
            await _context.Couriers.AddAsync(courier);
            await _context.SaveChangesAsync();
            
        }
        public async Task DeleteAsync(int id)
        {
            var courier = await _context.Couriers.FindAsync(id);
            if (courier != null)
            {
                _context.Couriers.Remove(courier);
                await _context.SaveChangesAsync();
            }
        }
        public async Task<IEnumerable<Courier>> GetAllAsync()
        {
            return await _context.Couriers.ToListAsync();
        }
        public async Task<Courier?> GetByIdAsync(int id)
        {
            // Id'ye göre kuryeyi bul (Bulamazsa null döner)
            return await _context.Couriers.FindAsync(id);
        }

        public async Task UpdateAsync(Courier courier)
        {
            // Kuryeyi güncelle ve veritabanına kaydet
            _context.Couriers.Update(courier);
            await _context.SaveChangesAsync();
        }
    }
}
