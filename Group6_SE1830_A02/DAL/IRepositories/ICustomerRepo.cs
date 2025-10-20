using DAL.Entities;

namespace DAL.IRepositories
{
    public interface ICustomerRepo
    {
        Task<ICollection<Customer>> GetAllCustomersAsync();
        Task AddCustomerAsync(Customer customer);
        Task<Customer?> GetCustomerByIdAsync(int id);
        Task<Customer?> GetCustomerByEmailAsync(string email);
        Task<Customer?> GetCustomerByPhoneAsync(string phone);
        Task UpdateCustomerAsync(Customer customer);
        Task DeleteCustomerAsync(Customer customer);
    }
}