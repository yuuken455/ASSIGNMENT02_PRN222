using DAL.IRepositories;

namespace DAL.Repositories
{
    public class OrderRepo : IOrderRepo
    {
        private readonly DBContext _context;

        public OrderRepo(DBContext context)
        {
            _context = context;
        }
    }
}
