using AutoMapper;
using BLL.DTOs;
using BLL.DTOs.CustomerDTOs;
using DAL.Entities;

// Tạo bí danh để code gọn gàng hơn
using DALCustomer = DAL.Entities.Customer;
using DALModel = DAL.Entities.Model;
using DALInventory = DAL.Entities.Inventory;
using DALTestDrive = DAL.Entities.TestDriveAppointment;

namespace BLL.Mappings
{
    public class AutoMappingProfile : Profile
    {
        public AutoMappingProfile()
        {
            // 🟩 Staff mapping
            CreateMap<Staff, StaffDto>().ReverseMap();

            // 🟩 Customer mappings
            CreateMap<DALCustomer, CustomerDTO>().ReverseMap();

            // Create rõ ràng bỏ map Id để DB tự sinh
            CreateMap<CreateCustomerDTO, DALCustomer>()
                .ForMember(d => d.CustomerId, opt => opt.Ignore());

            CreateMap<UpdateCustomerDTO, DALCustomer>();
            CreateMap<CustomerDTO, UpdateCustomerDTO>().ReverseMap();

            // 🟩 Model mappings
            CreateMap<DALModel, ModelDto>().ReverseMap();

            // 🟩 Version mappings
            CreateMap<DALVersion, VersionDto>().ReverseMap();

            // 🟩 Color mappings
            CreateMap<DALColor, ColorDto>().ReverseMap();

            // 🟩 TestDriveAppointment mappings
            CreateMap<DALTestDrive, TestDriveAppointmentDTO>()
                // Null-guard để tránh NRE khi navigation null
                .ForMember(dest => dest.ModelId, opt => opt.MapFrom(src => src.CarVersion != null ? src.CarVersion.ModelId : (int?)null))
                .ForMember(dest => dest.ModelName, opt => opt.MapFrom(src => src.CarVersion != null && src.CarVersion.Model != null ? src.CarVersion.Model.ModelName : null))
                .ForMember(dest => dest.CarVersionId, opt => opt.MapFrom(src => src.CarVersionId))
                .ForMember(dest => dest.VersionName, opt => opt.MapFrom(src => src.CarVersion != null ? src.CarVersion.VersionName : null))
                .ForMember(dest => dest.ColorId, opt => opt.MapFrom(src => src.ColorId))
                .ForMember(dest => dest.ColorName, opt => opt.MapFrom(src => src.Color != null ? src.Color.ColorName : null))
                .ForMember(dest => dest.CustomerName, opt => opt.MapFrom(src => src.Customer != null ? src.Customer.FullName : null))
                // ✅ THÊM 2 field mới
                .ForMember(dest => dest.CustomerPhone, opt => opt.MapFrom(src => src.Customer != null ? src.Customer.Phone : null))
                .ForMember(dest => dest.CustomerIdnumber, opt => opt.MapFrom(src => src.Customer != null ? src.Customer.Idnumber : null));

            CreateMap<TestDriveAppointmentDTO, DALTestDrive>()
                .ForMember(dest => dest.CarVersion, opt => opt.Ignore())
                .ForMember(dest => dest.Color, opt => opt.Ignore())
                .ForMember(dest => dest.Customer, opt => opt.Ignore());

            // 🟩 Inventory mappings
            CreateMap<DALInventory, InventoryDto>().ReverseMap();
        }
    }
}
