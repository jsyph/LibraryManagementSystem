namespace LibraryManagement.Application.Common.Interfaces.Services;

using LibraryManagement.Application.DTOs;

public interface IMemberService
{
    // Queries
    Task<MemberDto> GetByIdAsync(int id, CancellationToken ct = default);
    Task<IEnumerable<MemberDto>> GetAllAsync(CancellationToken ct = default);
    Task<MemberDto?> GetByEmailAsync(string email, CancellationToken ct = default);
    Task<MemberDto?> GetByPhoneAsync(string phone, CancellationToken ct = default);
    Task<MemberDto?> GetByNationalIdAsync(string nationalId, CancellationToken ct = default);

    Task<MemberDto> CreateAsync(CreateMemberDto dto, CancellationToken ct = default);
    Task UpdateAsync(int id, UpdateMemberDto dto, CancellationToken ct = default);
    Task DeleteAsync(int id, CancellationToken ct = default);

    Task BanAsync(int id, BanMemberDto dto, CancellationToken ct = default);
    Task UnbanAsync(int id, CancellationToken ct = default);
    Task ActivateAsync(int id, CancellationToken ct = default);
    Task DeactivateAsync(int id, CancellationToken ct = default);
    Task RenewMembershipAsync(int id, RenewMembershipDto dto, CancellationToken ct = default);

    // Borrowing History & Active Loans
    Task<IEnumerable<BorrowRecordDto>> GetBorrowHistoryAsync(int memberId, CancellationToken ct = default);
    Task<IEnumerable<BorrowRecordDto>> GetActiveBorrowsAsync(int memberId, CancellationToken ct = default);
}