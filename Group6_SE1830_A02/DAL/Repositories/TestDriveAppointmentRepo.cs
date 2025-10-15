using DAL.IRepositories;

namespace DAL.Repositories
{
    public class TestDriveAppointmentRepo : ITestDriveAppointmentRepo
    {
        private readonly DBContext _context;

        public TestDriveAppointmentRepo(DBContext context)
        {
            _context = context;
        }
    }
}
