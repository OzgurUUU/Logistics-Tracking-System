using Microsoft.AspNetCore.Mvc;
using Models.Entities;
using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CourierController : ControllerBase
    {
        private readonly ICourierService _service;

        public CourierController(ICourierService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var couriers = await _service.GetAllCouriersAsync();
            return Ok(couriers);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Courier courier)
        {
            await _service.CreateCourierAsync(courier);
            return Ok("Kurye başarıyla oluşturuldu.");
        }
    }
}
