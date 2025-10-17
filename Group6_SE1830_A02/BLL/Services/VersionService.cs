using AutoMapper;
using BLL.DTOs;
using BLL.IServices;
using DAL.IRepositories;

namespace BLL.Services
{
    public class VersionService : IVersionService
    {
        private readonly IVersionRepo _repo;
        private readonly IMapper _mapper;

        public VersionService(IVersionRepo repo, IMapper mapper)
        { _repo = repo; _mapper = mapper; }

        public async Task<List<VersionDto>> GetAllAsync()
            => _mapper.Map<List<VersionDto>>(await _repo.GetAllAsync());

        public async Task<VersionDto?> GetByIdAsync(int id)
            => _mapper.Map<VersionDto?>(await _repo.GetByIdAsync(id));

        public async Task<VersionDto> CreateAsync(VersionDto dto)
        {
            if (await _repo.ExistsByNameAsync(dto.ModelId, dto.VersionName))
                throw new InvalidOperationException("Version name already exists in this model.");

            var entity = _mapper.Map<DALVersion>(dto);
            entity = await _repo.AddAsync(entity);
            return _mapper.Map<VersionDto>(entity);
        }

        public async Task UpdateAsync(VersionDto dto)
        {
            if (await _repo.ExistsByNameAsync(dto.ModelId, dto.VersionName, dto.VersionId))
                throw new InvalidOperationException("Version name already exists in this model.");

            var entity = await _repo.GetByIdAsync(dto.VersionId)
                        ?? throw new KeyNotFoundException("Version not found.");

            _mapper.Map(dto, entity);
            await _repo.UpdateAsync(entity);
        }

        public Task DeleteAsync(int id) => _repo.DeleteAsync(id);
    }
}
