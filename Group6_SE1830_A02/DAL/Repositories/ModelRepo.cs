using DAL.IRepositories;

namespace DAL.Repositories
{
    public class ModelRepo : IModelRepo
    {
        private readonly DBContext _context;

        public ModelRepo(DBContext context)
        {
            _context = context;
        }
    }
}
