using System.Threading.Tasks;
using BLL.DTOs;

namespace BLL.IServices
{
    public interface IRealtimeNotifier
    {
        Task AppointmentCreated(TestDriveAppointmentDTO dto);
        Task AppointmentUpdated(TestDriveAppointmentDTO dto);
        Task AppointmentDeleted(int appointmentId);
    }
}
