using CourierTrackingAPI;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Models.DTOs;
using Models.Entities;
using Services.Interfaces;


namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CourierController : ControllerBase
    {
        private readonly ICourierService _service;
        private readonly IHubContext<CourierHub> _hubContext;
        public CourierController(ICourierService service,IHubContext<CourierHub> hubContext)
        {
            _service = service;
            _hubContext = hubContext;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var couriers = await _service.GetAllCouriersAsync();
            return Ok(couriers);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCourier(int id)
        {
            try
            {
                await _service.DeleteCourierAsync(id);
                await _hubContext.Clients.All.SendAsync("CourierDeleted", id);
                return Ok(new { Message = $"Kurye ({id}) başarıyla sistemden ve haritadan silindi." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Error = "Kurye silinirken bir hata oluştu: " + ex.Message });
            }
        }
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CourierCreateDto courierDto)
        {
            await _service.CreateCourierAsync(courierDto);
            return Ok("Kurye DTO ile başarıyla oluşturuldu.");
        }

        [HttpPut("{id}/location")]
        public async Task<IActionResult> UpdateLocation(int id, [FromQuery] double lat, [FromQuery] double lon)
        {
            await _service.UpdateLocationAsync(id, lat, lon);
            return Ok(new { Message = "Konum başarıyla hem PostgreSQL hem de Redis'e kaydedildi." });
        }
    }
}
