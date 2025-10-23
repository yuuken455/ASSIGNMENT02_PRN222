using BLL.DTOs;

namespace BLL.IServices
{
    public interface IStaffService
    {
        Task<StaffDto?> Login(LoginDto loginDto);
    }
}
