using DAL.Entities;
using DAL.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public class CustomerRepo : ICustomerRepo
    {
        private readonly DBContext _context;

        public CustomerRepo(DBContext context)
        {
            _context = context;
        }

        public async Task AddCustomerAsync(Customer customer)
        {
            await _context.Customers.AddAsync(customer);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteCustomerAsync(Customer customer)
        {
            _context.Customers.Remove(customer);
            await _context.SaveChangesAsync();
        }

        public async Task<ICollection<Customer>> GetAllCustomersAsync()
        {
            return await _context.Customers.ToListAsync();
        }

        public async Task<Customer?> GetCustomerByEmailAsync(string email)
        {
            return await _context.Customers
                .FirstOrDefaultAsync(c => c.Email == email);
        }

        public Task<Customer?> GetCustomerByIdAsync(int id)
        {
            return _context.Customers
                .FirstOrDefaultAsync(c => c.CustomerId == id);
        }

        public Task<Customer?> GetCustomerByPhoneAsync(string phone)
        {
            return _context.Customers
                .FirstOrDefaultAsync(c => c.Phone == phone);
        }

        public async Task UpdateCustomerAsync(Customer customer)
        {
            _context.Customers.Update(customer);
            await _context.SaveChangesAsync();
        }
    }
}
