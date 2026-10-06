using LibraryManagement.Application.Common.Interfaces.Persistence;
using LibraryManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Infrastructure.Persistence.Repositories;

public sealed class BookRepository : IBookRepository
{
    private readonly DbSet<Book> DbSet;

    public BookRepository(LibraryDbContext context)
    {
        DbSet = context.Set<Book>();
    }

    public async Task<Book?> GetByIdAsync(int id, CancellationToken cancellationToken = default) =>
        await DbSet
            .Include(book => book.Author)
            .Include(book => book.Category)
            .Include(book => book.Copies)
            .FirstOrDefaultAsync(book => book.Id == id, cancellationToken);

    public async Task<IEnumerable<Book>> GetAllAsync(
        CancellationToken cancellationToken = default
    ) =>
        await DbSet
            .AsNoTracking()
            .Include(book => book.Author)
            .Include(book => book.Category)
            .Include(book => book.Copies)
            .ToListAsync(cancellationToken);

    public async Task<IEnumerable<Book>> SearchAsync(
        string searchTerm,
        CancellationToken cancellationToken = default
    )
    {
        var books = await DbSet
            .AsNoTracking()
            .Include(book => book.Author)
            .Include(book => book.Category)
            .Include(book => book.Copies)
            .ToListAsync(cancellationToken);

        return FuzzySearch.Rank(books, searchTerm, book => [book.Title, book.Description]);
    }

    public async Task<Book?> GetByIsbnAsync(
        string isbn,
        CancellationToken cancellationToken = default
    ) =>
        await DbSet
            .Include(book => book.Author)
            .Include(book => book.Category)
            .Include(book => book.Copies)
            .FirstOrDefaultAsync(book => book.ISBN == isbn, cancellationToken);

    public async Task<IEnumerable<Book>> GetByCategoryIdAsync(
        int categoryId,
        CancellationToken cancellationToken = default
    ) =>
        await DbSet
            .AsNoTracking()
            .Include(book => book.Author)
            .Include(book => book.Category)
            .Include(book => book.Copies)
            .Where(book => book.CategoryId == categoryId)
            .ToListAsync(cancellationToken);

    public async Task<IEnumerable<Book>> GetByAuthorIdAsync(
        int authorId,
        CancellationToken cancellationToken = default
    ) =>
        await DbSet
            .AsNoTracking()
            .Include(book => book.Author)
            .Include(book => book.Category)
            .Include(book => book.Copies)
            .Where(book => book.AuthorId == authorId)
            .ToListAsync(cancellationToken);

    public async Task<bool> IsIsbnUniqueAsync(
        string isbn,
        CancellationToken cancellationToken = default
    ) => !await DbSet.AnyAsync(book => book.ISBN == isbn, cancellationToken);

    public async Task AddAsync(Book entity, CancellationToken cancellationToken = default) =>
        await DbSet.AddAsync(entity, cancellationToken);

    public Task UpdateAsync(Book entity, CancellationToken cancellationToken = default)
    {
        DbSet.Update(entity);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(Book entity, CancellationToken cancellationToken = default)
    {
        DbSet.Remove(entity);
        return Task.CompletedTask;
    }

    public Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default) =>
        DbSet.AnyAsync(book => EF.Property<int>(book, "Id") == id, cancellationToken);
}
