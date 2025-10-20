using BLL.DTOs;
using BLL.IServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EVDManagement.Pages.Appointments
{
    public class EditModel : PageModel
    {
        private readonly ITestDriveAppointmentService _svc;
        private readonly IVersionService _versionSvc;
        private readonly IColorService _colorSvc;
        private readonly ICustomerService _customerSvc;
        private readonly IModelService _modelSvc;

        public EditModel(
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

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var dto = await _svc.GetByIdAsync(id);
            if (dto == null) return RedirectToPage("/Appointments/Index");

            Item = dto;
            await LoadDropdownsAsync();

            if (Item.ModelId == null)
            {
                var ver = (await _versionSvc.GetAllAsync()).FirstOrDefault(v => v.VersionId == Item.CarVersionId);
                if (ver != null) Item.ModelId = ver.ModelId;
            }

            return Page();
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
                await _svc.UpdateAsync(Item);
                TempData["Success"] = "Cập nhật lịch hẹn thành công!";
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
