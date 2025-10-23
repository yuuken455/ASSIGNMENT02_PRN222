using Microsoft.AspNetCore.Mvc.RazorPages;
using BLL.IServices;
using BLL.DTOs;

namespace EVDManagement.Pages.Appointments
{
    public class IndexModel : PageModel
    {
        private readonly ITestDriveAppointmentService _svc;
        public IndexModel(ITestDriveAppointmentService svc) => _svc = svc;

        public List<TestDriveAppointmentDTO> Items { get; set; } = new();

        public DateTime? FilterDate { get; set; }

        public async Task OnGetAsync(DateTime? date)
        {
            FilterDate = date;
            Items = date.HasValue
                ? await _svc.GetByDayAsync(date.Value)
                : await _svc.GetAllAsync();
        }
    }
}
