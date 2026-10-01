namespace LibraryManagement.Application.Services;

using LibraryManagement.Application.Common.Exceptions;
using LibraryManagement.Application.Common.Interfaces.Persistence;
using LibraryManagement.Application.Common.Interfaces.Services;
using LibraryManagement.Application.DTOs;
using LibraryManagement.Domain.Entities;
using LibraryManagement.Domain.Enums;

public class UserService : IUserService
{
    private readonly IUnitOfWork unitOfWork;

    public UserService(IUnitOfWork unitOfWork)
    {
        this.unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<UserDto>> SearchUsersAsync(
        SearchUserDto dto,
        CancellationToken cancellationToken = default
    )
    {
        var users = await unitOfWork.Users.SearchAsync(
            dto.FirstName,
            dto.LastName,
            dto.Email,
            dto.PhoneNumber,
            dto.IsActive,
            dto.DateAdded,
            dto.Role,
            cancellationToken
        );

        return users.Select(MapToDto).ToList();
    }

    public async Task<MemberDto?> GetMemberByIdAsync(
        int id,
        CancellationToken cancellationToken = default
    )
    {
        var member = await GetUserByRoleAsync<Member>(id, UserRole.Member, cancellationToken);
        return member is null ? null : MapToDto(member);
    }

    public async Task<IEnumerable<MemberDto>> GetAllMembersAsync(
        CancellationToken cancellationToken = default
    )
    {
        var members = await unitOfWork.Users.GetByRoleAsync(UserRole.Member, cancellationToken);
        return members.OfType<Member>().Select(MapToDto).ToList();
    }

    public async Task<MemberDto> AddMemberAsync(
        CreateMemberDto dto,
        CancellationToken cancellationToken = default
    )
    {
        ValidateMembershipDuration(dto.MembershipDurationMonths);
        var startDate = DateTime.UtcNow;
        var member = new Member
        {
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Email = dto.Email,
            PhoneNumber = dto.PhoneNumber,
            PasswordHash = dto.PasswordHash,
            MembershipStartDate = startDate,
            MembershipExpiryDate = startDate.AddMonths(dto.MembershipDurationMonths),
        };

        await unitOfWork.Users.AddAsync(member, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return MapToDto(member);
    }

    public async Task UpdateMemberAsync(
        int id,
        UpdateMemberDto dto,
        CancellationToken cancellationToken = default
    )
    {
        var member = await GetRequiredUserByRoleAsync<Member>(
            id,
            UserRole.Member,
            cancellationToken
        );
        member.FirstName = dto.FirstName;
        member.LastName = dto.LastName;
        member.Email = dto.Email;
        member.PhoneNumber = dto.PhoneNumber;

        await unitOfWork.Users.UpdateAsync(member, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public Task DeleteMemberAsync(int id, CancellationToken cancellationToken = default) =>
        DeleteUserAsync<Member>(id, UserRole.Member, cancellationToken);

    public async Task<LibrarianDto?> GetLibrarianByIdAsync(
        int id,
        CancellationToken cancellationToken = default
    )
    {
        var librarian = await GetUserByRoleAsync<Librarian>(
            id,
            UserRole.Librarian,
            cancellationToken
        );
        return librarian is null ? null : MapToDto(librarian);
    }

    public async Task<IEnumerable<LibrarianDto>> GetAllLibrariansAsync(
        CancellationToken cancellationToken = default
    )
    {
        var librarians = await unitOfWork.Users.GetByRoleAsync(
            UserRole.Librarian,
            cancellationToken
        );
        return librarians.OfType<Librarian>().Select(MapToDto).ToList();
    }

    public async Task<LibrarianDto> AddLibrarianAsync(
        CreateLibrarianDto dto,
        CancellationToken cancellationToken = default
    )
    {
        var librarian = new Librarian
        {
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Email = dto.Email,
            PhoneNumber = dto.PhoneNumber,
            PasswordHash = dto.PasswordHash,
            Department = dto.Department,
            HireDate = dto.HireDate,
        };

        await unitOfWork.Users.AddAsync(librarian, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return MapToDto(librarian);
    }

    public async Task UpdateLibrarianAsync(
        int id,
        UpdateLibrarianDto dto,
        CancellationToken cancellationToken = default
    )
    {
        var librarian = await GetRequiredUserByRoleAsync<Librarian>(
            id,
            UserRole.Librarian,
            cancellationToken
        );
        librarian.FirstName = dto.FirstName;
        librarian.LastName = dto.LastName;
        librarian.Email = dto.Email;
        librarian.PhoneNumber = dto.PhoneNumber;
        librarian.Department = dto.Department;

        await unitOfWork.Users.UpdateAsync(librarian, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public Task DeleteLibrarianAsync(int id, CancellationToken cancellationToken = default) =>
        DeleteUserAsync<Librarian>(id, UserRole.Librarian, cancellationToken);

    public async Task<AdminDto?> GetAdminByIdAsync(
        int id,
        CancellationToken cancellationToken = default
    )
    {
        var admin = await GetUserByRoleAsync<Admin>(id, UserRole.Admin, cancellationToken);
        return admin is null ? null : MapToDto(admin);
    }

    public async Task<IEnumerable<AdminDto>> GetAllAdminsAsync(
        CancellationToken cancellationToken = default
    )
    {
        var admins = await unitOfWork.Users.GetByRoleAsync(UserRole.Admin, cancellationToken);
        return admins.OfType<Admin>().Select(MapToDto).ToList();
    }

    public async Task<AdminDto> AddAdminAsync(
        CreateAdminDto dto,
        CancellationToken cancellationToken = default
    )
    {
        var admin = new Admin
        {
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Email = dto.Email,
            PhoneNumber = dto.PhoneNumber,
            PasswordHash = dto.PasswordHash,
        };

        await unitOfWork.Users.AddAsync(admin, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return MapToDto(admin);
    }

    public async Task UpdateAdminAsync(
        int id,
        UpdateAdminDto dto,
        CancellationToken cancellationToken = default
    )
    {
        var admin = await GetRequiredUserByRoleAsync<Admin>(id, UserRole.Admin, cancellationToken);
        admin.FirstName = dto.FirstName;
        admin.LastName = dto.LastName;
        admin.Email = dto.Email;
        admin.PhoneNumber = dto.PhoneNumber;

        await unitOfWork.Users.UpdateAsync(admin, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task ChangeUserPasswordHashAsync(
        int id,
        ChangeUserPasswordHashDto dto,
        CancellationToken cancellationToken = default
    )
    {
        var user = await GetRequiredUserAsync(id, cancellationToken);
        user.PasswordHash = dto.NewPasswordHash;
        await unitOfWork.Users.UpdateAsync(user, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public Task ActivateUserAsync(int id, CancellationToken cancellationToken = default) =>
        SetUserActiveStateAsync(id, true, cancellationToken);

    public Task DeactivateUserAsync(int id, CancellationToken cancellationToken = default) =>
        SetUserActiveStateAsync(id, false, cancellationToken);

    public async Task RenewMembershipAsync(
        RenewMembershipDto dto,
        CancellationToken cancellationToken = default
    )
    {
        ValidateMembershipDuration(dto.Months);
        var member = await GetRequiredUserByRoleAsync<Member>(
            dto.UserId,
            UserRole.Member,
            cancellationToken
        );
        var renewalStart =
            member.MembershipExpiryDate is { } expiry && expiry > DateTime.UtcNow
                ? expiry
                : DateTime.UtcNow;
        member.MembershipExpiryDate = renewalStart.AddMonths(dto.Months);
        member.IsActive = true;

        await unitOfWork.Users.UpdateAsync(member, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }

    private async Task<TUser?> GetUserByRoleAsync<TUser>(
        int id,
        UserRole role,
        CancellationToken cancellationToken
    )
        where TUser : User
    {
        var user = await unitOfWork.Users.GetByIdAsync(id, cancellationToken);
        return user is TUser && user.Role == role ? (TUser)user : null;
    }

    private async Task<TUser> GetRequiredUserByRoleAsync<TUser>(
        int id,
        UserRole role,
        CancellationToken cancellationToken
    )
        where TUser : User
    {
        var user = await GetUserByRoleAsync<TUser>(id, role, cancellationToken);
        return user ?? throw new NotFoundException(nameof(TUser), id);
    }

    private async Task<User> GetRequiredUserAsync(int id, CancellationToken cancellationToken)
    {
        return await unitOfWork.Users.GetByIdAsync(id, cancellationToken)
            ?? throw new NotFoundException(nameof(User), id);
    }

    private async Task DeleteUserAsync<TUser>(
        int id,
        UserRole role,
        CancellationToken cancellationToken
    )
        where TUser : User
    {
        var user = await GetRequiredUserByRoleAsync<TUser>(id, role, cancellationToken);
        await unitOfWork.Users.DeleteAsync(user, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }

    private async Task SetUserActiveStateAsync(
        int id,
        bool isActive,
        CancellationToken cancellationToken
    )
    {
        var user = await GetRequiredUserAsync(id, cancellationToken);
        user.IsActive = isActive;
        await unitOfWork.Users.UpdateAsync(user, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }

    private static void ValidateMembershipDuration(int months)
    {
        if (months <= 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(months),
                "Months must be greater than zero."
            );
        }
    }

    private static UserDto MapToDto(User user) =>
        user switch
        {
            Member member => MapToDto(member),
            Librarian librarian => MapToDto(librarian),
            Admin admin => MapToDto(admin),
            _ => throw new InvalidOperationException(
                $"Unsupported user type: {user.GetType().Name}."
            ),
        };

    private static MemberDto MapToDto(Member member) =>
        new(
            member.Id,
            member.FirstName,
            member.LastName,
            member.Email,
            member.PhoneNumber,
            member.IsActive,
            member.DateAdded,
            member.MembershipStartDate,
            member.MembershipExpiryDate ?? member.MembershipStartDate,
            member.BorrowRecords.Count
        );

    private static LibrarianDto MapToDto(Librarian librarian) =>
        new(
            librarian.Id,
            librarian.FirstName,
            librarian.LastName,
            librarian.Email,
            librarian.PhoneNumber,
            librarian.IsActive,
            librarian.DateAdded,
            librarian.HireDate,
            librarian.Department
        );

    private static AdminDto MapToDto(Admin admin) =>
        new(
            admin.Id,
            admin.FirstName,
            admin.LastName,
            admin.Email,
            admin.PhoneNumber,
            admin.IsActive,
            admin.DateAdded
        );
}
