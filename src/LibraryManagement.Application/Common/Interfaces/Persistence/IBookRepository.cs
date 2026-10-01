namespace LibraryManagement.Application.Common.Interfaces.Persistence;

using LibraryManagement.Domain.Entities;

public interface IBookRepository
{
    Task<Book?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<Book>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<Book>> SearchAsync(
        string searchTerm,
        CancellationToken cancellationToken = default
    );

    Task<Book?> GetByIsbnAsync(string isbn, CancellationToken cancellationToken = default);
    Task<IEnumerable<Book>> GetByCategoryIdAsync(
        int categoryId,
        CancellationToken cancellationToken = default
    );
    Task<IEnumerable<Book>> GetByAuthorIdAsync(
        int authorId,
        CancellationToken cancellationToken = default
    );
    Task AddAsync(Book book, CancellationToken cancellationToken = default);
    Task UpdateAsync(Book book, CancellationToken cancellationToken = default);
    Task DeleteAsync(Book book, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default);
    Task<bool> IsIsbnUniqueAsync(string isbn, CancellationToken cancellationToken = default);
}
