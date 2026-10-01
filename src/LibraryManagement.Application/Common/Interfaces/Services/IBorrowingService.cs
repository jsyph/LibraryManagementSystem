namespace LibraryManagement.Application.Common.Interfaces.Services;

using LibraryManagement.Application.DTOs;

public interface IBorrowingService
{
    Task<BorrowRecordDto?> GetRecordByIdAsync(
        int id,
        CancellationToken cancellationToken = default
    );
    Task<BorrowRecordDto?> GetActiveRecordByBookCopyIdAsync(
        int bookCopyId,
        CancellationToken cancellationToken = default
    );
    Task<IEnumerable<BorrowRecordDto>> GetAllRecordsByBookCopyIdAsync(
        int bookCopyId,
        CancellationToken cancellationToken = default
    );
    Task<IEnumerable<BorrowRecordDto>> GetActiveRecordsByUserIdAsync(
        int userId,
        CancellationToken cancellationToken = default
    );
    Task<IEnumerable<BorrowRecordDto>> GetAllRecordsByUserIdAsync(
        int userId,
        CancellationToken cancellationToken = default
    );
    Task<IEnumerable<BorrowRecordDto>> GetAllOverdueRecordsAsync(
        CancellationToken cancellationToken = default
    );
    Task<BorrowRecordDto> BorrowBookAsync(
        int bookCopyId,
        CreateBorrowRecordDto dto,
        CancellationToken cancellationToken = default
    );
    Task ReturnBorrowedBookAsync(
        int bookCopyId,
        ReturnBorrowedBookDto dto,
        CancellationToken cancellationToken = default
    );
    Task DeleteRecordAsync(int id, CancellationToken cancellationToken = default);
}
