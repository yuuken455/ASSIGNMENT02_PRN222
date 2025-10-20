using BLL.DTOs.CustomerDTOs;
using BLL.IServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EVDManagement.Pages.Customers
{
    public class IndexModel : PageModel
    {
        private readonly ICustomerService _customerService;

        public IndexModel(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        public ICollection<CustomerDTO> Customers { get; set; } = new List<CustomerDTO>();

        public async Task OnGet()
        {
            Customers = await _customerService.GetAllCustomersAsync();
        }

        public async Task<JsonResult> OnGetCustomersAsync()
        {
            var customers = await _customerService.GetAllCustomersAsync();
            return new JsonResult(customers);
        }
    }
}
