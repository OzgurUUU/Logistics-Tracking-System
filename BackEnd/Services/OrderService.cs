using Microsoft.EntityFrameworkCore;
using Models.Entities;
using Repository;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class OrderService : IOrderService
    {
        private AppDbContext _context;
        public OrderService(AppDbContext context)
        {
            _context = context;
        }
        public async Task<Order?> CreateOrderAndAssignCourierAsync(string customerName, double lat, double lon)
        {
            var order = new Order{
                CostumerName = customerName,
                DeliveryLatitude = lat,
                DeliveryLongitude = lon,
                Status = OrderStatus.Pending
            };
            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();

            var Couirers = await _context.Couriers.Where(x => x.IsAvailable).ToListAsync<Courier>();
            if(Couirers.Count == 0) return null;
            var AvaibleCouirers = new List<Courier>();
            foreach(var courier in Couirers)
            {
                if (courier.IsAvailable)
                {
                    AvaibleCouirers.Add(courier);
                }
            }
            var closest = AvaibleCouirers.MinBy(x => GeoHelper.CalculateDistance(lat, lon, x.LastLatitude, x.LastLongitude));
            if (closest != null)
            {
                closest.IsAvailable = false;
                closest.ActiveOrderId = order.Id;
                order.Status = OrderStatus.Assigned;
                order.AssignedCouirerId = closest.Id;
                await _context.SaveChangesAsync();
            }
            return order;
        }
    }
}
