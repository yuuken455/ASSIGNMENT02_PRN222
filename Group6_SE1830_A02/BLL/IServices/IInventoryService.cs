using BLL.DTOs;

namespace BLL.IServices
{
    public interface IInventoryService
    {
        Task<List<InventoryDto>> GetAllAsync();
        Task<InventoryDto?> GetByIdAsync(int id);
        Task<InventoryDto> CreateAsync(InventoryDto dto);
        Task UpdateAsync(InventoryDto dto);
        Task DeleteAsync(int id);

        Task<InventoryDto?> GetByVersionColorAsync(int versionId, int colorId);
    }
}
