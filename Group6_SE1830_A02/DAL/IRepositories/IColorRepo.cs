using DAL.Entities;

namespace DAL.IRepositories
{
    public interface IColorRepo
    {
        Task<List<DALColor>> GetAllAsync(CancellationToken ct = default);
        Task<DALColor?> GetByIdAsync(int id, CancellationToken ct = default);
        Task<DALColor> AddAsync(DALColor entity, CancellationToken ct = default);
        Task UpdateAsync(DALColor entity, CancellationToken ct = default);
        Task DeleteAsync(int id, CancellationToken ct = default);

        Task<bool> ExistsByNameAsync(int versionId, string colorName, int? excludeId = null, CancellationToken ct = default);
    }
}
