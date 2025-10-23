using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EVDManagement.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public IActionResult OnGet()
        {
            if (HttpContext.Session.GetInt32("StaffID") == null)
            {
                return RedirectToPage("/Staffs/Login");
            }

            ViewData["Title"] = "Trang chủ";
            return Page();
        }
    }
}
