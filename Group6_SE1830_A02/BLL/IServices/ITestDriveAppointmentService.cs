using BLL.DTOs;

namespace BLL.IServices
{
    public interface ITestDriveAppointmentService
    {
        // READ
        Task<List<TestDriveAppointmentDto>> GetAllAsync();
        Task<List<TestDriveAppointmentDto>> GetByDayAsync(DateTime date);
        Task<TestDriveAppointmentDto?> GetByIdAsync(int id);

        // WRITE
        Task<TestDriveAppointmentDto> CreateAsync(TestDriveAppointmentDto dto);
        Task UpdateAsync(TestDriveAppointmentDto dto);
        Task DeleteAsync(int id);

        // UTIL: kiểm tra slot rảnh (áp dụng rule duration + buffer trong service/repo)
        Task<bool> IsSlotAvailableAsync(
            int carVersionId,
            int colorId,
            DateTime startLocal,
            int durationMinutes = 60,
            int bufferMinutes = 15,
            int? excludeAppointmentId = null);
    }
}
