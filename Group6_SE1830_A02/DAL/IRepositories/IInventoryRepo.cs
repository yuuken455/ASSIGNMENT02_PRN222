using DAL.Entities;

namespace DAL.IRepositories
{
    public interface IInventoryRepo
    {
        Task<List<Inventory>> GetAllAsync(CancellationToken ct = default);
        Task<Inventory?> GetByIdAsync(int id, CancellationToken ct = default);
        Task<Inventory> AddAsync(Inventory entity, CancellationToken ct = default);
        Task UpdateAsync(Inventory entity, CancellationToken ct = default);
        Task DeleteAsync(int id, CancellationToken ct = default);

        // tiện cho UI: tìm một dòng theo Version+Color
        Task<Inventory?> GetByVersionColorAsync(int versionId, int colorId, CancellationToken ct = default);
    }
}
