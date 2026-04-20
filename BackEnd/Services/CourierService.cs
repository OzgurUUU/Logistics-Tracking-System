using Models.DTOs;
using Models.Entities;
using Repository.Interfaces;
using Services.Interfaces;


namespace Services
{
    public class CourierService : ICourierService
    {
        private readonly ICourierRepository _repository;
        private readonly ICourierCacheRepository _cacheRepository; // Redis repository

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

            await _repository.DeleteAsync(id);

            await _cacheRepository.RemoveLocationAsync(id);
        }

        public async Task UpdateLocationAsync(int courierId, double lat, double lon)
        {

            var courier = await _repository.GetByIdAsync(courierId);

            if (courier == null)
                throw new Exception($"Kurye bulunamadı! ID: {courierId}");


            courier.LastLatitude = lat;
            courier.LastLongitude = lon;
            await _repository.UpdateAsync(courier);

            await _cacheRepository.UpdateLocationAsync(courierId, lat, lon);
        }
    }
}
