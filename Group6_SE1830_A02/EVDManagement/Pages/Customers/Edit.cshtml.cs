using BLL.DTOs.CustomerDTOs;
using BLL.IServices;
using EVDManagement.SignalR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;

namespace EVDManagement.Pages.Customers
{
    public class EditModel : PageModel
    {
        private readonly ICustomerService _customerService;
        private readonly IHubContext<CustomerHub> _hubContext;
        public EditModel(ICustomerService customerService, IHubContext<CustomerHub> hubContext)
        {
            _customerService = customerService;
            _hubContext = hubContext;
        }

        public CustomerDTO Customer { get; set; } = new CustomerDTO();
        [BindProperty]
        public UpdateCustomerDTO UpdateCustomer { get; set; } = new UpdateCustomerDTO();

        public async Task<IActionResult> OnGet(int id)
        {
            var customer = await _customerService.GetCustomerByIdAsync(id);
            if (customer == null)
            {
                return NotFound();
            }

            UpdateCustomer.CustomerId = customer.CustomerId;
            UpdateCustomer.FullName = customer.FullName;
            UpdateCustomer.Email = customer.Email;
            UpdateCustomer.Phone = customer.Phone;
            UpdateCustomer.Idnumber = customer.Idnumber;
            UpdateCustomer.Dob = customer.Dob;
            UpdateCustomer.Address = customer.Address;
            UpdateCustomer.Note = customer.Note;

            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid)
                return Page();

            try
            {
                await _customerService.UpdateCustomerAsync(UpdateCustomer);
                TempData["SuccessMessage"] = "Cập nhật khách hàng thành công!";

                await _hubContext.Clients.All.SendAsync("CustomerUpdated", new
                {
                    customerId = UpdateCustomer.CustomerId,
                    fullName = UpdateCustomer.FullName
                });

                return RedirectToPage(new { id = UpdateCustomer.CustomerId });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return Page();
            }
        }
    }
}
