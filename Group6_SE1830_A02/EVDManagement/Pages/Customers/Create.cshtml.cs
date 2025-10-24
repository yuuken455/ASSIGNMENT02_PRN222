using BLL.DTOs.CustomerDTOs;
using BLL.IServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EVDManagement.Pages.Customers
{
    public class CreateModel : PageModel
    {
        private readonly ICustomerService _customerService;

        public CreateModel(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        [BindProperty]
        public CreateCustomerDTO Customer { get; set; } = new();

        public void OnGet() { }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            try
            {
                await _customerService.AddCustomerAsync(Customer);
                TempData["SuccessMessage"] = "Thêm khách hàng thành công!";
                return RedirectToPage("Index");
            }
            catch (Exception ex)
            {
                // gom tất cả lỗi từ Service hiển thị ra Validation summary
                ModelState.AddModelError(string.Empty, ex.Message);
                return Page();
            }
        }
    }
}
