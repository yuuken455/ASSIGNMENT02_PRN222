

namespace DAL.IRepositories
{
    public interface IVersionRepo
    {
        Task<List<DALVersion>> GetAllAsync(CancellationToken ct = default);
        Task<DALVersion?> GetByIdAsync(int id, CancellationToken ct = default);
        Task<DALVersion> AddAsync(DALVersion entity, CancellationToken ct = default);
        Task UpdateAsync(DALVersion entity, CancellationToken ct = default);
        Task DeleteAsync(int id, CancellationToken ct = default);

        Task<bool> ExistsByNameAsync(int modelId, string versionName, int? excludeId = null, CancellationToken ct = default);
    }
}
