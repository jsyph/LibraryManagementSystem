namespace LibraryManagement.Application.DTOs;

using LibraryManagement.Domain.Enums;

public record UserDto(
    int Id,
    string FirstName,
    string LastName,
    string Email,
    string PhoneNumber,
    int RoleId,
    string RoleName,
    bool IsActive,
    DateTime DateAdded
);

// Used by librarians and admin
public record CreateUserDto(
    string FirstName,
    string LastName,
    string Email,
    string PhoneNumber,
    string PasswordHash
);


// Used by librarians and admin and user
public record UpdateUserDto(
    string FirstName,
    string LastName,
    string Email,
    string PhoneNumber
);

// Used by librarians, admin and User
public record ChangeUserPasswordDto(
    string PasswordHash
);

// Used by admin
public record ChangeUserRoleDto(
    int RoleId
);

// Used by admin
public record ActivateUserDto(
    int UserId
);

// Used by  admin
public record DeactivateUserDto(
    int UserId
);

// Used by librarians and admin
public record SearchUserDto(
    string? FirstName,
    string? LastName,
    string? Email,
    string? PhoneNumber
);