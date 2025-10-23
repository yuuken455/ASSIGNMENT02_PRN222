using BLL.DTOs;

namespace BLL.IServices
{
    public interface ITestDriveAppointmentService
    {
        // READ
        Task<List<TestDriveAppointmentDTO>> GetAllAsync();
        Task<List<TestDriveAppointmentDTO>> GetByDayAsync(DateTime date);
        Task<TestDriveAppointmentDTO?> GetByIdAsync(int id);

        // WRITE
        Task<TestDriveAppointmentDTO> CreateAsync(TestDriveAppointmentDTO dto);
        Task UpdateAsync(TestDriveAppointmentDTO dto);
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
