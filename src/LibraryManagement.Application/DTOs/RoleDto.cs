namespace LibraryManagement.Application.DTOs;

using LibraryManagement.Domain.Enums;

public record RoleDto(
    int Id,
    string RoleName,
    string Description
);

// Used by admin
public record CreateRoleDto(
    string RoleName,
    string Description
);

// Used by admin
public record UpdateRoleDto(
    string RoleName,
    string Description
);