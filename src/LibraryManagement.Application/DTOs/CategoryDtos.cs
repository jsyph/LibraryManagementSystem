namespace LibraryManagement.Application.DTOs;

public record CategoryDto(
    int Id,
    string Name,
    string? Description
);

// Used by librarians and admin
public record CreateCategoryDto(
    string Name,
    string? Description
);

// Used by librarians and admin
public record UpdateCategoryDto(
    string Name,
    string? Description
);

// Used by librarians and admin and user
public record SearchCategoryDto(
    string? Name,
    string? Description
);