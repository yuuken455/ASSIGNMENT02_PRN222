using DAL.Entities;
namespace DAL.IRepositories
{
    public interface IModelRepo
    {
        Task<List<Model>> GetAllAsync(CancellationToken ct = default);
        Task<Model?> GetByIdAsync(int id, CancellationToken ct = default);
        Task<Model> AddAsync(Model entity, CancellationToken ct = default);
        Task UpdateAsync(Model entity, CancellationToken ct = default);
        Task DeleteAsync(int id, CancellationToken ct = default);
        Task<bool> ExistsByNameAsync(string name, int? excludeId = null, CancellationToken ct = default);
    }
}
