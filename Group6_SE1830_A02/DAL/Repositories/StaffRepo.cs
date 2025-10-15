using DAL.IRepositories;

namespace DAL.Repositories
{
    public class StaffRepo : IStaffRepo
    {
        private readonly DBContext _context;

        public StaffRepo(DBContext context)
        {
            _context = context;
        }
    }
}
