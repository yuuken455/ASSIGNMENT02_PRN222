using BLL.DTOs;
using BLL.IServices;
using EVDManagement.SignalR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;

namespace EVDManagement.Pages.Models
{
    public class EditModel : PageModel
    {
        private readonly IModelService _svc;
        private readonly IHubContext<ModelHub> _hub;

        public EditModel(IModelService svc, IHubContext<ModelHub> hub)
        { _svc = svc; _hub = hub; }

        [BindProperty] public ModelDto Item { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var dto = await _svc.GetByIdAsync(id);
            if (dto == null) return NotFound();
            Item = dto;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) return Page();
            await _svc.UpdateAsync(Item);
            await _hub.Clients.All.SendAsync("ModelsChanged");
            TempData["Msg"] = "Updated successfully.";
            return RedirectToPage("Index");
        }
    }
}
