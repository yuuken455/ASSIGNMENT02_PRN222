using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BLL.IServices;
using BLL.DTOs;
using EVDManagement.SignalR;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace EVDManagement.Pages.Catalog
{
    public class IndexModel : PageModel
    {
        private readonly IModelService _modelSvc;
        private readonly IVersionService _versionSvc;
        private readonly IColorService _colorSvc;
        private readonly IInventoryService _inventorySvc;
        private readonly IHubContext<ModelHub> _hub;

        public IndexModel(
            IModelService modelSvc,
            IVersionService versionSvc,
            IColorService colorSvc,
            IInventoryService inventorySvc,
            IHubContext<ModelHub> hub)
        {
            _modelSvc = modelSvc;
            _versionSvc = versionSvc;
            _colorSvc = colorSvc;
            _inventorySvc = inventorySvc;
            _hub = hub;
        }

        public List<ModelDto> Models { get; set; } = new();
        public List<VersionDto> Versions { get; set; } = new();
        public List<ColorDto> Colors { get; set; } = new();

        // Map quantity theo (VersionId, ColorId)
        public Dictionary<(int VersionId, int ColorId), int> InventoryMap { get; set; } = new();

        // ===== Create form =====
        public class CreateCarInput
        {
            [BindProperty] public string? ModelName { get; set; }
            [BindProperty] public string? ModelSegment { get; set; }
            [BindProperty] public string? ModelDescription { get; set; }

            [BindProperty] public string? VersionName { get; set; }
            [BindProperty] public decimal? BatteryCapacity { get; set; }
            [BindProperty] public int? RangeKm { get; set; }
            [BindProperty] public int? Seat { get; set; }
            [BindProperty] public decimal? BasePrice { get; set; }

            [BindProperty] public string? ColorName { get; set; }

            [BindProperty] public decimal? ExtraCost { get; set; }
            [BindProperty] public int? Quantity { get; set; }
        }
        [BindProperty] public CreateCarInput CarInput { get; set; } = new();

        // ===== Edit modal DTO =====
        public class EditCarDto
        {
            // IDs
            [BindProperty] public int ModelId { get; set; }
            [BindProperty] public int VersionId { get; set; }
            [BindProperty] public int ColorId { get; set; }

            // Model
            [BindProperty] public string? ModelName { get; set; }    // đổi sang string?
            [BindProperty] public string? Segment { get; set; }
            [BindProperty] public string? Description { get; set; }

            // Version
            [BindProperty] public string? VersionName { get; set; } // đổi sang string?
            [BindProperty] public decimal? BatteryCapacity { get; set; }
            [BindProperty] public int? RangeKm { get; set; }
            [BindProperty] public int? Seat { get; set; }
            [BindProperty] public decimal BasePrice { get; set; }

            // Color
            [BindProperty] public string? ColorName { get; set; }
            [BindProperty] public string? HexCode { get; set; }
            [BindProperty] public decimal? ExtraCost { get; set; }

            // Inventory
            [BindProperty] public int? Quantity { get; set; }
        }
        [BindProperty] public EditCarDto EditCar { get; set; } = new();

        private const decimal MAX_VND = 9_999_000_000_000M;

        public async Task OnGetAsync() => await LoadAllAsync();

        private async Task LoadAllAsync()
        {
            Models = await _modelSvc.GetAllAsync();
            Versions = await _versionSvc.GetAllAsync();
            Colors = await _colorSvc.GetAllAsync();

            InventoryMap.Clear();
            var invs = await _inventorySvc.GetAllAsync();
            foreach (var it in invs)
            {
                var qty = it.Quantity ?? 0;
                var key = (it.VersionId, it.ColorId);
                if (InventoryMap.ContainsKey(key)) InventoryMap[key] += qty;
                else InventoryMap[key] = qty;
            }
        }

        // ===== Upsert helpers =====
        private async Task<ModelDto> GetOrCreateModelByNameAsync(string name, string? seg, string? desc)
        {
            var all = Models.Count == 0 ? await _modelSvc.GetAllAsync() : Models;
            var found = all.FirstOrDefault(m => string.Equals(m.ModelName?.Trim(), name.Trim(), StringComparison.OrdinalIgnoreCase));
            if (found != null)
            {
                var changed = false;
                if (!string.IsNullOrWhiteSpace(seg) && seg != found.Segment) { found.Segment = seg; changed = true; }
                if (!string.IsNullOrWhiteSpace(desc) && desc != found.Description) { found.Description = desc; changed = true; }
                if (changed) await _modelSvc.UpdateAsync(found);
                return found;
            }
            return await _modelSvc.CreateAsync(new ModelDto
            {
                ModelName = name.Trim(),
                Segment = string.IsNullOrWhiteSpace(seg) ? null : seg.Trim(),
                Description = string.IsNullOrWhiteSpace(desc) ? null : desc.Trim()
            });
        }

        private async Task<VersionDto> GetOrCreateVersionByNameAsync(int modelId, string versionName,
            decimal? battery, int? range, int? seat, decimal basePrice)
        {
            var all = Versions.Count == 0 ? await _versionSvc.GetAllAsync() : Versions;
            var found = all.FirstOrDefault(v => v.ModelId == modelId &&
                string.Equals(v.VersionName?.Trim(), versionName.Trim(), StringComparison.OrdinalIgnoreCase));

            if (found != null)
            {
                found.BatteryCapacity = battery;
                found.RangeKm = range;
                found.Seat = seat;
                found.BasePrice = basePrice;
                await _versionSvc.UpdateAsync(found);
                return found;
            }

            return await _versionSvc.CreateAsync(new VersionDto
            {
                ModelId = modelId,
                VersionName = versionName.Trim(),
                BatteryCapacity = battery,
                RangeKm = range,
                Seat = seat,
                BasePrice = basePrice
            });
        }

        private async Task<ColorDto> GetOrCreateColorByNameAsync(int versionId, string colorName, decimal? extra)
        {
            var all = Colors.Count == 0 ? await _colorSvc.GetAllAsync() : Colors;
            var found = all.FirstOrDefault(c => c.VersionId == versionId &&
                string.Equals(c.ColorName?.Trim(), colorName.Trim(), StringComparison.OrdinalIgnoreCase));

            if (found != null)
            {
                found.ExtraCost = extra;
                found.HexCode = null; // bỏ Hex theo yêu cầu
                await _colorSvc.UpdateAsync(found);
                return found;
            }

            return await _colorSvc.CreateAsync(new ColorDto
            {
                VersionId = versionId,
                ColorName = colorName.Trim(),
                HexCode = null,
                ExtraCost = extra ?? 0
            });
        }

        // ===== CREATE =====
        public async Task<IActionResult> OnPostSaveCarAsync()
        {
            if (string.IsNullOrWhiteSpace(CarInput.ModelName))
                ModelState.AddModelError("CarInput.ModelName", "Model name is required.");
            if (string.IsNullOrWhiteSpace(CarInput.VersionName))
                ModelState.AddModelError("CarInput.VersionName", "Version name is required.");
            if (!CarInput.BatteryCapacity.HasValue)
                ModelState.AddModelError("CarInput.BatteryCapacity", "Battery capacity is required.");
            if (!CarInput.RangeKm.HasValue)
                ModelState.AddModelError("CarInput.RangeKm", "Range (km) is required.");
            if (!CarInput.Seat.HasValue)
                ModelState.AddModelError("CarInput.Seat", "Seat is required.");
            if (!CarInput.BasePrice.HasValue)
                ModelState.AddModelError("CarInput.BasePrice", "Base price is required.");
            else if (CarInput.BasePrice < 0 || CarInput.BasePrice > MAX_VND)
                ModelState.AddModelError("CarInput.BasePrice", $"Base price must be between 0 and {MAX_VND:N0}.");
            if (string.IsNullOrWhiteSpace(CarInput.ColorName))
                ModelState.AddModelError("CarInput.ColorName", "Color name is required.");
            if (!CarInput.ExtraCost.HasValue)
                ModelState.AddModelError("CarInput.ExtraCost", "Extra price is required.");
            else if (CarInput.ExtraCost < 0 || CarInput.ExtraCost > MAX_VND)
                ModelState.AddModelError("CarInput.ExtraCost", $"Extra price must be between 0 and {MAX_VND:N0}.");
            if (!CarInput.Quantity.HasValue)
                ModelState.AddModelError("CarInput.Quantity", "Quantity is required.");
            else if (CarInput.Quantity < 0)
                ModelState.AddModelError("CarInput.Quantity", "Quantity cannot be negative.");

            if (!ModelState.IsValid)
            {
                await LoadAllAsync();
                return Page();
            }

            try
            {
                var model = await GetOrCreateModelByNameAsync(
                    CarInput.ModelName!, CarInput.ModelSegment, CarInput.ModelDescription);

                var version = await GetOrCreateVersionByNameAsync(
                    model.ModelId,
                    CarInput.VersionName!,
                    CarInput.BatteryCapacity,
                    CarInput.RangeKm,
                    CarInput.Seat,
                    CarInput.BasePrice!.Value);

                var color = await GetOrCreateColorByNameAsync(
                    version.VersionId,
                    CarInput.ColorName!,
                    CarInput.ExtraCost);

                await _inventorySvc.CreateAsync(new InventoryDto
                {
                    VersionId = version.VersionId,
                    ColorId = color.ColorId,
                    Quantity = CarInput.Quantity
                });

                TempData["Msg"] = "Car created successfully!";
                await _hub.Clients.All.SendAsync("CatalogChanged");
                return RedirectToPage();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                await LoadAllAsync();
                return Page();
            }
        }

        // ===== UPDATE (Edit modal) =====
        public async Task<IActionResult> OnPostUpdateCarFullAsync()
        {
            if (EditCar == null)
            {
                ModelState.AddModelError(string.Empty, "Invalid input.");
                await LoadAllAsync();
                return Page();
            }

            try
            {
                var model = await _modelSvc.GetByIdAsync(EditCar.ModelId)
                            ?? throw new KeyNotFoundException("Model not found.");
                model.ModelName = EditCar.ModelName;
                model.Segment = EditCar.Segment;
                model.Description = EditCar.Description;
                await _modelSvc.UpdateAsync(model);

                var version = await _versionSvc.GetByIdAsync(EditCar.VersionId)
                              ?? throw new KeyNotFoundException("Version not found.");
                version.ModelId = EditCar.ModelId;
                version.VersionName = EditCar.VersionName;
                version.BatteryCapacity = EditCar.BatteryCapacity;
                version.RangeKm = EditCar.RangeKm;
                version.Seat = EditCar.Seat;
                version.BasePrice = EditCar.BasePrice;
                await _versionSvc.UpdateAsync(version);

                var color = await _colorSvc.GetByIdAsync(EditCar.ColorId)
                            ?? throw new KeyNotFoundException("Color not found.");
                color.VersionId = EditCar.VersionId;
                color.ColorName = EditCar.ColorName;
                color.HexCode = EditCar.HexCode;
                color.ExtraCost = EditCar.ExtraCost;
                await _colorSvc.UpdateAsync(color);

                var inv = await _inventorySvc.GetByVersionColorAsync(version.VersionId, color.ColorId);
                if (EditCar.Quantity.HasValue)
                {
                    if (inv != null)
                    {
                        inv.Quantity = EditCar.Quantity;
                        await _inventorySvc.UpdateAsync(inv);
                    }
                    else
                    {
                        await _inventorySvc.CreateAsync(new InventoryDto
                        {
                            VersionId = version.VersionId,
                            ColorId = color.ColorId,
                            Quantity = EditCar.Quantity
                        });
                    }
                }

                TempData["Msg"] = "Car updated successfully.";
                await _hub.Clients.All.SendAsync("CatalogChanged");
                return RedirectToPage();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                await LoadAllAsync();
                return Page();
            }
        }

        // ===== DELETE: Xoá Inventory trước rồi xoá Color =====
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OnPostDeleteCarAsync(int colorId)
        {
            try
            {
                await _inventorySvc.DeleteByColorAsync(colorId); // dọn hết inventory của màu
                await _colorSvc.DeleteAsync(colorId);            // xoá Color

                TempData["Msg"] = "Car deleted.";
                await _hub.Clients.All.SendAsync("CatalogChanged");
                return RedirectToPage();
            }
            catch (DbUpdateException ex)
            {
                var inner = ex.InnerException?.Message ?? ex.Message;
                if (inner.Contains("OrderDetails", StringComparison.OrdinalIgnoreCase))
                    TempData["Msg"] = "Không thể xoá vì màu này đã được dùng trong đơn hàng (OrderDetails).";
                else
                    TempData["Msg"] = $"Delete failed: {inner}";
                return RedirectToPage();
            }
            catch (Exception ex)
            {
                TempData["Msg"] = ex.Message;
                return RedirectToPage();
            }
        }
    }
}
