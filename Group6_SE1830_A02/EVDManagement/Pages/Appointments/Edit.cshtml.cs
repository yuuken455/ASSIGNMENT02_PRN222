using BLL.DTOs;
using BLL.IServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EVDManagement.Pages.Appointments
{
    public class EditModel : PageModel
    {
        private readonly ITestDriveAppointmentService _svc;
        private readonly IInventoryService _inventorySvc;
        private readonly IVersionService _versionSvc;
        private readonly IModelService _modelSvc;
        private readonly IColorService _colorSvc;
        private readonly ICustomerService _customerSvc;

        public EditModel(
            ITestDriveAppointmentService svc,
            IInventoryService inventorySvc,
            IVersionService versionSvc,
            IModelService modelSvc,
            IColorService colorSvc,
            ICustomerService customerSvc)
        {
            _svc = svc;
            _inventorySvc = inventorySvc;
            _versionSvc = versionSvc;
            _modelSvc = modelSvc;
            _colorSvc = colorSvc;
            _customerSvc = customerSvc;
        }

        [BindProperty]
        public TestDriveAppointmentDTO Item { get; set; } = new();

        // Dropdown data
        public IEnumerable<CustomerOption> Customers { get; set; } = Enumerable.Empty<CustomerOption>();
        public IEnumerable<InventoryOption> Inventories { get; set; } = Enumerable.Empty<InventoryOption>();

        // để biết user đang chọn inventory nào (không post vào DTO)
        [BindProperty]
        public int? SelectedInventoryId { get; set; }

        public class CustomerOption
        {
            public int CustomerId { get; set; }
            public string FullName { get; set; } = "";
            public string? Phone { get; set; }
        }

        public class InventoryOption
        {
            public int InventoryId { get; set; }
            public int VersionId { get; set; }
            public int ColorId { get; set; }
            public int ModelId { get; set; }
            public string ModelName { get; set; } = "";
            public string VersionName { get; set; } = "";
            public string ColorName { get; set; } = "";
            public int Quantity { get; set; }

            public string Display => $"{ModelName} • {VersionName} • {ColorName} (x{Quantity})";
        }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var dto = await _svc.GetByIdAsync(id);
            if (dto == null) return RedirectToPage("/Appointments/Index");

            Item = dto;
            await LoadDropdownsAsync();

            // cố gắng preselect inventory theo cặp VersionId-ColorId hiện có
            SelectedInventoryId = Inventories
                .FirstOrDefault(x => x.VersionId == Item.CarVersionId && x.ColorId == Item.ColorId)
                ?.InventoryId;

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            // phải có CarVersionId & ColorId (được set từ JS khi chọn inventory)
            if (SelectedInventoryId is null || Item.CarVersionId == 0 || Item.ColorId == 0)
                ModelState.AddModelError(string.Empty, "Vui lòng chọn xe trong kho.");

            // kiểm tra tồn kho server-side
            var stock = await _inventorySvc.GetByVersionColorAsync(Item.CarVersionId, Item.ColorId);
            if (stock == null || (stock.Quantity ?? 0) <= 0)
                ModelState.AddModelError(string.Empty, "Phiên bản & màu đã hết hàng.");

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
            // Customers: Họ tên (SĐT)
            var allCustomers = await _customerSvc.GetAllCustomersAsync();
            Customers = allCustomers
                .OrderBy(c => c.FullName)
                .Select(c => new CustomerOption
                {
                    CustomerId = c.CustomerId,
                    FullName = c.FullName,
                    Phone = c.Phone
                })
                .ToList();

            // Inventory + join Version/Model/Color; chỉ lấy > 0
            var invs = await _inventorySvc.GetAllAsync();
            var versions = await _versionSvc.GetAllAsync();
            var models = await _modelSvc.GetAllAsync();
            var colors = await _colorSvc.GetAllAsync();

            var vMap = versions.ToDictionary(v => v.VersionId);
            var mMap = models.ToDictionary(m => m.ModelId);
            var cMap = colors.ToDictionary(c => c.ColorId);

            Inventories = invs
                .Where(i => (i.Quantity ?? 0) > 0
                            && vMap.ContainsKey(i.VersionId)
                            && cMap.ContainsKey(i.ColorId)
                            && mMap.ContainsKey(vMap[i.VersionId].ModelId))
                .Select(i =>
                {
                    var v = vMap[i.VersionId];
                    var m = mMap[v.ModelId];
                    var c = cMap[i.ColorId];
                    return new InventoryOption
                    {
                        InventoryId = i.InventoryId,
                        VersionId = v.VersionId,
                        ColorId = c.ColorId,
                        ModelId = m.ModelId,
                        ModelName = m.ModelName,
                        VersionName = v.VersionName,
                        ColorName = c.ColorName,
                        Quantity = i.Quantity ?? 0
                    };
                })
                .OrderBy(x => x.ModelName).ThenBy(x => x.VersionName).ThenBy(x => x.ColorName)
                .ToList();
        }
    }
}
