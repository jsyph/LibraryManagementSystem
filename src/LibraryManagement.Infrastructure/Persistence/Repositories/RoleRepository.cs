using LibraryManagement.Application.Common.Interfaces.Persistence;
using LibraryManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Infrastructure.Persistence.Repositories;

public sealed class RoleRepository : IRoleRepository
{
    private readonly DbSet<Role> DbSet;

    public RoleRepository(LibraryDbContext context)
    {
        DbSet = context.Set<Role>();
    }

    public async Task<Role?> GetByIdAsync(int id, CancellationToken cancellationToken = default) =>
        await DbSet.FirstOrDefaultAsync(role => role.Id == id, cancellationToken);

    public async Task<IEnumerable<Role>> GetAllAsync(
        CancellationToken cancellationToken = default
    ) => await DbSet.AsNoTracking().ToListAsync(cancellationToken);

    public async Task AddAsync(Role entity, CancellationToken cancellationToken = default) =>
        await DbSet.AddAsync(entity, cancellationToken);

    public Task UpdateAsync(Role entity, CancellationToken cancellationToken = default)
    {
        DbSet.Update(entity);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(Role entity, CancellationToken cancellationToken = default)
    {
        DbSet.Remove(entity);
        return Task.CompletedTask;
    }

    public Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default) =>
        DbSet.AnyAsync(role => EF.Property<int>(role, "Id") == id, cancellationToken);

    public async Task<IEnumerable<Role>> SearchAsync(
        string title,
        CancellationToken cancellationToken = default
    )
    {
        var roles = await DbSet.AsNoTracking().ToListAsync(cancellationToken);

        return FuzzySearch.Rank(roles, title, role => [role.RoleName]);
    }
}
