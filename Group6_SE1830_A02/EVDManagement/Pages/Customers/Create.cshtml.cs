using BLL.DTOs.CustomerDTOs;
using BLL.IServices;
using EVDManagement.SignalR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;

namespace EVDManagement.Pages.Customers
{
    public class CreateModel : PageModel
    {
        private readonly ICustomerService _customerService;
        private readonly IHubContext<CustomerHub> _hubContext;

        public CreateModel(ICustomerService customerService, IHubContext<CustomerHub> hubContext)
        {
            _customerService = customerService;
            _hubContext = hubContext;
        }

        [BindProperty]
        public CreateCustomerDTO Customer { get; set; } = new CreateCustomerDTO();

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                // Replace this line:
                // await _customerService.AddCustomerAsync(Customer);

                // With this line:
                throw new NotImplementedException("AddCustomerAsync is not implemented in ICustomerService. Please implement this method or use an existing method.");

                await _hubContext.Clients.All.SendAsync("CustomerAdded", new
                {
                    fullName = Customer.FullName,
                    phone = Customer.Phone,
                    email = Customer.Email,
                    idnumber = Customer.Idnumber,
                    dob = Customer.Dob?.ToString("dd/MM/yyyy"),
                    address = Customer.Address,
                    note = Customer.Note
                });

                TempData["SuccessMessage"] = "Thêm khách hàng thành công!";

                return RedirectToPage("Create");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return Page();
            }
        }
    }
}
