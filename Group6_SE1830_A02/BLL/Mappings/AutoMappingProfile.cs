using AutoMapper;
using BLL.DTOs;
using BLL.DTOs.CustomerDTOs;
using DAL.Entities;
using DALCustomer = DAL.Entities.Customer;
using DALInventory = DAL.Entities.Inventory;
using DALModel = DAL.Entities.Model;
using DALTestDrive = DAL.Entities.TestDriveAppointment;

namespace BLL.Mappings
{
    public class AutoMappingProfile : Profile
    {
        public AutoMappingProfile()
        {
            // Model
            CreateMap<DALModel, ModelDto>().ReverseMap();

            // Version & Color
            CreateMap<DALVersion, VersionDto>().ReverseMap();
            CreateMap<DALColor, ColorDto>().ReverseMap();

            CreateMap<DALInventory, InventoryDto>().ReverseMap();

            CreateMap<DAL.Entities.TestDriveAppointment, TestDriveAppointmentDto>()
                .ForMember(d => d.ModelId, o => o.MapFrom(s => s.CarVersion.ModelId))
                .ForMember(d => d.ModelName, o => o.MapFrom(s => s.CarVersion.Model.ModelName))
                .ForMember(d => d.VersionName, o => o.MapFrom(s => s.CarVersion.VersionName))
                .ForMember(d => d.ColorName, o => o.MapFrom(s => s.Color.ColorName))
                .ForMember(d => d.CustomerName, o => o.MapFrom(s => s.Customer.FullName));

            CreateMap<TestDriveAppointmentDto, DAL.Entities.TestDriveAppointment>()
                .ForMember(d => d.Customer, o => o.Ignore())
                .ForMember(d => d.CarVersion, o => o.Ignore())
                .ForMember(d => d.Color, o => o.Ignore());



            CreateMap<DALCustomer, CustomerDTO>().ReverseMap();
        }
    }
}
