using Models.DTOs;
using Models.Entities;
using Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class CourierService : ICourierService
    {
        private readonly ICourierRepository _repository;
        private readonly ICourierCacheRepository _cacheRepository; // Redis repository

        // Constructor'a Cache Repository'yi de ekledik
        public CourierService(ICourierRepository repository, ICourierCacheRepository cacheRepository)
        {
            _repository = repository;
            _cacheRepository = cacheRepository;
        }

        public async Task<IEnumerable<Courier>> GetAllCouriersAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task CreateCourierAsync(CourierCreateDto courierDto)
        {
            var courier = new Courier
            {
                Name = courierDto.Name,
                VehicleType = courierDto.VehicleType,
                LastLatitude = courierDto.LastLatitude,
                LastLongitude = courierDto.LastLongitude,
                IsActive = true
            };

            await _repository.AddAsync(courier);
        }
        public async Task DeleteCourierAsync(int id)
        {
            // 1. Kalıcı veritabanından sil
            await _repository.DeleteAsync(id);

            // 2. Anlık haritadan (Redis) sil
            await _cacheRepository.RemoveLocationAsync(id);
        }

        // Sorun çıkaran ve düzelttiğimiz metod:
        public async Task UpdateLocationAsync(int courierId, double lat, double lon)
        {
            // 1. Kuryeyi DB'den bul
            var courier = await _repository.GetByIdAsync(courierId);

            // Eğer kurye yoksa hata fırlat (500 yerine anlamlı bir hata)
            if (courier == null)
                throw new Exception($"Kurye bulunamadı! ID: {courierId}");

            // 2. DB için konum güncelle
            courier.LastLatitude = lat;
            courier.LastLongitude = lon;
            await _repository.UpdateAsync(courier);

            // 3. Redis için konum güncelle (Çift Yazma / Dual Write)
            await _cacheRepository.UpdateLocationAsync(courierId, lat, lon);
        }
    }
}
