using System.Threading.Tasks;
using BLL.DTOs;

namespace BLL.IServices
{
    public interface IRealtimeNotifier
    {
        Task AppointmentCreated(TestDriveAppointmentDto dto);
        Task AppointmentUpdated(TestDriveAppointmentDto dto);
        Task AppointmentDeleted(int appointmentId);
    }
}
