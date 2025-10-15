using DAL.IRepositories;

namespace DAL.Repositories
{
    public class CustomerRepo : ICustomerRepo
    {
        private readonly DBContext _context;

        public CustomerRepo(DBContext context)
        {
            _context = context;
        }
    }
}
