using AutoMapper;
using BLL.DTOs;
using BLL.IServices;
using DAL.Entities;
using DAL.IRepositories;
using DAL.Repositories;

namespace BLL.Services
{
    public class ModelService : IModelService
    {
        private readonly IModelRepo _repo;
        private readonly IMapper _mapper;

        public ModelService(IModelRepo repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<List<ModelDto>> GetAllAsync()
        {
            var list = await _repo.GetAllAsync();
            return _mapper.Map<List<ModelDto>>(list);
        }

        public async Task<ModelDto?> GetByIdAsync(int id)
        {
            var entity = await _repo.GetByIdAsync(id);
            return entity == null ? null : _mapper.Map<ModelDto>(entity);
        }

        public async Task<ModelDto> CreateAsync(ModelDto dto)
        {
            if (await _repo.ExistsByNameAsync(dto.ModelName))
                throw new InvalidOperationException("Model name already exists.");

            var entity = _mapper.Map<Model>(dto);
            entity = await _repo.AddAsync(entity);
            return _mapper.Map<ModelDto>(entity);
        }

        public async Task UpdateAsync(ModelDto dto)
        {
            if (await _repo.ExistsByNameAsync(dto.ModelName, dto.ModelId))
                throw new InvalidOperationException("Model name already exists.");

            var entity = await _repo.GetByIdAsync(dto.ModelId)
                        ?? throw new KeyNotFoundException("Model not found.");
            _mapper.Map(dto, entity);
            await _repo.UpdateAsync(entity);
        }

        public Task DeleteAsync(int id) => _repo.DeleteAsync(id);
    }
}
