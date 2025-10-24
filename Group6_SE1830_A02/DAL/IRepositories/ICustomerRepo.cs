using DAL.Entities;

namespace DAL.IRepositories
{
    public interface ICustomerRepo
    {
        Task<Customer?> GetCustomerByIdAsync(int id);
        Task<Customer?> GetCustomerByEmailAsync(string email);
        Task<Customer?> GetCustomerByPhoneAsync(string phone);

        
        Task<Customer?> GetCustomerByIdNumberAsync(string idnumber);

        Task<ICollection<Customer>> GetAllCustomersAsync();
        Task AddCustomerAsync(Customer customer);
        Task UpdateCustomerAsync(Customer customer);
        Task DeleteCustomerAsync(Customer customer);
    }
}
