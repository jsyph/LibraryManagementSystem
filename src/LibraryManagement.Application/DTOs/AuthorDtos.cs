namespace LibraryManagement.Application.DTOs;

public record AuthorDto(
    int Id,
    string FirstName,
    string LastName,
    string? Biography,
    string? Nationality
);

public record CreateAuthorDto(
    string FirstName,
    string LastName,
    string? Biography,
    string? Nationality
);

public record UpdateAuthorDto(
    string FirstName,
    string LastName,
    string? Biography,
    string? Nationality
);