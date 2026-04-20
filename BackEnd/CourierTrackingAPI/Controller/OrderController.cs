using Microsoft.AspNetCore.Mvc;
using Models.DTOs;
using Services;
using Services.Interfaces;

namespace CourierTrackingAPI.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController :ControllerBase
    {
        private readonly IOrderService _orderService;
        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }
        [HttpPost("create-and-assign")]
        public async Task<IActionResult> AddOrder([FromBody] OrderCreateDto request)
        {
            try
            {
                var order = await _orderService.CreateOrderAndAssignCourierAsync(
                    request.CustomerName,
                    request.Latitude,
                    request.Longitude
                );

                if (order == null)
                {
                    return BadRequest(new
                    {
                        Message = "Operasyon durduruldu: Şu an sahada boş kurye bulunmamaktadır."
                    });
                }
                return Ok(new
                {
                    Message = "Sipariş başarıyla oluşturuldu ve en yakın kuryeye atandı!",
                    OrderId = order.Id,
                    AssignedCourierId = order.AssignedCouirerId,
                    Status = order.Status.ToString()
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = "Sistem hatası: " + ex.Message });
            }
        }
    }
}
