using Microsoft.EntityFrameworkCore;
using Models.Entities;
using Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class OrderRepository : IOrderRepository
    {
        private AppDbContext _context;
        public OrderRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task AddAsync(Order order)
        {
            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var _order = await _context.Orders.FindAsync(id);
            if (_order != null) {
                _context.Orders.Remove(_order);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Order>> GetAllAsync()
        {
            return await _context.Orders.ToListAsync<Order>();
        }

        public async Task<Order?> GetByIdAsync(int id)
        {
           
            return await _context.Orders.FindAsync(id);
        }

        public async Task UpdateAsync(Order order)
        {
            _context.Orders.Update(order);
        }
    }
}
