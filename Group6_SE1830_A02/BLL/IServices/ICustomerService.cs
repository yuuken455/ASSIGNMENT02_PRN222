using BLL.DTOs.CustomerDTOs;
using DAL.Entities;

namespace BLL.IServices
{
    public interface ICustomerService
    {
        Task<CustomerDTO?> GetCustomerByIdAsync(int id);
        Task UpdateCustomerAsync(UpdateCustomerDTO updateCustomerDto);
        Task DeleteCustomerAsync(int id);
        Task<ICollection<CustomerDTO>> GetAllCustomersAsync();
        Task AddCustomerAsync(CreateCustomerDTO createCustomerDto);

    }
}