using DAL.IRepositories;

namespace DAL.Repositories
{
    public class OrderDetailRepo : IOrderDetailRepo
    {
        private readonly DBContext _context;

        public OrderDetailRepo(DBContext context)
        {
            _context = context;
        }
    }
}
