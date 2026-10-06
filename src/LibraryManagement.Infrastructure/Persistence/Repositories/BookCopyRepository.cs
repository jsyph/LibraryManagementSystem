using LibraryManagement.Application.Common.Interfaces.Persistence;
using LibraryManagement.Domain.Entities;
using LibraryManagement.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Infrastructure.Persistence.Repositories;

public sealed class BookCopyRepository : IBookCopyRepository
{
    private readonly DbSet<BookCopy> DbSet;

    public BookCopyRepository(LibraryDbContext context)
    {
        DbSet = context.Set<BookCopy>();
    }

    public async Task<BookCopy?> GetByIdAsync(
        int id,
        CancellationToken cancellationToken = default
    ) =>
        await DbSet
            .Include(copy => copy.Book)
            .FirstOrDefaultAsync(copy => copy.Id == id, cancellationToken);

    public async Task<IEnumerable<BookCopy>> GetAllAsync(
        CancellationToken cancellationToken = default
    ) => await DbSet.AsNoTracking().Include(copy => copy.Book).ToListAsync(cancellationToken);

    public async Task<IEnumerable<BookCopy>> GetAllByBookIdAsync(
        int bookId,
        CancellationToken cancellationToken = default
    ) =>
        await DbSet
            .AsNoTracking()
            .Include(copy => copy.Book)
            .Where(copy => copy.BookId == bookId)
            .ToListAsync(cancellationToken);

    public async Task<IEnumerable<BookCopy>> GetByStatusAsync(
        BookStatus bookStatus,
        CancellationToken cancellationToken = default
    ) =>
        await DbSet
            .AsNoTracking()
            .Include(copy => copy.Book)
            .Where(copy => copy.Status == bookStatus)
            .ToListAsync(cancellationToken);

    public async Task<bool> IsAvailableAsync(
        int id,
        CancellationToken cancellationToken = default
    ) =>
        await DbSet.AnyAsync(
            copy => copy.Id == id && copy.Status == BookStatus.Available,
            cancellationToken
        );

    public async Task AddAsync(BookCopy entity, CancellationToken cancellationToken = default) =>
        await DbSet.AddAsync(entity, cancellationToken);

    public Task UpdateAsync(BookCopy entity, CancellationToken cancellationToken = default)
    {
        DbSet.Update(entity);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(BookCopy entity, CancellationToken cancellationToken = default)
    {
        DbSet.Remove(entity);
        return Task.CompletedTask;
    }

    public Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default) =>
        DbSet.AnyAsync(copy => EF.Property<int>(copy, "Id") == id, cancellationToken);
}
