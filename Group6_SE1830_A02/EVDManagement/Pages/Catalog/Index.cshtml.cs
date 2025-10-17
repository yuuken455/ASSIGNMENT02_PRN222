using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;
using EVDManagement.SignalR;
using BLL.IServices;
using BLL.DTOs;

namespace EVDManagement.Pages.Catalog
{
    public class IndexModel : PageModel
    {
        private readonly IModelService _modelSvc;
        private readonly IVersionService _versionSvc;
        private readonly IColorService _colorSvc;
        private readonly IHubContext<ModelHub> _hub;

        public IndexModel(IModelService modelSvc, IVersionService versionSvc, IColorService colorSvc, IHubContext<ModelHub> hub)
        {
            _modelSvc = modelSvc;
            _versionSvc = versionSvc;
            _colorSvc = colorSvc;
            _hub = hub;
        }

        public List<ModelDto> Models { get; set; } = new();
        public List<VersionDto> Versions { get; set; } = new();
        public List<ColorDto> Colors { get; set; } = new();

        // ---- Cars input (create) ----
        public class CarInputDto
        {
            public int? ExistingModelId { get; set; }
            public string? NewModelName { get; set; }
            public string? NewModelSegment { get; set; }
            public string? NewModelDescription { get; set; }

            public int? ExistingVersionId { get; set; }
            public string? NewVersionName { get; set; }
            public decimal? NewBatteryCapacity { get; set; }
            public int? NewRangeKm { get; set; }
            public int? NewSeat { get; set; }
            public decimal? NewBasePrice { get; set; }

            public int? ExistingColorId { get; set; }
            public string? NewColorName { get; set; }
            public string? NewHexCode { get; set; }
            public decimal? NewExtraCost { get; set; }
        }
        [BindProperty] public CarInputDto CarInput { get; set; } = new();

        // ---- Cars edit modal ----
        public class EditCarDto
        {
            public int ModelId { get; set; }
            public int VersionId { get; set; }
            public int ColorId { get; set; }

            public string ModelName { get; set; } = null!;
            public string? Segment { get; set; }
            public string? Description { get; set; }

            public string VersionName { get; set; } = null!;
            public decimal? BatteryCapacity { get; set; }
            public int? RangeKm { get; set; }
            public int? Seat { get; set; }
            public decimal BasePrice { get; set; }

            public string? ColorName { get; set; }
            public string? HexCode { get; set; }
            public decimal? ExtraCost { get; set; }
        }
        [BindProperty] public EditCarDto EditCar { get; set; } = new();

        // ---- Models tab ----
        [BindProperty] public ModelDto NewModel { get; set; } = new();
        [BindProperty] public ModelDto EditModel { get; set; } = new();

        // ---- Versions tab ----
        [BindProperty] public VersionDto NewVersion { get; set; } = new();
        [BindProperty] public VersionDto EditVersion { get; set; } = new();

        // ---- Colors tab ----
        [BindProperty] public ColorDto NewColor { get; set; } = new();
        [BindProperty] public ColorDto EditColor { get; set; } = new();

        public async Task OnGetAsync(string? activeTab = "cars")
        {
            ViewData["ActiveTab"] = activeTab ?? "cars";
            await LoadAllAsync();
        }

        private async Task LoadAllAsync()
        {
            Models = await _modelSvc.GetAllAsync();
            Versions = await _versionSvc.GetAllAsync();
            Colors = await _colorSvc.GetAllAsync();
        }

        // ========== Cars ==========
        public async Task<IActionResult> OnPostSaveCarAsync()
        {
            // ---- VALIDATION KẾT HỢP ----
            // Model: phải chọn ExistingModelId hoặc nhập NewModelName
            if (!CarInput.ExistingModelId.HasValue && string.IsNullOrWhiteSpace(CarInput.NewModelName))
                ModelState.AddModelError(string.Empty, "Please select a model or enter a new model name.");

            // Version: phải chọn ExistingVersionId hoặc nhập NewVersionName
            if (!CarInput.ExistingVersionId.HasValue && string.IsNullOrWhiteSpace(CarInput.NewVersionName))
                ModelState.AddModelError(string.Empty, "Please select a version or enter a new version name.");

            // Nếu tạo Version mới mà BasePrice rỗng -> bắt buộc
            if (!CarInput.ExistingVersionId.HasValue && !CarInput.NewBasePrice.HasValue)
                ModelState.AddModelError(nameof(CarInput.NewBasePrice), "Base price is required for a new version.");

            // Color: phải chọn ExistingColorId hoặc nhập NewColorName
            if (!CarInput.ExistingColorId.HasValue && string.IsNullOrWhiteSpace(CarInput.NewColorName))
                ModelState.AddModelError(string.Empty, "Please select a color or enter a new color name.");

            if (!ModelState.IsValid)
            {
                ViewData["ActiveTab"] = "cars";
                await LoadAllAsync();
                return Page();
            }

            // ---- TẠO / GÁN MODEL ----
            int modelId;
            if (CarInput.ExistingModelId.HasValue)
            {
                modelId = CarInput.ExistingModelId.Value;

                // Nếu có nhập Segment/Description mới -> cập nhật nhẹ model đang chọn
                if (!string.IsNullOrWhiteSpace(CarInput.NewModelSegment) || !string.IsNullOrWhiteSpace(CarInput.NewModelDescription))
                {
                    var mm = await _modelSvc.GetByIdAsync(modelId);
                    if (mm != null)
                    {
                        if (!string.IsNullOrWhiteSpace(CarInput.NewModelSegment)) mm.Segment = CarInput.NewModelSegment;
                        if (!string.IsNullOrWhiteSpace(CarInput.NewModelDescription)) mm.Description = CarInput.NewModelDescription;
                        await _modelSvc.UpdateAsync(mm);
                    }
                }
            }
            else
            {
                var created = await _modelSvc.CreateAsync(new ModelDto
                {
                    ModelName = CarInput.NewModelName!.Trim(),
                    Segment = string.IsNullOrWhiteSpace(CarInput.NewModelSegment) ? null : CarInput.NewModelSegment!.Trim(),
                    Description = string.IsNullOrWhiteSpace(CarInput.NewModelDescription) ? null : CarInput.NewModelDescription!.Trim()
                });
                modelId = created.ModelId;
            }

            // ---- TẠO / GÁN VERSION ----
            int versionId;
            if (CarInput.ExistingVersionId.HasValue)
            {
                var v = await _versionSvc.GetByIdAsync(CarInput.ExistingVersionId.Value);
                if (v == null) { TempData["Msg"] = "Version not found."; return RedirectToPage(new { activeTab = "cars" }); }

                // gán sang model vừa chọn/tạo
                v.ModelId = modelId;

                // nếu có override field mới -> cập nhật
                if (!string.IsNullOrWhiteSpace(CarInput.NewVersionName)) v.VersionName = CarInput.NewVersionName!.Trim();
                if (CarInput.NewBatteryCapacity.HasValue) v.BatteryCapacity = CarInput.NewBatteryCapacity;
                if (CarInput.NewRangeKm.HasValue) v.RangeKm = CarInput.NewRangeKm;
                if (CarInput.NewSeat.HasValue) v.Seat = CarInput.NewSeat;
                if (CarInput.NewBasePrice.HasValue) v.BasePrice = CarInput.NewBasePrice.Value;

                await _versionSvc.UpdateAsync(v);
                versionId = v.VersionId;
            }
            else
            {
                var createdV = await _versionSvc.CreateAsync(new VersionDto
                {
                    ModelId = modelId,
                    VersionName = CarInput.NewVersionName!.Trim(),
                    BatteryCapacity = CarInput.NewBatteryCapacity,
                    RangeKm = CarInput.NewRangeKm,
                    Seat = CarInput.NewSeat,
                    BasePrice = CarInput.NewBasePrice!.Value
                });
                versionId = createdV.VersionId;
            }

            // ---- TẠO / GÁN COLOR ----
            if (CarInput.ExistingColorId.HasValue)
            {
                var c = await _colorSvc.GetByIdAsync(CarInput.ExistingColorId.Value);
                if (c != null)
                {
                    c.VersionId = versionId;
                    if (!string.IsNullOrWhiteSpace(CarInput.NewColorName)) c.ColorName = CarInput.NewColorName!.Trim();
                    if (!string.IsNullOrWhiteSpace(CarInput.NewHexCode)) c.HexCode = CarInput.NewHexCode!.Trim();
                    if (CarInput.NewExtraCost.HasValue) c.ExtraCost = CarInput.NewExtraCost;
                    await _colorSvc.UpdateAsync(c);
                }
            }
            else
            {
                await _colorSvc.CreateAsync(new ColorDto
                {
                    VersionId = versionId,
                    ColorName = CarInput.NewColorName!.Trim(),
                    HexCode = string.IsNullOrWhiteSpace(CarInput.NewHexCode) ? null : CarInput.NewHexCode!.Trim(),
                    ExtraCost = CarInput.NewExtraCost ?? 0
                });
            }

            TempData["Msg"] = "Car created successfully!";
            await _hub.Clients.All.SendAsync("CatalogChanged");
            return RedirectToPage(new { activeTab = "cars" });
        }

        public async Task<IActionResult> OnPostUpdateCarFullAsync()
        {
            var m = await _modelSvc.GetByIdAsync(EditCar.ModelId);
            if (m == null) { TempData["Msg"] = "Model not found."; return RedirectToPage(new { activeTab = "cars" }); }
            m.ModelName = EditCar.ModelName; m.Segment = EditCar.Segment; m.Description = EditCar.Description;
            await _modelSvc.UpdateAsync(m);

            var v = await _versionSvc.GetByIdAsync(EditCar.VersionId);
            if (v == null) { TempData["Msg"] = "Version not found."; return RedirectToPage(new { activeTab = "cars" }); }
            v.ModelId = EditCar.ModelId;
            v.VersionName = EditCar.VersionName;
            v.BatteryCapacity = EditCar.BatteryCapacity;
            v.RangeKm = EditCar.RangeKm;
            v.Seat = EditCar.Seat;
            v.BasePrice = EditCar.BasePrice;
            await _versionSvc.UpdateAsync(v);

            var c = await _colorSvc.GetByIdAsync(EditCar.ColorId);
            if (c == null) { TempData["Msg"] = "Color not found."; return RedirectToPage(new { activeTab = "cars" }); }
            c.VersionId = EditCar.VersionId;
            c.ColorName = EditCar.ColorName;
            c.HexCode = EditCar.HexCode;
            c.ExtraCost = EditCar.ExtraCost;
            await _colorSvc.UpdateAsync(c);

            TempData["Msg"] = "Car updated successfully.";
            await _hub.Clients.All.SendAsync("CatalogChanged");
            return RedirectToPage(new { activeTab = "cars" });
        }

        public async Task<IActionResult> OnPostDeleteCarAsync(int colorId)
        {
            await _colorSvc.DeleteAsync(colorId);
            TempData["Msg"] = "Car deleted.";
            await _hub.Clients.All.SendAsync("CatalogChanged");
            return RedirectToPage(new { activeTab = "cars" });
        }

        // ========== Models ==========
        public async Task<IActionResult> OnPostCreateModelAsync()
        {
            if (string.IsNullOrWhiteSpace(NewModel.ModelName))
            {
                TempData["Msg"] = "Model name is required.";
                return RedirectToPage(new { activeTab = "models" });
            }
            await _modelSvc.CreateAsync(NewModel);
            TempData["Msg"] = "Model created.";
            await _hub.Clients.All.SendAsync("CatalogChanged");
            return RedirectToPage(new { activeTab = "models" });
        }

        public async Task<IActionResult> OnPostLoadModelAsync(int id)
        {
            var m = await _modelSvc.GetByIdAsync(id);
            if (m != null) EditModel = m;
            return RedirectToPage(new { activeTab = "models" });
        }

        public async Task<IActionResult> OnPostUpdateModelAsync()
        {
            await _modelSvc.UpdateAsync(EditModel);
            TempData["Msg"] = "Model updated.";
            await _hub.Clients.All.SendAsync("CatalogChanged");
            return RedirectToPage(new { activeTab = "models" });
        }

        public async Task<IActionResult> OnPostDeleteModelAsync(int id)
        {
            await _modelSvc.DeleteAsync(id);
            TempData["Msg"] = "Model deleted.";
            await _hub.Clients.All.SendAsync("CatalogChanged");
            return RedirectToPage(new { activeTab = "models" });
        }

        // ========== Versions ==========
        public async Task<IActionResult> OnPostCreateVersionAsync()
        {
            await _versionSvc.CreateAsync(NewVersion);
            TempData["Msg"] = "Version created.";
            await _hub.Clients.All.SendAsync("CatalogChanged");
            return RedirectToPage(new { activeTab = "versions" });
        }

        public async Task<IActionResult> OnPostLoadVersionAsync(int id)
        {
            var v = await _versionSvc.GetByIdAsync(id);
            if (v != null) EditVersion = v;
            return RedirectToPage(new { activeTab = "versions" });
        }

        public async Task<IActionResult> OnPostUpdateVersionAsync()
        {
            await _versionSvc.UpdateAsync(EditVersion);
            TempData["Msg"] = "Version updated.";
            await _hub.Clients.All.SendAsync("CatalogChanged");
            return RedirectToPage(new { activeTab = "versions" });
        }

        public async Task<IActionResult> OnPostDeleteVersionAsync(int id)
        {
            await _versionSvc.DeleteAsync(id);
            TempData["Msg"] = "Version deleted.";
            await _hub.Clients.All.SendAsync("CatalogChanged");
            return RedirectToPage(new { activeTab = "versions" });
        }

        // ========== Colors ==========
        public async Task<IActionResult> OnPostCreateColorAsync()
        {
            await _colorSvc.CreateAsync(NewColor);
            TempData["Msg"] = "Color created.";
            await _hub.Clients.All.SendAsync("CatalogChanged");
            return RedirectToPage(new { activeTab = "colors" });
        }

        public async Task<IActionResult> OnPostLoadColorAsync(int id)
        {
            var c = await _colorSvc.GetByIdAsync(id);
            if (c != null) EditColor = c;
            return RedirectToPage(new { activeTab = "colors" });
        }

        public async Task<IActionResult> OnPostUpdateColorAsync()
        {
            await _colorSvc.UpdateAsync(EditColor);
            TempData["Msg"] = "Color updated.";
            await _hub.Clients.All.SendAsync("CatalogChanged");
            return RedirectToPage(new { activeTab = "colors" });
        }

        public async Task<IActionResult> OnPostDeleteColorAsync(int id)
        {
            await _colorSvc.DeleteAsync(id);
            TempData["Msg"] = "Color deleted.";
            await _hub.Clients.All.SendAsync("CatalogChanged");
            return RedirectToPage(new { activeTab = "colors" });
        }
    }
}
