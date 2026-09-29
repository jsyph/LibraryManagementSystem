namespace LibraryManagement.Application.DTOs;

public record BookDto(
    int Id,
    string Title,
    string ISBN,
    string Language,
    string? Description,
    int PublicationYear,
    int AuthorId,
    string AuthorName,
    int CategoryId,
    string CategoryName,
    int TotalCopyCount,
    int AvailableCopyCount,
    DateTime DateAdded
);

// Used by librarians and admin
public record CreateBookDto(
    string Title,
    string ISBN,
    string? Description,
    int PublicationYear,
    int AuthorId,
    int CategoryId,
    string Language = "en"
);

// Used by librarians and admin
public record UpdateBookDto(
    string Title,
    string ISBN,
    string? Description,
    int PublicationYear,
    int AuthorId,
    int CategoryId,
    string Language = "en"
);

// Used by librarians and admin and user
public record BookSearchFilterDto(
    string? Title,
    string? ISBN,
    int? AuthorId,
    int? CategoryId
);
