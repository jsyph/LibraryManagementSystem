using LibraryManagement.Application.Common.Interfaces.Persistence;
using LibraryManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Infrastructure.Persistence.Repositories;

public sealed class AuthorRepository : IAuthorRepository
{
    private readonly DbSet<Author> DbSet;

    public AuthorRepository(LibraryDbContext context)
    {
        DbSet = context.Set<Author>();
    }

    public Task<Author?> GetByIdAsync(int id, CancellationToken cancellationToken = default) =>
        DbSet.FindAsync([id], cancellationToken).AsTask();

    public async Task<IEnumerable<Author>> GetAllAsync(
        CancellationToken cancellationToken = default
    ) => await DbSet.AsNoTracking().ToListAsync(cancellationToken);

    public async Task AddAsync(Author entity, CancellationToken cancellationToken = default) =>
        await DbSet.AddAsync(entity, cancellationToken);

    public Task UpdateAsync(Author entity, CancellationToken cancellationToken = default)
    {
        DbSet.Update(entity);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(Author entity, CancellationToken cancellationToken = default)
    {
        DbSet.Remove(entity);
        return Task.CompletedTask;
    }

    public Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default) =>
        DbSet.AnyAsync(author => EF.Property<int>(author, "Id") == id, cancellationToken);

    public async Task<IEnumerable<Author>> SearchAsync(
        string? firstName,
        string? lastName,
        string? nationality,
        CancellationToken cancellationToken = default
    )
    {
        if (
            string.IsNullOrWhiteSpace(firstName)
            && string.IsNullOrWhiteSpace(lastName)
            && string.IsNullOrWhiteSpace(nationality)
        )
        {
            return Enumerable.Empty<Author>();
        }

        IQueryable<Author> query = DbSet.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(firstName))
        {
            query = query.Where(a => EF.Functions.Like(a.FirstName, $"%{firstName}%"));
        }

        if (!string.IsNullOrWhiteSpace(lastName))
        {
            query = query.Where(a => EF.Functions.Like(a.LastName, $"%{lastName}%"));
        }

        if (!string.IsNullOrWhiteSpace(nationality))
        {
            query = query.Where(a => EF.Functions.Like(a.Nationality, $"%{nationality}%"));
        }

        var candidates = await query.ToListAsync(cancellationToken);

        if (candidates.Count == 0)
        {
            return Enumerable.Empty<Author>();
        }

        IEnumerable<Author> rankedResults = candidates;

        if (!string.IsNullOrWhiteSpace(firstName))
        {
            rankedResults = FuzzySearch.Rank(rankedResults, firstName, a => [a.FirstName]);
        }
        if (!string.IsNullOrWhiteSpace(lastName))
        {
            rankedResults = FuzzySearch.Rank(rankedResults, lastName, a => [a.LastName]);
        }
        if (!string.IsNullOrWhiteSpace(nationality))
        {
            rankedResults = FuzzySearch.Rank(rankedResults, nationality, a => [a.Nationality]);
        }

        return rankedResults;
    }
}
