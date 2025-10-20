using BLL.DTOs;
using BLL.IServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EVDManagement.Pages.Appointments
{
    public class CreateModel : PageModel
    {
        private readonly ITestDriveAppointmentService _svc;
        private readonly IVersionService _versionSvc;
        private readonly IColorService _colorSvc;
        private readonly ICustomerService _customerSvc;
        private readonly IModelService _modelSvc;

        public CreateModel(
            ITestDriveAppointmentService svc,
            IVersionService versionSvc,
            IColorService colorSvc,
            ICustomerService customerSvc,
            IModelService modelSvc)
        {
            _svc = svc;
            _versionSvc = versionSvc;
            _colorSvc = colorSvc;
            _customerSvc = customerSvc;
            _modelSvc = modelSvc;
        }

        [BindProperty]
        public TestDriveAppointmentDto Item { get; set; } = new();

        public IEnumerable<dynamic> Versions { get; set; } = Enumerable.Empty<dynamic>();
        public IEnumerable<dynamic> Colors { get; set; } = Enumerable.Empty<dynamic>();
        public IEnumerable<dynamic> Customers { get; set; } = Enumerable.Empty<dynamic>();
        public IEnumerable<dynamic> Models { get; set; } = Enumerable.Empty<dynamic>();

        public async Task OnGetAsync()
        {
            await LoadDropdownsAsync();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                await LoadDropdownsAsync();
                return Page();
            }

            try
            {
                await _svc.CreateAsync(Item);
                TempData["Success"] = "Tạo lịch hẹn thành công!";
                return RedirectToPage("/Appointments/Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                await LoadDropdownsAsync();
                return Page();
            }
        }

        private async Task LoadDropdownsAsync()
        {
            Versions = (await _versionSvc.GetAllAsync()).Select(v => new { v.VersionId, v.VersionName, v.ModelId });
            Colors = (await _colorSvc.GetAllAsync()).Select(c => new { c.ColorId, c.ColorName });
            Customers = (await _customerSvc.GetAllCustomersAsync()).Select(c => new { c.CustomerId, c.FullName });
            Models = (await _modelSvc.GetAllAsync()).Select(m => new { m.ModelId, m.ModelName });
        }
    }
}
