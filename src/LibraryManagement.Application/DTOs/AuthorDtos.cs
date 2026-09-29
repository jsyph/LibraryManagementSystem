namespace LibraryManagement.Application.DTOs;

// Used for returning a author's data from operations 
public record AuthorDto(
    int Id,
    string FirstName,
    string LastName,
    string? Biography,
    string? Nationality
);

// Used by librarians and admin
public record CreateAuthorDto(
    string FirstName,
    string LastName,
    string? Biography,
    string? Nationality
);

// Used by librarians and admin
public record UpdateAuthorDto(
    string FirstName,
    string LastName,
    string? Biography,
    string? Nationality
);

// Used by librarians and admin and user
public record SearchAuthorDto(
    string? FirstName,
    string? LastName,
    string? Nationality
);