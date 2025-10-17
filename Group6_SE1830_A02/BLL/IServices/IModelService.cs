using BLL.DTOs;

namespace BLL.IServices
{
    public interface IModelService
    {
        Task<List<ModelDto>> GetAllAsync();
        Task<ModelDto?> GetByIdAsync(int id);
        Task<ModelDto> CreateAsync(ModelDto dto);
        Task UpdateAsync(ModelDto dto);
        Task DeleteAsync(int id);
    }
}
