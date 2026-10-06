using LibraryManagement.Application.Common.Interfaces.Persistence;
using LibraryManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Infrastructure.Persistence.Repositories;

public sealed class CategoryRepository : ICategoryRepository
{
    private readonly DbSet<Category> DbSet;

    public CategoryRepository(LibraryDbContext context)
    {
        DbSet = context.Set<Category>();
    }

    public async Task<Category?> GetByIdAsync(
        int id,
        CancellationToken cancellationToken = default
    ) => await DbSet.FirstOrDefaultAsync(category => category.Id == id, cancellationToken);

    public async Task<IEnumerable<Category>> GetAllAsync(
        CancellationToken cancellationToken = default
    ) => await DbSet.AsNoTracking().ToListAsync(cancellationToken);

    public async Task AddAsync(Category entity, CancellationToken cancellationToken = default) =>
        await DbSet.AddAsync(entity, cancellationToken);

    public Task UpdateAsync(Category entity, CancellationToken cancellationToken = default)
    {
        DbSet.Update(entity);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(Category entity, CancellationToken cancellationToken = default)
    {
        DbSet.Remove(entity);
        return Task.CompletedTask;
    }

    public Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default) =>
        DbSet.AnyAsync(category => EF.Property<int>(category, "Id") == id, cancellationToken);

    public async Task<IEnumerable<Category>> SearchAsync(
        string name,
        CancellationToken cancellationToken = default
    )
    {
        var categories = await DbSet.AsNoTracking().ToListAsync(cancellationToken);

        return FuzzySearch.Rank(categories, name, category => [category.Name]);
    }
}
