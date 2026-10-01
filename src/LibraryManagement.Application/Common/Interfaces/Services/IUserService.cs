namespace LibraryManagement.Application.Common.Interfaces.Services;

using LibraryManagement.Application.DTOs;

public interface IUserService
{
    Task<IEnumerable<UserDto>> SearchUsersAsync(
        SearchUserDto dto,
        CancellationToken cancellationToken = default
    );

    Task<MemberDto?> GetMemberByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<MemberDto>> GetAllMembersAsync(CancellationToken cancellationToken = default);
    Task<MemberDto> AddMemberAsync(
        CreateMemberDto dto,
        CancellationToken cancellationToken = default
    );
    Task UpdateMemberAsync(
        int id,
        UpdateMemberDto dto,
        CancellationToken cancellationToken = default
    );
    Task DeleteMemberAsync(int id, CancellationToken cancellationToken = default);

    Task<LibrarianDto?> GetLibrarianByIdAsync(
        int id,
        CancellationToken cancellationToken = default
    );
    Task<IEnumerable<LibrarianDto>> GetAllLibrariansAsync(
        CancellationToken cancellationToken = default
    );
    Task<LibrarianDto> AddLibrarianAsync(
        CreateLibrarianDto dto,
        CancellationToken cancellationToken = default
    );
    Task UpdateLibrarianAsync(
        int id,
        UpdateLibrarianDto dto,
        CancellationToken cancellationToken = default
    );
    Task DeleteLibrarianAsync(int id, CancellationToken cancellationToken = default);

    Task<AdminDto?> GetAdminByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<AdminDto>> GetAllAdminsAsync(CancellationToken cancellationToken = default);
    Task<AdminDto> AddAdminAsync(CreateAdminDto dto, CancellationToken cancellationToken = default);
    Task UpdateAdminAsync(
        int id,
        UpdateAdminDto dto,
        CancellationToken cancellationToken = default
    );

    Task ChangeUserPasswordHashAsync(
        int id,
        ChangeUserPasswordHashDto dto,
        CancellationToken cancellationToken = default
    );
    Task ActivateUserAsync(int id, CancellationToken cancellationToken = default);
    Task DeactivateUserAsync(int id, CancellationToken cancellationToken = default);
    Task RenewMembershipAsync(
        RenewMembershipDto dto,
        CancellationToken cancellationToken = default
    );
}
