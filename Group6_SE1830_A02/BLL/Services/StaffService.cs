using AutoMapper;
using BLL.DTOs;
using BLL.IServices;
using DAL.IRepositories;

namespace BLL.Services
{
    public class StaffService : IStaffService
    {
        private readonly IMapper _mapper;
        private readonly IStaffRepo _staffRepo;

        public StaffService(IMapper mapper, IStaffRepo staffRepo)
        {
            _mapper = mapper;
            _staffRepo = staffRepo;
        }

        public async Task<StaffDto?> Login(LoginDto loginDto)
        {
            var staff = await _staffRepo.GetStaffByEmailAsync(loginDto.Email);
            if (staff == null || staff.Password != loginDto.Password)
            {
                return null;
            }
            return _mapper.Map<StaffDto>(staff);
        }
    }
}
