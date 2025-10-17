using AutoMapper;
using BLL.DTOs.CustomerDTOs;
using DAL.Entities;

namespace BLL.Mappings
{
    public class AutoMappingProfile : Profile
    {
        public AutoMappingProfile()
        {
            CreateMap<Customer, CustomerDTO>().ReverseMap();

            CreateMap<CreateCustomerDTO, Customer>();

            CreateMap<UpdateCustomerDTO, Customer>(); 
            
            CreateMap<CustomerDTO, UpdateCustomerDTO>().ReverseMap();   
        }
    }
}
