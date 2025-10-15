using DAL.IRepositories;

namespace DAL.Repositories
{
    public class VersionRepo : IVersionRepo
    {
        private readonly DBContext _context;

        public VersionRepo(DBContext context)
        {
            _context = context;
        }
    }
}
