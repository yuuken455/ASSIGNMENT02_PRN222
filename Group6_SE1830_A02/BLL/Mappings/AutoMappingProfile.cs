using AutoMapper;
using BLL.DTOs;
using DAL.Entities;

namespace BLL.Mappings
{
    public class AutoMappingProfile : Profile
    {
        public AutoMappingProfile()
        {
            // ... các mapping khác 

            CreateMap<Model, ModelDto>().ReverseMap();
        }
    }
}
