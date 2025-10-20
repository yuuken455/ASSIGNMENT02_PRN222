using BLL.IServices;
using EVDManagement.SignalR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;

namespace EVDManagement.Pages.Customers
{
    public class DeleteModel : PageModel
    {
        private readonly ICustomerService _customerService;
        private readonly IHubContext<CustomerHub> _hubContext;

        public DeleteModel(ICustomerService customerService, IHubContext<CustomerHub> hubContext)
        {
            _customerService = customerService;
            _hubContext = hubContext;
        }

        [BindProperty]
        public int CustomerId { get; set; }

        [BindProperty]
        public string? CustomerName { get; set; }

        public async Task<IActionResult> OnGet(int id)
        {
            var customer = await _customerService.GetCustomerByIdAsync(id);
            if (customer == null)
                return NotFound();

            CustomerId = customer.CustomerId;
            CustomerName = customer.FullName;
            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            try
            {
                var deletedCustomer = await _customerService.GetCustomerByIdAsync(CustomerId);
                await _customerService.DeleteCustomerAsync(CustomerId);
                if (deletedCustomer != null)
                {
                    await _hubContext.Clients.All.SendAsync("CustomerDeleted", new
                    {
                        customerId = deletedCustomer.CustomerId,
                        fullName = deletedCustomer.FullName
                    });
                }

                TempData["SuccessMessage"] = "Xóa khách hàng thành công!";
                return RedirectToPage("Index");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToPage("Index");
            }
        }
    }
}
