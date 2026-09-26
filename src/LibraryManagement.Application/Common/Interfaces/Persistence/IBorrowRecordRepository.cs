namespace LibraryManagement.Application.Common.Interfaces.Persistence;

using LibraryManagement.Domain.Entities;

public interface IBorrowRecordRepository
{
    Task<BorrowRecord?> GetByIdAsync(int id);
    Task<BorrowRecord?> GetActiveRecordByBookIdAsync(int bookId);
    Task<IEnumerable<BorrowRecord>> GetActiveRecordsByMemberIdAsync(int memberId);
    Task<IEnumerable<BorrowRecord>> GetAllRecordsByMemberIdAsync(int memberId);
    Task<IEnumerable<BorrowRecord>> GetOverdueRecordsAsync();
    Task<IEnumerable<BorrowRecord>> GetAllAsync();
    Task AddAsync(BorrowRecord record);
    Task UpdateAsync(BorrowRecord record);
}