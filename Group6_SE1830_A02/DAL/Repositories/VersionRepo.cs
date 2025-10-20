using DAL.IRepositories;
using Microsoft.EntityFrameworkCore;
using DAL.Entities;
namespace DAL.Repositories
{
    public class VersionRepo : IVersionRepo
    {
        private readonly DBContext _ctx;
        public VersionRepo(DBContext ctx) => _ctx = ctx;

        public Task<List<DALVersion>> GetAllAsync(CancellationToken ct = default) =>
            _ctx.Versions.AsNoTracking()
                .OrderBy(v => v.ModelId).ThenBy(v => v.VersionName)
                .ToListAsync(ct);

        public Task<DALVersion?> GetByIdAsync(int id, CancellationToken ct = default) =>
            _ctx.Versions.FirstOrDefaultAsync(v => v.VersionId == id, ct);

        public async Task<DALVersion> AddAsync(DALVersion entity, CancellationToken ct = default)
        {
            _ctx.Versions.Add(entity);
            await _ctx.SaveChangesAsync(ct);
            return entity;
        }

        public async Task UpdateAsync(DALVersion entity, CancellationToken ct = default)
        {
            _ctx.Versions.Update(entity);
            await _ctx.SaveChangesAsync(ct);
        }

        public async Task DeleteAsync(int id, CancellationToken ct = default)
        {
            var entity = await _ctx.Versions.FindAsync(new object?[] { id }, ct);
            if (entity != null)
            {
                _ctx.Versions.Remove(entity);
                await _ctx.SaveChangesAsync(ct);
            }
        }

        public Task<bool> ExistsByNameAsync(int modelId, string versionName, int? excludeId = null, CancellationToken ct = default)
        {
            var q = _ctx.Versions.AsQueryable()
                    .Where(v => v.ModelId == modelId && v.VersionName == versionName);
            if (excludeId.HasValue) q = q.Where(v => v.VersionId != excludeId.Value);
            return q.AnyAsync(ct);
        }
    }
}
