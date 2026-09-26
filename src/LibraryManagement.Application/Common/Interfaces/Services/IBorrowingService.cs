namespace LibraryManagement.Application.Common.Interfaces.Services;

using LibraryManagement.Application.DTOs;

public interface IBorrowingService
{
    Task<BorrowRecordDto> BorrowBookAsync(BorrowBookRequestDto dto, CancellationToken ct = default);
    Task<BorrowRecordDto> ReturnBookAsync(ReturnBookRequestDto dto, CancellationToken ct = default);

    Task<BorrowRecordDto> GetByIdAsync(int id, CancellationToken ct = default);
    Task<IEnumerable<BorrowRecordDto>> GetBookBorrowHistoryAsync(int bookId, CancellationToken ct = default);
    Task<IEnumerable<BorrowRecordDto>> GetMemberBorrowHistoryAsync(int memberId, CancellationToken ct = default);
    Task<IEnumerable<BorrowRecordDto>> GetActiveBorrowsByMemberAsync(int memberId, CancellationToken ct = default);
    Task<IEnumerable<BorrowRecordDto>> GetOverdueBorrowsAsync(CancellationToken ct = default);
}