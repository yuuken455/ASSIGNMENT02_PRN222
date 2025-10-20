using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BLL.IServices;
using BLL.DTOs;

namespace EVDManagement.Pages.Appointments
{
    public class DetailsModel : PageModel
    {
        private readonly ITestDriveAppointmentService _svc;
        public DetailsModel(ITestDriveAppointmentService svc) => _svc = svc;

        public TestDriveAppointmentDto? Item { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Item = await _svc.GetByIdAsync(id);
            if (Item == null) return RedirectToPage("/Appointments/Index");
            return Page();
        }
    }
}
