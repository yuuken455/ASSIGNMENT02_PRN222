using AutoMapper;
using BLL.DTOs.CustomerDTOs;
using BLL.IServices;
using DAL.Entities;
using DAL.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace BLL.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly IMapper _mapper;
        private readonly ICustomerRepo _customerRepo;

        public CustomerService(IMapper mapper, ICustomerRepo customerRepo)
        {
            _mapper = mapper;
            _customerRepo = customerRepo;
        }

        public async Task AddCustomerAsync(CreateCustomerDTO createCustomerDto)
        {
            var errors = new List<string>();

            // Normalize input (tránh trùng vì khác hoa/thường/khoảng trắng)
            createCustomerDto.Email = createCustomerDto.Email?.Trim().ToLowerInvariant();
            createCustomerDto.Phone = createCustomerDto.Phone?.Trim();
            createCustomerDto.Idnumber = createCustomerDto.Idnumber?.Trim();

            var existEmail = await _customerRepo.GetCustomerByEmailAsync(createCustomerDto.Email!);
            if (existEmail is not null) errors.Add("Email đã được sử dụng");

            var existPhone = await _customerRepo.GetCustomerByPhoneAsync(createCustomerDto.Phone!);
            if (existPhone is not null) errors.Add("Số điện thoại đã được sử dụng");

            var existId = await _customerRepo.GetCustomerByIdNumberAsync(createCustomerDto.Idnumber!);
            if (existId is not null) errors.Add("CCCD/CMND đã được sử dụng");

            if (errors.Any())
                throw new Exception(string.Join("\n", errors));

            var newCustomer = _mapper.Map<Customer>(createCustomerDto);
            await _customerRepo.AddCustomerAsync(newCustomer);
        }

        public async Task DeleteCustomerAsync(int id)
        {
            var customer = await _customerRepo.GetCustomerByIdAsync(id);
            if (customer == null)
                throw new Exception("Khách hàng không tồn tại.");

            try
            {
                await _customerRepo.DeleteCustomerAsync(customer);
            }
            catch (DbUpdateException)
            {
                throw new Exception("Không thể xóa khách hàng vì có dữ liệu liên quan (giao dịch).");
            }
        }

        public async Task<ICollection<CustomerDTO>> GetAllCustomersAsync()
        {
            var customers = await _customerRepo.GetAllCustomersAsync();
            return _mapper.Map<ICollection<CustomerDTO>>(customers);
        }

        public async Task<CustomerDTO?> GetCustomerByIdAsync(int id)
        {
            var customer = await _customerRepo.GetCustomerByIdAsync(id);
            return _mapper.Map<CustomerDTO?>(customer);
        }

        public async Task UpdateCustomerAsync(UpdateCustomerDTO customerDto)
        {
            var errors = new List<string>();

            var oldCustomer = await _customerRepo.GetCustomerByIdAsync(customerDto.CustomerId);
            if (oldCustomer is null)
                throw new Exception("Khách hàng không tồn tại");

            // Normalize trước khi so sánh/check trùng
            var newEmail = customerDto.Email?.Trim().ToLowerInvariant();
            var newPhone = customerDto.Phone?.Trim();
            var newId = customerDto.Idnumber?.Trim();

            if (!string.Equals(oldCustomer.Email, newEmail, StringComparison.OrdinalIgnoreCase))
            {
                var dup = await _customerRepo.GetCustomerByEmailAsync(newEmail!);
                if (dup is not null) errors.Add("Email đã được sử dụng");
            }

            if (!string.Equals(oldCustomer.Phone, newPhone, StringComparison.OrdinalIgnoreCase))
            {
                var dup = await _customerRepo.GetCustomerByPhoneAsync(newPhone!);
                if (dup is not null) errors.Add("Số điện thoại đã được sử dụng");
            }

            if (!string.Equals(oldCustomer.Idnumber, newId, StringComparison.OrdinalIgnoreCase))
            {
                var dup = await _customerRepo.GetCustomerByIdNumberAsync(newId!);
                if (dup is not null) errors.Add("CCCD/CMND đã được sử dụng");
            }

            if (errors.Any())
                throw new Exception(string.Join("\n", errors));

            // Map DTO -> entity đang track
            _mapper.Map(customerDto, oldCustomer);

            await _customerRepo.UpdateCustomerAsync(oldCustomer);
        }
    }
}
