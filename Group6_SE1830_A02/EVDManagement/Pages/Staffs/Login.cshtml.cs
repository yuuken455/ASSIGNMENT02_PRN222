using BLL.DTOs;
using BLL.IServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace EVDManagement.Pages.Staffs
{
    public class LoginModel : PageModel
    {
        private readonly IStaffService _staffService;

        public LoginModel(IStaffService staffService)
        {
            _staffService = staffService;
        }

        [BindProperty]
        public LoginDto LoginDto { get; set; }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid)
                return Page();

            var staff = await _staffService.Login(LoginDto);

            if (staff == null)
            {
                TempData["ErrorMessage"] = "Email hoặc mật khẩu không chính xác.";
                return Page();
            }

            HttpContext.Session.SetString("StaffName", staff.FullName ?? staff.Email);
            HttpContext.Session.SetInt32("StaffID", staff.StaffId);

            return RedirectToPage("/Index");
        }
    }
}
