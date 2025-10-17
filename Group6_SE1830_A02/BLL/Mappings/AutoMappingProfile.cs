using AutoMapper;
using BLL.DTOs;
// alias sẵn cho tránh System.Version (nếu bạn đang dùng global usings thì giữ nguyên)
using DALModel = DAL.Entities.Model;
using DALInventory = DAL.Entities.Inventory;

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
        }
    }
}
