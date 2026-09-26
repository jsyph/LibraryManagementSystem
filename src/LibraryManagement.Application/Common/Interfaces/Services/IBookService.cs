namespace LibraryManagement.Application.Common.Interfaces.Services;

using LibraryManagement.Application.DTOs;

public interface IBookService
{
    Task<BookDto> GetByIdAsync(int id, CancellationToken ct = default);
    Task<IEnumerable<BookDto>> GetAllAsync(CancellationToken ct = default);
    Task<BookDto> CreateAsync(CreateBookDto dto, CancellationToken ct = default);
    Task UpdateAsync(int id, UpdateBookDto dto, CancellationToken ct = default);
    Task DeleteAsync(int id, CancellationToken ct = default);

    Task<IEnumerable<BookDto>> SearchAsync(
        string? searchTerm = null,
        string? title = null,
        string? authorName = null,
        int? authorId = null,
        string? isbn = null,
        int? categoryId = null,
        CancellationToken ct = default);

    Task<bool> IsAvailableAsync(int bookId, CancellationToken ct = default);

    Task<IEnumerable<BorrowRecordDto>> GetBorrowHistoryAsync(int bookId, CancellationToken ct = default);
}