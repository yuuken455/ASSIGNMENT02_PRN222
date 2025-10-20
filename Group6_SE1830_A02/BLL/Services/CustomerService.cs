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

            var existEmail = await _customerRepo.GetCustomerByEmailAsync(createCustomerDto.Email!);
            var existPhone = await _customerRepo.GetCustomerByPhoneAsync(createCustomerDto.Phone!);

            if (existEmail is not null)
            {
                errors.Add("Email đã được sử dụng");
            }

            if (existPhone is not null)
            {
                errors.Add("Số điện thoại đã được sử dụng");
            }

            if (errors.Any())
            {
                throw new Exception(string.Join("\n", errors));
            }

            var newCustomer = _mapper.Map<DAL.Entities.Customer>(createCustomerDto);
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
                throw new Exception("Không thể xóa khách hàng vì có dữ liệu liên quan (giao dịch)");
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
            {
                throw new Exception("Khách hàng không tồn tại");
            }

            if (oldCustomer.Email != customerDto.Email)
            {
                var existEmail = await _customerRepo.GetCustomerByEmailAsync(customerDto.Email!);
                if (existEmail is not null)
                {
                    errors.Add("Email đã được sử dụng");
                }
            }

            if (oldCustomer.Phone != customerDto.Phone)
            {
                var existPhone = await _customerRepo.GetCustomerByPhoneAsync(customerDto.Phone!);
                if (existPhone is not null)
                {
                    errors.Add("Số điện thoại đã được sử dụng");
                }
            }

            if (errors.Any())
            {
                throw new Exception(string.Join("\n", errors));
            }

            _mapper.Map(customerDto, oldCustomer);

            var updatedCustomer = _mapper.Map<DAL.Entities.Customer>(oldCustomer);
            await _customerRepo.UpdateCustomerAsync(updatedCustomer);
        }
    }
}
