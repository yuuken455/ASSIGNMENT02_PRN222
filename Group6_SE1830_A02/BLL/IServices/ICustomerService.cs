using BLL.DTOs.CustomerDTOs;
using DAL.Entities;

namespace BLL.IServices
{
    public interface ICustomerService
    {
        Task<ICollection<CustomerDTO>> GetAllCustomersAsync();
        Task AddCustomerAsync(CreateCustomerDTO createCustomerDto);
        Task<CustomerDTO?> GetCustomerByIdAsync(int id);
        Task UpdateCustomerAsync(UpdateCustomerDTO customerDto);
        Task DeleteCustomerAsync(int id);
    }
}