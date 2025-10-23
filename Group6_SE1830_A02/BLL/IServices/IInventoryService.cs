using BLL.DTOs;

namespace BLL.IServices
{
    public interface IInventoryService
    {
        Task<List<InventoryDto>> GetAllAsync(CancellationToken ct = default);
        Task<InventoryDto?> GetByIdAsync(int id, CancellationToken ct = default);
        Task<InventoryDto> CreateAsync(InventoryDto dto, CancellationToken ct = default);
        Task UpdateAsync(InventoryDto dto, CancellationToken ct = default);
        Task DeleteAsync(int id, CancellationToken ct = default);

        Task<InventoryDto?> GetByVersionColorAsync(int versionId, int colorId, CancellationToken ct = default);

        // NEW
        Task DeleteByColorAsync(int colorId, CancellationToken ct = default);
    }
}
