using AutoMapper;
using BLL.DTOs;
using BLL.IServices;
using DAL.IRepositories;

namespace BLL.Services
{
    public class ColorService : IColorService
    {
        private readonly IColorRepo _repo;
        private readonly IMapper _mapper;

        public ColorService(IColorRepo repo, IMapper mapper)
        { _repo = repo; _mapper = mapper; }

        public async Task<List<ColorDto>> GetAllAsync()
            => _mapper.Map<List<ColorDto>>(await _repo.GetAllAsync());

        public async Task<ColorDto?> GetByIdAsync(int id)
            => _mapper.Map<ColorDto?>(await _repo.GetByIdAsync(id));

        public async Task<ColorDto> CreateAsync(ColorDto dto)
        {
            if (await _repo.ExistsByNameAsync(dto.VersionId, dto.ColorName))
                throw new InvalidOperationException("Color name already exists in this version.");

            var entity = _mapper.Map<DALColor>(dto);
            entity = await _repo.AddAsync(entity);
            return _mapper.Map<ColorDto>(entity);
        }

        public async Task UpdateAsync(ColorDto dto)
        {
            if (await _repo.ExistsByNameAsync(dto.VersionId, dto.ColorName, dto.ColorId))
                throw new InvalidOperationException("Color name already exists in this version.");

            var entity = await _repo.GetByIdAsync(dto.ColorId)
                        ?? throw new KeyNotFoundException("Color not found.");

            _mapper.Map(dto, entity);
            await _repo.UpdateAsync(entity);
        }

        public Task DeleteAsync(int id) => _repo.DeleteAsync(id);
    }
}
