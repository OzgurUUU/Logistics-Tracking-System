using Microsoft.AspNetCore.SignalR;
using Services;
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
                try
                {
                    using (var scope = _serviceProvider.CreateScope())
                    {
                        var courierService = scope.ServiceProvider.GetRequiredService<ICourierService>();

                        // 1. Veritabanındaki tüm kuryeleri al
                        var couriers = await courierService.GetAllCouriersAsync();

                        foreach (var courier in couriers)
                        {
                            // 2. Eğer kurye sözlükte yoksa başlangıç konumunu DB'den al
                            if (!_courierPositions.ContainsKey(courier.Id))
                            {
                                _courierPositions.TryAdd(courier.Id, (courier.LastLatitude, courier.LastLongitude));
                            }

                            // 3. Mevcut konumu al ve küçük bir hareket ekle
                            var currentPos = _courierPositions[courier.Id];
                            double newLat = currentPos.Lat + (random.NextDouble() - 0.5) * 0.0015;
                            double newLon = currentPos.Lon + (random.NextDouble() - 0.5) * 0.0015;

                            // 4. Yeni konumu sözlükte güncelle
                            _courierPositions[courier.Id] = (newLat, newLon);

                            // 5. Servis üzerinden DB ve Redis'i güncelle
                            await courierService.UpdateLocationAsync(courier.Id, newLat, newLon);

                            // 6. SignalR ile canlı yayın yap
                            await _hubContext.Clients.All.SendAsync("ReceiveLocation", courier.Id, newLat, newLon);
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError($"[Simülatör Hatası]: {ex.Message}");
                }

                // Tüm kuryeler güncellendikten sonra 3 saniye bekle
                await Task.Delay(3000, stoppingToken);
            }
        }
    }
}
