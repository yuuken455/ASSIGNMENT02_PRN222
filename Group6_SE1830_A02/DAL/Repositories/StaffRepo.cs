using DAL.Entities;
using DAL.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public class StaffRepo : IStaffRepo
    {
        private readonly DBContext _context;

        public StaffRepo(DBContext context)
        {
            _context = context;
        }

        public async Task<Staff?> GetStaffByEmailAsync(string email)
        {
            return await _context.Staffs.FirstOrDefaultAsync(s => s.Email == email);    
        }
    }
}
