namespace LibraryManagement.Application.Common.Interfaces.Persistence;

using LibraryManagement.Domain.Entities;

public interface IBorrowRecordRepository
{
    Task<BorrowRecord?> GetByIdAsync(int id);
    Task<BorrowRecord?> GetActiveRecordByBookCopyIdAsync(int bookCopyId);
    Task<IEnumerable<BorrowRecord>> GetBorrowHistoryByBookCopyIdAsync(int bookCopyId);
    Task<IEnumerable<BorrowRecord>> GetActiveRecordsByUserIdAsync(int userId);
    Task<IEnumerable<BorrowRecord>> GetAllRecordsByUserIdAsync(int userId);
    Task<IEnumerable<BorrowRecord>> GetOverdueRecordsAsync();
    Task<IEnumerable<BorrowRecord>> GetAllAsync();
    Task AddAsync(BorrowRecord record);
    Task UpdateAsync(BorrowRecord record);
    Task DeleteAsync(int id);
}