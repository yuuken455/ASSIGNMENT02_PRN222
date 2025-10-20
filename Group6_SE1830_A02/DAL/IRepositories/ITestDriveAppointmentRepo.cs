using DAL.Entities;

namespace DAL.IRepository
{
    public interface ITestDriveAppointmentRepo
    {
        Task<ICollection<TestDriveAppointment>> GetAllAppointments();
        Task<ICollection<TestDriveAppointment>> GetAppointmentsInDay(DateTime date);
        Task<ICollection<TestDriveAppointment>> GetScheduledAppointmentsInDay(DateTime date);
        Task DeleteTestDriveAppointments(ICollection<TestDriveAppointment> testDriveAppointments);

        // CRUD
        Task AddAsync(TestDriveAppointment entity);
        void Update(TestDriveAppointment entity);
        Task SaveAsync();

        // Kiểm tra chồng chéo lịch
        Task<bool> ExistsOverlapAsync(
            int carVersionId,
            int colorId,
            DateTime startLocal,
            int durationMinutes = 60,
            int bufferMinutes = 15,
            int? excludeAppointmentId = null);
    }
}
