using DAL.Entities;

namespace DAL.IRepository
{
    public interface ITestDriveAppointmentRepo
    {
        // READ
        Task<ICollection<TestDriveAppointment>> GetAllAppointments();
        Task<ICollection<TestDriveAppointment>> GetAppointmentsInDay(DateTime date);
        Task<ICollection<TestDriveAppointment>> GetScheduledAppointmentsInDay(DateTime date);
        Task<TestDriveAppointment?> GetByIdAsync(int id); 

        // WRITE
        Task AddAsync(TestDriveAppointment entity);
        void Update(TestDriveAppointment entity);
        Task DeleteTestDriveAppointments(ICollection<TestDriveAppointment> testDriveAppointments);
        Task SaveAsync();

        // OVERLAP theo xe (đã có)
        Task<bool> ExistsOverlapAsync(
            int carVersionId,
            int colorId,
            DateTime startLocal,
            int durationMinutes = 60,
            int bufferMinutes = 15,
            int? excludeAppointmentId = null);

        // ✅ THÊM: OVERLAP theo KHÁCH HÀNG
        Task<bool> ExistsCustomerOverlapAsync(
            int customerId,
            DateTime startLocal,
            int durationMinutes = 60,
            int bufferMinutes = 0,
            int? excludeAppointmentId = null);
    }
}
