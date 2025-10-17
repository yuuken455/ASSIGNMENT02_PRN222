using AutoMapper;
using BLL.DTOs;
using BLL.IServices;
using DAL.Entities;
using DAL.IRepositories;

namespace BLL.Services
{
    public class InventoryService : IInventoryService
    {
        private readonly IInventoryRepo _repo;
        private readonly IMapper _mapper;

        public InventoryService(IInventoryRepo repo, IMapper mapper)
        { _repo = repo; _mapper = mapper; }

        public async Task<List<InventoryDto>> GetAllAsync()
            => _mapper.Map<List<InventoryDto>>(await _repo.GetAllAsync());

        public async Task<InventoryDto?> GetByIdAsync(int id)
            => _mapper.Map<InventoryDto?>(await _repo.GetByIdAsync(id));

        public async Task<InventoryDto> CreateAsync(InventoryDto dto)
        {
            // hợp nhất nếu đã tồn tại cùng Version+Color → cộng dồn quantity
            var exist = await _repo.GetByVersionColorAsync(dto.VersionId, dto.ColorId);
            if (exist != null)
            {
                exist.Quantity = (exist.Quantity ?? 0) + (dto.Quantity ?? 0);
                await _repo.UpdateAsync(exist);
                return _mapper.Map<InventoryDto>(exist);
            }

            var entity = _mapper.Map<Inventory>(dto);
            entity = await _repo.AddAsync(entity);
            return _mapper.Map<InventoryDto>(entity);
        }

        public async Task UpdateAsync(InventoryDto dto)
        {
            var entity = await _repo.GetByIdAsync(dto.InventoryId)
                        ?? throw new KeyNotFoundException("Inventory not found.");
            _mapper.Map(dto, entity);
            await _repo.UpdateAsync(entity);
        }

        public Task DeleteAsync(int id) => _repo.DeleteAsync(id);

        public async Task<InventoryDto?> GetByVersionColorAsync(int versionId, int colorId)
            => _mapper.Map<InventoryDto?>(await _repo.GetByVersionColorAsync(versionId, colorId));
    }
}
