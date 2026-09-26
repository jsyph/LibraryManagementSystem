namespace LibraryManagement.Application.DTOs;

using LibraryManagement.Domain.Enums;

public record BookDto(
    int Id,
    string Title,
    string ISBN,
    string? Description,
    int PublicationYear,
    BookStatus Status,
    int AuthorId,
    string AuthorName,
    int CategoryId,
    string CategoryName
);

public record CreateBookDto(
    string Title,
    string ISBN,
    string? Description,
    int PublicationYear,
    int AuthorId,
    int CategoryId
);

public record UpdateBookDto(
    string Title,
    string ISBN,
    string? Description,
    int PublicationYear,
    int AuthorId,
    int CategoryId
);