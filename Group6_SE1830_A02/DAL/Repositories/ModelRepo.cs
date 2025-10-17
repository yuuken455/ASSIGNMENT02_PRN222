using DAL.IRepositories;
using Microsoft.EntityFrameworkCore;
using DAL.Entities;
using Microsoft.EntityFrameworkCore;
namespace DAL.Repositories
{
    public class ModelRepo : IModelRepo
    {
        private readonly DBContext _ctx;
        public ModelRepo(DBContext ctx) => _ctx = ctx;

        public Task<List<Model>> GetAllAsync(CancellationToken ct = default) =>
            _ctx.Models.AsNoTracking().OrderBy(m => m.ModelName).ToListAsync(ct);
            
        public Task<Model?> GetByIdAsync(int id, CancellationToken ct = default) =>
            _ctx.Models.FirstOrDefaultAsync(m => m.ModelId == id, ct);

        public async Task<Model> AddAsync(Model entity, CancellationToken ct = default)
        {
            _ctx.Models.Add(entity);
            await _ctx.SaveChangesAsync(ct);
            return entity;
        }

        public async Task UpdateAsync(Model entity, CancellationToken ct = default)
        {
            _ctx.Models.Update(entity);
            await _ctx.SaveChangesAsync(ct);
        }

        public async Task DeleteAsync(int id, CancellationToken ct = default)
        {
            var entity = await _ctx.Models.FindAsync(new object?[] { id }, ct);
            if (entity != null)
            {
                _ctx.Models.Remove(entity);
                await _ctx.SaveChangesAsync(ct);
            }
        }

        public Task<bool> ExistsByNameAsync(string name, int? excludeId = null, CancellationToken ct = default)
        {
            var q = _ctx.Models.AsQueryable().Where(m => m.ModelName == name);
            if (excludeId.HasValue) q = q.Where(m => m.ModelId != excludeId.Value);
            return q.AnyAsync(ct);
        }
    }
}
