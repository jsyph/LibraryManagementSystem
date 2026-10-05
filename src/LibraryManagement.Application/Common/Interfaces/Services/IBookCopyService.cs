namespace LibraryManagement.Application.Common.Interfaces.Services;

using LibraryManagement.Application.DTOs;

public interface IBookCopyService
{
    Task<BookCopyDto?> GetBookCopyByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<BookCopyDto>> GetAllBookCopiesAsync(
        int bookId,
        CancellationToken cancellationToken = default
    );
    Task<IEnumerable<BookCopyDto>> GetAllBookCopiesByBookIdAsync(
        int bookId,
        CancellationToken cancellationToken = default
    );
    Task<IEnumerable<BookCopyDto>> GetBookCopiesByStatusAsync(
        int bookStatus,
        CancellationToken cancellationToken = default
    );
    Task<IEnumerable<BookCopyDto>> GetAvilableBookCopiesAsync(
        int bookId,
        CancellationToken cancellationToken = default
    );
    Task<IEnumerable<BookCopyDto>> AddBookCopiesAsync(
        int bookId,
        AddBookCopiesDto dto,
        CancellationToken cancellationToken = default
    );
    Task UpdateBookCopyStatusAsync(
        int id,
        ChangeBookCopyStatusDto dto,
        CancellationToken cancellationToken = default
    );
    Task DeleteBookCopyAsync(int id, CancellationToken cancellationToken = default);
}
