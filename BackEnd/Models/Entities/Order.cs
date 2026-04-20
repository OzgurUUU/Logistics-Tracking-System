using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Entities
{
    public enum OrderStatus
    {
        Pending = 0,    
        Assigned = 1,   
        Delivered = 2   
    }
    public class Order
    {
        public int Id { get; set; }
        public string CostumerName { get; set; }
        public double DeliveryLatitude { get; set; }
        public double DeliveryLongitude { get; set; }
        public OrderStatus Status { get; set; } = OrderStatus.Pending;

        public int? AssignedCouirerId { get; set; }
    }
}
