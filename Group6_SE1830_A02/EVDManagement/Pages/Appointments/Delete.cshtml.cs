using BLL.DTOs;
using BLL.IServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EVDManagement.Pages.Appointments
{
    public class DeleteModel : PageModel
    {
        private readonly ITestDriveAppointmentService _svc;

        public DeleteModel(ITestDriveAppointmentService svc)
        {
            _svc = svc;
        }

        [BindProperty]
        public TestDriveAppointmentDto Item { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var dto = await _svc.GetByIdAsync(id);
            if (dto == null)
                return RedirectToPage("/Appointments/Index");

            Item = dto;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            await _svc.DeleteAsync(Item.AppointmentId);
            return RedirectToPage("/Appointments/Index");
        }
    }
}
