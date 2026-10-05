namespace LibraryManagement.Application.Common.Interfaces.Services;

using LibraryManagement.Application.DTOs;

public interface IBookService
{
    Task<BookDto?> GetBookByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<BookDto>> GetAllBooksAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<BookDto>> SearchBooksAsync(
        TitleDescriptionBookSearchFilterDto dto,
        CancellationToken cancellationToken = default
    );
    Task<IEnumerable<BookDto>> GetBooksByAuthorIdAsync(
        int authorId,
        CancellationToken cancellationToken = default
    );
    Task<IEnumerable<BookDto>> GetBooksByCategory(
        int categoryId,
        CancellationToken cancellationToken = default
    );
    Task<BookDto?> GetBookByIsbnAsync(string isbn, CancellationToken cancellationToken = default);
    Task<BookDto> AddBookAsync(CreateBookDto dto, CancellationToken cancellationToken = default);
    Task UpdateBookAsync(int id, UpdateBookDto dto, CancellationToken cancellationToken = default);
    Task DeleteBookAsync(int id, CancellationToken cancellationToken = default);
}
