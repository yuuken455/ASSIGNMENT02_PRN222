using BLL.DTOs;

namespace BLL.IServices
{
    public interface IVersionService
    {
        Task<List<VersionDto>> GetAllAsync();
        Task<VersionDto?> GetByIdAsync(int id);
        Task<VersionDto> CreateAsync(VersionDto dto);
        Task UpdateAsync(VersionDto dto);
        Task DeleteAsync(int id);
    }
}
