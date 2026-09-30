namespace LibraryManagement.Application.DTOs;

using LibraryManagement.Domain.Enums;

#region DTOs
public abstract record UserDto(
    int Id,
    string FirstName,
    string LastName,
    string Email,
    string PhoneNumber,
    bool IsActive,
    DateTime DateAdded,
    UserRole Role
);

public record MemberDto(
    int Id,
    string FirstName,
    string LastName,
    string Email,
    string PhoneNumber,
    bool IsActive,
    DateTime DateAdded,

    DateTime MembershipStartDate,
    DateTime MembershipExpiryDate,
    int TotalBorrowRecords

) : UserDto(Id, FirstName, LastName, Email, PhoneNumber, IsActive, DateAdded, UserRole.Member);

public record LibrarianDto(
    int Id,
    string FirstName,
    string LastName,
    string Email,
    string PhoneNumber,
    bool IsActive,
    DateTime DateAdded,

    DateTime HireDate,
    string Department

) : UserDto(Id, FirstName, LastName, Email, PhoneNumber, IsActive, DateAdded, UserRole.Librarian);

public record AdminDto(
    int Id,
    string FirstName,
    string LastName,
    string Email,
    string PhoneNumber,
    bool IsActive,
    DateTime DateAdded
) : UserDto(Id, FirstName, LastName, Email, PhoneNumber, IsActive, DateAdded, UserRole.Admin);

#endregion

#region Creation DTOs
public abstract record CreateUserDto(
    string FirstName,
    string LastName,
    string Email,
    string PhoneNumber,
    string PasswordHash
);

public record CreateMemberDto(
    string FirstName,
    string LastName,
    string Email,
    string PhoneNumber,
    string PasswordHash,
    int MembershipDurationMonths
) : CreateUserDto(FirstName, LastName, Email, PhoneNumber, PasswordHash);

public record CreateLibrarianDto(
    string FirstName,
    string LastName,
    string Email,
    string PhoneNumber,
    string PasswordHash,
    string Department,
    DateTime HireDate
) : CreateUserDto(FirstName, LastName, Email, PhoneNumber, PasswordHash);

public record CreateAdminDto(
    string FirstName,
    string LastName,
    string Email,
    string PhoneNumber,
    string PasswordHash
) : CreateUserDto(FirstName, LastName, Email, PhoneNumber, PasswordHash);

#endregion

#region update DTOs

public record UpdateUserDto(
    string FirstName,
    string LastName,
    string Email,
    string PhoneNumber
);

public record UpdateMemberDto(
    string FirstName,
    string LastName,
    string Email,
    string PhoneNumber,
    int MembershipDurationMonths
);

public record UpdateLibrarianDto(
    string FirstName,
    string LastName,
    string Email,
    string PhoneNumber,
    string Department
);

public record ChangeUserPasswordHashDto(
    string NewPasswordHash
);

public record ActivateUserDto(
    int UserId
);

public record DeactivateUserDto(
    int UserId
);

public record RenewMembershipDto(
    int UserId,
    int Months
);

#endregion

#region Search DTOs

public record SearchUserDto(
    string? FirstName,
    string? LastName,
    string? Email,
    string? PhoneNumber,
    DateTime? DateAdded,
    UserRole? Role
);

#endregion