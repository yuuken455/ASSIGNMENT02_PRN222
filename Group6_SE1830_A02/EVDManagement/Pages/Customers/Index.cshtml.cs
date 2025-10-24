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

        // Dùng OnGetAsync và GỌI service
        public async Task OnGetAsync()
        {
            Customers = await _customerService.GetAllCustomersAsync();
        }

        // Handler JSON cho fetch('?handler=Customers')
        public async Task<JsonResult> OnGetCustomersAsync()
        {
            var customers = await _customerService.GetAllCustomersAsync();
            return new JsonResult(customers);
        }
    }
}
