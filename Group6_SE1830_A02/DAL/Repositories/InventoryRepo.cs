using DAL.Entities;
using DAL.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public class InventoryRepo : IInventoryRepo
    {
        private readonly DBContext _ctx;
        public InventoryRepo(DBContext ctx) => _ctx = ctx;

        public Task<List<Inventory>> GetAllAsync(CancellationToken ct = default) =>
            _ctx.Inventories.AsNoTracking()
                .OrderBy(i => i.VersionId).ThenBy(i => i.ColorId)
                .ToListAsync(ct);

        public Task<Inventory?> GetByIdAsync(int id, CancellationToken ct = default) =>
            _ctx.Inventories.FirstOrDefaultAsync(i => i.InventoryId == id, ct);

        public async Task<Inventory> AddAsync(Inventory entity, CancellationToken ct = default)
        {
            _ctx.Inventories.Add(entity);
            await _ctx.SaveChangesAsync(ct);
            return entity;
        }

        public async Task UpdateAsync(Inventory entity, CancellationToken ct = default)
        {
            _ctx.Inventories.Update(entity);
            await _ctx.SaveChangesAsync(ct);
        }

        public async Task DeleteAsync(int id, CancellationToken ct = default)
        {
            var e = await _ctx.Inventories.FindAsync(new object?[] { id }, ct);
            if (e != null)
            {
                _ctx.Inventories.Remove(e);
                await _ctx.SaveChangesAsync(ct);
            }
        }

        public Task<Inventory?> GetByVersionColorAsync(int versionId, int colorId, CancellationToken ct = default) =>
            _ctx.Inventories.FirstOrDefaultAsync(i => i.VersionId == versionId && i.ColorId == colorId, ct);

        // NEW
        public async Task DeleteByColorAsync(int colorId, CancellationToken ct = default)
        {
            var items = await _ctx.Inventories.Where(i => i.ColorId == colorId).ToListAsync(ct);
            if (items.Count == 0) return;
            _ctx.Inventories.RemoveRange(items);
            await _ctx.SaveChangesAsync(ct);
        }
    }
}
