namespace LibraryManagement.Application.DTOs;

using LibraryManagement.Domain.Enums;

public record BookCopyDto(
    int Id,
    int BookId,
    string BookTitle,
    string BookISBN,
    BookStatus Status,
    DateTime DateAdded,
    bool IsAvailable
);

// // Used by librarians and admin
public record AddBookCopiesDto(
    int Count,
    BookStatus Status = BookStatus.Available
);

// Used by librarians and admin
public record ChangeBookCopyStatusDto(
    BookStatus Status
);