using AutoMapper;
using BLL.DTOs;
using BLL.DTOs.CustomerDTOs;
using DAL.Entities;
using DALModel = DAL.Entities.Model;
using DALVersion = DAL.Entities.Version;
using DALColor = DAL.Entities.Color;
using DALInventory = DAL.Entities.Inventory;

namespace BLL.Mappings
{
    public class AutoMappingProfile : Profile
    {
        public AutoMappingProfile()
        {
            // Customer mappings
            CreateMap<Customer, CustomerDTO>().ReverseMap();
            CreateMap<CreateCustomerDTO, Customer>();
            CreateMap<UpdateCustomerDTO, Customer>();
            CreateMap<CustomerDTO, UpdateCustomerDTO>().ReverseMap();

            // Model mappings
            CreateMap<DALModel, ModelDto>().ReverseMap();

            // Version & Color mappings
            CreateMap<DALVersion, VersionDto>().ReverseMap();
            CreateMap<DALColor, ColorDto>().ReverseMap();

            // Inventory mappings
            CreateMap<DALInventory, InventoryDTO>().ReverseMap();
        }
    }
}
