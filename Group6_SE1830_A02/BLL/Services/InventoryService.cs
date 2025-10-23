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

        public async Task<List<InventoryDto>> GetAllAsync(CancellationToken ct = default)
            => _mapper.Map<List<InventoryDto>>(await _repo.GetAllAsync(ct));

        public async Task<InventoryDto?> GetByIdAsync(int id, CancellationToken ct = default)
            => _mapper.Map<InventoryDto?>(await _repo.GetByIdAsync(id, ct));

        public async Task<InventoryDto> CreateAsync(InventoryDto dto, CancellationToken ct = default)
        {
            var exist = await _repo.GetByVersionColorAsync(dto.VersionId, dto.ColorId, ct);
            if (exist != null)
            {
                exist.Quantity = (exist.Quantity ?? 0) + (dto.Quantity ?? 0);
                await _repo.UpdateAsync(exist, ct);
                return _mapper.Map<InventoryDto>(exist);
            }

            var entity = _mapper.Map<Inventory>(dto);
            entity = await _repo.AddAsync(entity, ct);
            return _mapper.Map<InventoryDto>(entity);
        }

        public async Task UpdateAsync(InventoryDto dto, CancellationToken ct = default)
        {
            var entity = await _repo.GetByIdAsync(dto.InventoryId, ct)
                        ?? throw new KeyNotFoundException("Inventory not found.");
            _mapper.Map(dto, entity);
            await _repo.UpdateAsync(entity, ct);
        }

        public Task DeleteAsync(int id, CancellationToken ct = default)
            => _repo.DeleteAsync(id, ct);

        public async Task<InventoryDto?> GetByVersionColorAsync(int versionId, int colorId, CancellationToken ct = default)
            => _mapper.Map<InventoryDto?>(await _repo.GetByVersionColorAsync(versionId, colorId, ct));

        public Task DeleteByColorAsync(int colorId, CancellationToken ct = default)
            => _repo.DeleteByColorAsync(colorId, ct);
    }
}
