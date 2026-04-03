using Models.Interfaces;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class RedisCourierRepository : ICourierCacheRepository
    {
        private readonly IDatabase _db;
        private const string RedisKey = "courier_locations";

        public RedisCourierRepository(IConnectionMultiplexer redis)
        {
            _db = redis.GetDatabase();
        }

        public async Task UpdateLocationAsync(int courierId, double lat, double lon)
        {

            await _db.GeoAddAsync(RedisKey, lon, lat, courierId.ToString());
        }
        public async Task RemoveLocationAsync(int courierId)
        {
            // Redis'teki Geo (Koordinat) listesinden bu ID'yi uçuruyoruz
            await _db.GeoRemoveAsync(RedisKey, courierId.ToString());
        }
        public async Task<List<string>> GetNearbyCouriersAsync(double lat, double lon, double radiusKm)
        {

            var results = await _db.GeoRadiusAsync(RedisKey, lon, lat, radiusKm, GeoUnit.Kilometers);
            return results.Select(x => x.Member.ToString()).ToList();
        }
    }
}
