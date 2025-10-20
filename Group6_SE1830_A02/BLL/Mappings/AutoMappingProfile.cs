using AutoMapper;
using BLL.DTOs;
using BLL.DTOs.CustomerDTOs;
using DAL.Entities;
using DALCustomer = DAL.Entities.Customer;
using DALModel = DAL.Entities.Model;
using DALVersion = DAL.Entities.Version;
using DALColor = DAL.Entities.Color;
using DALInventory = DAL.Entities.Inventory;
using DALTestDrive = DAL.Entities.TestDriveAppointment;

namespace BLL.Mappings
{
    public class AutoMappingProfile : Profile
    {
        public AutoMappingProfile()
        {
            // Customer mappings
            CreateMap<DALCustomer, CustomerDTO>().ReverseMap();
            CreateMap<CreateCustomerDTO, DALCustomer>();
            CreateMap<UpdateCustomerDTO, DALCustomer>();
            CreateMap<CustomerDTO, UpdateCustomerDTO>().ReverseMap();

            // Model mappings
            CreateMap<DALModel, ModelDto>().ReverseMap();

            // Version & Color mappings
            CreateMap<DALVersion, VersionDto>().ReverseMap();
            CreateMap<DALColor, ColorDto>().ReverseMap();

            // TestDriveAppointment mappings
            CreateMap<DALTestDrive, TestDriveAppointmentDTO>()
                .ForMember(d => d.ModelId, o => o.MapFrom(s => s.CarVersion.ModelId))
                .ForMember(d => d.ModelName, o => o.MapFrom(s => s.CarVersion.Model.ModelName))
                .ForMember(d => d.VersionId, o => o.MapFrom(s => s.CarVersionId))
                .ForMember(d => d.VersionName, o => o.MapFrom(s => s.CarVersion.VersionName))
                .ForMember(d => d.ColorId, o => o.MapFrom(s => s.ColorId))
                .ForMember(d => d.ColorName, o => o.MapFrom(s => s.Color.ColorName))
                .ForMember(d => d.CustomerName, o => o.MapFrom(s => s.Customer.FullName));

            CreateMap<TestDriveAppointmentDTO, DALTestDrive>()
                .ForMember(d => d.ModelId, o => o.Ignore())
                .ForMember(d => d.CarVersion, o => o.Ignore())
                .ForMember(d => d.Color, o => o.Ignore());

            // Inventory mappings
            CreateMap<DALInventory, InventoryDTO>().ReverseMap();
        }
    }
}
