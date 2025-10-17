using BLL.DTOs;
using BLL.IServices;
using EVDManagement.SignalR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;

namespace EVDManagement.Pages.Models
{
    public class DeleteModel : PageModel
    {
        private readonly IModelService _svc;
        private readonly IHubContext<ModelHub> _hub;

        public DeleteModel(IModelService svc, IHubContext<ModelHub> hub)
        { _svc = svc; _hub = hub; }

        public ModelDto? Item { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Item = await _svc.GetByIdAsync(id);
            return Item == null ? NotFound() : Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            await _svc.DeleteAsync(id);
            await _hub.Clients.All.SendAsync("ModelsChanged");
            TempData["Msg"] = "Deleted successfully.";
            return RedirectToPage("Index");
        }
    }
}
