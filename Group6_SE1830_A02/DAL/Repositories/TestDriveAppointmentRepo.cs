using DAL.Entities;
using DAL.IRepository;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repository
{
    public class TestDriveAppointmentRepo : ITestDriveAppointmentRepo
    {
        private readonly DBContext _context;
        public TestDriveAppointmentRepo(DBContext context)
        {
            _context = context;
        }

        public async Task<ICollection<TestDriveAppointment>> GetAllAppointments()
        {
            return await _context.TestDriveAppointments
                .Include(t => t.Customer)
                .Include(t => t.CarVersion)
                    .ThenInclude(v => v.Model)
                .Include(t => t.Color)
                .ToListAsync();
        }

        public async Task<ICollection<TestDriveAppointment>> GetAppointmentsInDay(DateTime date)
        {
            var day = date.Date;
            return await _context.TestDriveAppointments
                .Where(t => t.DateTime.Date == day)
                .Include(t => t.Customer)
                .Include(t => t.CarVersion)
                    .ThenInclude(v => v.Model)
                .Include(t => t.Color)
                .ToListAsync();
        }

        public async Task<ICollection<TestDriveAppointment>> GetScheduledAppointmentsInDay(DateTime date)
        {
            var day = date.Date;
            return await _context.TestDriveAppointments
                .Where(t => t.DateTime.Date == day && t.Status == "Scheduled")
                .Include(t => t.Customer)
                .Include(t => t.CarVersion)
                    .ThenInclude(v => v.Model)
                .Include(t => t.Color)
                .ToListAsync();
        }

        public async Task AddAsync(TestDriveAppointment entity)
        {
            await _context.TestDriveAppointments.AddAsync(entity);
        }

        public void Update(TestDriveAppointment entity)
        {
            _context.TestDriveAppointments.Update(entity);
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task DeleteTestDriveAppointments(ICollection<TestDriveAppointment> testDriveAppointments)
        {
            _context.TestDriveAppointments.RemoveRange(testDriveAppointments);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExistsOverlapAsync(
            int carVersionId,
            int colorId,
            DateTime startLocal,
            int durationMinutes = 60,
            int bufferMinutes = 15,
            int? excludeAppointmentId = null)
        {
            var endLocal = startLocal.AddMinutes(durationMinutes);

            // Tính khoảng thời gian cần check (cộng thêm buffer 15’ trước/sau)
            var checkStart = startLocal.AddMinutes(-bufferMinutes);
            var checkEnd = endLocal.AddMinutes(bufferMinutes);

            return await _context.TestDriveAppointments.AnyAsync(a =>
                a.CarVersionId == carVersionId &&
                a.ColorId == colorId &&
                (excludeAppointmentId == null || a.AppointmentId != excludeAppointmentId) &&
                a.DateTime < checkEnd &&
                a.DateTime.AddMinutes(durationMinutes) > checkStart);
        }
    }
}
