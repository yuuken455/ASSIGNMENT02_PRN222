using BLL.DTOs;
using BLL.IServices;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EVDManagement.Pages.Models
{
    public class IndexModel : PageModel
    {
        private readonly IModelService _svc;
        public IndexModel(IModelService svc) => _svc = svc;

        public List<ModelDto> Items { get; set; } = new();

        public async Task OnGetAsync()
        {
            Items = await _svc.GetAllAsync();
        }
    }
}
