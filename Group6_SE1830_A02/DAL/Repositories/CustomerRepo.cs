using DAL.Entities;
using DAL.IRepositories;
using Microsoft.EntityFrameworkCore;
using System;

namespace DAL.Repositories
{
    public class CustomerRepo : ICustomerRepo
    {
        private readonly DBContext _db; // đổi tên DbContext nếu khác

        public CustomerRepo(DBContext db)
        {
            _db = db;
        }

        public async Task AddCustomerAsync(Customer customer)
        {
            _db.Customers.Add(customer);
            await _db.SaveChangesAsync();
        }

        public async Task DeleteCustomerAsync(Customer customer)
        {
            _db.Customers.Remove(customer);
            await _db.SaveChangesAsync();
        }

        public async Task<ICollection<Customer>> GetAllCustomersAsync()
        {
            return await _db.Customers.AsNoTracking().ToListAsync();
        }

        public async Task<Customer?> GetCustomerByEmailAsync(string email)
        {
            if (string.IsNullOrWhiteSpace(email)) return null;
            var norm = email.Trim().ToLowerInvariant();
            return await _db.Customers.FirstOrDefaultAsync(c => c.Email != null && c.Email.ToLower() == norm);
        }

        public async Task<Customer?> GetCustomerByIdAsync(int id)
        {
            return await _db.Customers.FirstOrDefaultAsync(c => c.CustomerId == id);
        }

        public async Task<Customer?> GetCustomerByPhoneAsync(string phone)
        {
            if (string.IsNullOrWhiteSpace(phone)) return null;
            var norm = phone.Trim();
            return await _db.Customers.FirstOrDefaultAsync(c => c.Phone == norm);
        }

        public async Task<Customer?> GetCustomerByIdNumberAsync(string idnumber)
        {
            if (string.IsNullOrWhiteSpace(idnumber)) return null;
            var norm = idnumber.Trim();
            return await _db.Customers.FirstOrDefaultAsync(c => c.Idnumber == norm);
        }

        public async Task UpdateCustomerAsync(Customer customer)
        {
            _db.Customers.Update(customer);
            await _db.SaveChangesAsync();
        }
    }
}
