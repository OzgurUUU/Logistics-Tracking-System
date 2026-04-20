using Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IOrderService
    {
        Task<Order?> CreateOrderAndAssignCourierAsync(string customerName, double lat, double lon);
    }
}
