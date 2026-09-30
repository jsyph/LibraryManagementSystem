namespace LibraryManagement.Application.Common.Interfaces.Persistence;

using LibraryManagement.Domain.Entities;

public interface IBorrowRecordRepository
{
    Task<BorrowRecord?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<BorrowRecord?> GetActiveRecordByBookCopyIdAsync(
        int bookCopyId,
        CancellationToken cancellationToken = default
    );
    Task<IEnumerable<BorrowRecord>> GetBorrowHistoryByBookCopyIdAsync(
        int bookCopyId,
        CancellationToken cancellationToken = default
    );
    Task<IEnumerable<BorrowRecord>> GetActiveRecordsByUserIdAsync(
        int userId,
        CancellationToken cancellationToken = default
    );
    Task<IEnumerable<BorrowRecord>> GetAllRecordsByUserIdAsync(
        int userId,
        CancellationToken cancellationToken = default
    );
    Task<IEnumerable<BorrowRecord>> GetOverdueRecordsAsync(
        CancellationToken cancellationToken = default
    );
    Task<IEnumerable<BorrowRecord>> GetAllAsync(CancellationToken cancellationToken = default);
    Task AddAsync(BorrowRecord record, CancellationToken cancellationToken = default);
    Task UpdateAsync(BorrowRecord record, CancellationToken cancellationToken = default);
    Task DeleteAsync(int id, CancellationToken cancellationToken = default);
}
