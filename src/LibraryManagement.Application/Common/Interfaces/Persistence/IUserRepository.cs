namespace LibraryManagement.Application.Common.Interfaces.Persistence;

using LibraryManagement.Domain.Entities;
using LibraryManagement.Domain.Enums;

public interface IUserRepository
{
    Task<User?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
    Task<User?> GetByPhoneAsync(string phone, CancellationToken cancellationToken = default);
    Task<IEnumerable<User>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<User>> GetByRoleAsync(
        UserRole role,
        CancellationToken cancellationToken = default
    );

    Task<IEnumerable<User>> SearchAsync(
        string? FirstName,
        string? LastName,
        string? Email,
        string? PhoneNumber,
        bool? IsActive,
        DateTime? DateAdded,
        UserRole? Role
    );

    Task<Member?> GetMemberByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<Member?> GetMemberWithBorrowHistoryAsync(
        int id,
        CancellationToken cancellationToken = default
    );
    Task<IEnumerable<Member>> GetAllMembersAsync(CancellationToken cancellationToken = default);

    Task<Librarian?> GetLibrarianByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<Librarian>> GetAllLibrariansAsync(
        CancellationToken cancellationToken = default
    );

    Task<Admin?> GetAdminByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<Admin>> GetAllAdminsAsync(CancellationToken cancellationToken = default);

    Task AddAsync(User user, CancellationToken cancellationToken = default);
    Task UpdateAsync(User user, CancellationToken cancellationToken = default);
    Task DeleteAsync(User user, CancellationToken cancellationToken = default);

    Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default);
    Task<bool> EmailExistsAsync(string email, CancellationToken cancellationToken = default);
    Task<bool> PhoneExistsAsync(string phone, CancellationToken cancellationToken = default);
}
