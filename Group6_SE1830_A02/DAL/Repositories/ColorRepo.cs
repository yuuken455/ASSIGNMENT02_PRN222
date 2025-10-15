using DAL.IRepositories;

namespace DAL.Repositories
{
    public class ColorRepo : IColorRepo
    {
        private readonly DBContext _context;

        public ColorRepo(DBContext context) 
        {
            _context = context;
        }
    }
}
