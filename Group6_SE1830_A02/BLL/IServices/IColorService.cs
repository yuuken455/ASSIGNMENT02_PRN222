using BLL.DTOs;

namespace BLL.IServices
{
    public interface IColorService
    {
        Task<List<ColorDto>> GetAllAsync();
        Task<ColorDto?> GetByIdAsync(int id);
        Task<ColorDto> CreateAsync(ColorDto dto);
        Task UpdateAsync(ColorDto dto);
        Task DeleteAsync(int id);
    }
}
