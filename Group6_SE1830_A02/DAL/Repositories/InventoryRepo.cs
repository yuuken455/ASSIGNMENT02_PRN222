using DAL.IRepositories;

namespace DAL.Repositories
{
    public class InventoryRepo : IInventoryRepo
    {
        private readonly DBContext _context;

        public InventoryRepo(DBContext context)
        {
            _context = context;
        }
    }
}
