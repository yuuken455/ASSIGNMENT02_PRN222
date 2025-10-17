using BLL.DTOs;
using BLL.IServices;
using EVDManagement.SignalR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;

namespace EVDManagement.Pages.Models
{
    public class CreateModel : PageModel
    {
        private readonly IModelService _svc;
        private readonly IHubContext<ModelHub> _hub;

        public CreateModel(IModelService svc, IHubContext<ModelHub> hub)
        { _svc = svc; _hub = hub; }

        [BindProperty] public ModelDto Item { get; set; } = new();

        public void OnGet() { }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) return Page();
            await _svc.CreateAsync(Item);
            await _hub.Clients.All.SendAsync("ModelsChanged");
            TempData["Msg"] = "Created successfully.";
            return RedirectToPage("Index");
        }
    }
}
