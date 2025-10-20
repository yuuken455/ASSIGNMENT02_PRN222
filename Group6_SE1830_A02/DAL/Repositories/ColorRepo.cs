using DAL.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public class ColorRepo : IColorRepo
    {
        private readonly DBContext _ctx;
        public ColorRepo(DBContext ctx) => _ctx = ctx;

        public Task<List<DALColor>> GetAllAsync(CancellationToken ct = default) =>
            _ctx.Colors.AsNoTracking()
                .OrderBy(c => c.VersionId).ThenBy(c => c.ColorName)
                .ToListAsync(ct);

        public Task<DALColor?> GetByIdAsync(int id, CancellationToken ct = default) =>
            _ctx.Colors.FirstOrDefaultAsync(c => c.ColorId == id, ct);

        public async Task<DALColor> AddAsync(DALColor entity, CancellationToken ct = default)
        {
            _ctx.Colors.Add(entity);
            await _ctx.SaveChangesAsync(ct);
            return entity;
        }

        public async Task UpdateAsync(DALColor entity, CancellationToken ct = default)
        {
            _ctx.Colors.Update(entity);
            await _ctx.SaveChangesAsync(ct);
        }

        public async Task DeleteAsync(int id, CancellationToken ct = default)
        {
            var entity = await _ctx.Colors.FindAsync(new object?[] { id }, ct);
            if (entity != null)
            {
                _ctx.Colors.Remove(entity);
                await _ctx.SaveChangesAsync(ct);
            }
        }

        public Task<bool> ExistsByNameAsync(int versionId, string colorName, int? excludeId = null, CancellationToken ct = default)
        {
            var q = _ctx.Colors.AsQueryable()
                    .Where(c => c.VersionId == versionId && c.ColorName == colorName);
            if (excludeId.HasValue) q = q.Where(c => c.ColorId != excludeId.Value);
            return q.AnyAsync(ct);
        }
    }
}
