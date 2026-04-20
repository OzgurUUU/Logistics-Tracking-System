using Models.Entities;

namespace Repository.Interfaces
{
    public interface ICourierRepository
    {
        Task<IEnumerable<Courier>> GetAllAsync();
        Task AddAsync(Courier courier);

        Task DeleteAsync(int id);
        Task<Courier?> GetByIdAsync(int id);
        Task UpdateAsync(Courier courier);
    }
}
