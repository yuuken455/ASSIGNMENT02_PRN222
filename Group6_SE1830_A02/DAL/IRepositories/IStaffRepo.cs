using DAL.Entities;

namespace DAL.IRepositories
{
    public interface IStaffRepo
    {
        Task<Staff?> GetStaffByEmailAsync(string email);
    }
}
