using Microsoft.EntityFrameworkCore;
using Models.Entities;
using Repository.Interfaces;



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
            return await _context.Couriers.FindAsync(id);
        }

        public async Task UpdateAsync(Courier courier)
        {
            _context.Couriers.Update(courier);
            await _context.SaveChangesAsync();
        }
    }
}
