namespace LibraryManagement.Application.Common.Interfaces.Persistence;

using LibraryManagement.Domain.Entities;
using LibraryManagement.Domain.Enums;

public interface IBookCopyRepository
{
    Task<BookCopy?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<BookCopy>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<BookCopy>> GetAllByBookIdAsync(
        int bookId,
        CancellationToken cancellationToken = default
    );
    Task<IEnumerable<BookCopy>> GetByStatusAsync(
        BookStatus bookStatus,
        CancellationToken cancellationToken = default
    );
    Task AddAsync(BookCopy bookCopy, CancellationToken cancellationToken = default);
    Task UpdateAsync(BookCopy bookCopy, CancellationToken cancellationToken = default);
    Task DeleteAsync(BookCopy bookCopy, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default);
    Task<bool> IsAvailableAsync(int id, CancellationToken cancellationToken = default);
}
