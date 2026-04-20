using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Models.Entities;
using Repository;
using Services.Interfaces;
using System.Collections.Concurrent;

namespace CourierTrackingAPI.Workers
{
    public class CourierSimulatorWorker : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<CourierSimulatorWorker> _logger;
        private readonly IHubContext<CourierHub> _hubContext;

        // Kuryelerin anlık konumlarını hafızada tutmak için (Thread-safe sözlük)
        private readonly ConcurrentDictionary<int, (double Lat, double Lon)> _courierPositions = new();

        public CourierSimulatorWorker(
            IServiceProvider serviceProvider,
            ILogger<CourierSimulatorWorker> logger,
            IHubContext<CourierHub> hubContext)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
            _hubContext = hubContext;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("🚀 Dinamik Kurye Simülatörü Başlatıldı.");
            var random = new Random();

            while (!stoppingToken.IsCancellationRequested)
            {
                double stepSize = 0.002;
                try
                {
                    using (var scope = _serviceProvider.CreateScope())
                    {

                        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                        // 1. Veritabanındaki tüm kuryeleri al
                        var couriers = await context.Couriers.ToListAsync();

                        foreach (var courier in couriers)
                        {
                            if (courier.IsAvailable || courier.ActiveOrderId == null)
                            {
                                // KURYE BOŞTA - RASTGELE GEZ
                                courier.LastLatitude += (random.NextDouble() - 0.5) * 0.003;
                                courier.LastLongitude += (random.NextDouble() - 0.5) * 0.003;

                                await _hubContext.Clients.All.SendAsync("ReceiveLocation", courier.Id, courier.LastLatitude, courier.LastLongitude, courier.VehicleType);
                            }
                            else
                            {
                                // KURYE SİPARİŞTE - HEDEFE GİT
                                var order = await context.Orders.FindAsync(courier.ActiveOrderId);
                                if (order != null && order.Status == OrderStatus.Assigned)
                                {
                                    var targetLat = order.DeliveryLatitude;
                                    var targetLon = order.DeliveryLongitude;

                                    double dLat = targetLat - courier.LastLatitude;
                                    double dLon = targetLon - courier.LastLongitude;

                                    double distance = Math.Sqrt(dLat * dLat + dLon * dLon);

                                    if (distance > stepSize)
                                    {
                                        // Adım at
                                        courier.LastLatitude += (dLat / distance) * stepSize;
                                        courier.LastLongitude += (dLon / distance) * stepSize;
                                    }
                                    else
                                    {
                                        // Teslimatı yap
                                        courier.LastLatitude = targetLat;
                                        courier.LastLongitude = targetLon;

                                        courier.IsAvailable = true;
                                        courier.ActiveOrderId = null;
                                        order.Status = OrderStatus.Delivered;
                                        await _hubContext.Clients.All.SendAsync("OrderDelivered", order.Id);
                                        _logger.LogInformation($"✅ Kurye {courier.Id}, {order.Id} nolu siparişi teslim etti!");
                                    }

                                    // Yeni konumu yayınla
                                    await _hubContext.Clients.All.SendAsync("ReceiveLocation", courier.Id, courier.LastLatitude, courier.LastLongitude, courier.VehicleType);
                                }
                            }
                        }
                        await context.SaveChangesAsync();
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError($"[Simülatör Hatası]: {ex.Message}");
                }

                await Task.Delay(3000, stoppingToken);
            }
        }
    }
}
