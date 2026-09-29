namespace LibraryManagement.Application.DTOs;

public record BorrowRecordDto(
    int Id,
    DateTime BorrowDate,
    DateTime DueDate,
    DateTime? ReturnDate,
    int BookId,
    int BookCopyId,
    string BookTitle,
    string BookISBN,
    int UserId,
    string UserFirstName,
    string UserLastName,
    string? Notes,
    bool IsOverdue
);

// Used by librarians and admin
public record BorrowBookRequestDto(
    int BookCopyId,
    string? Notes
);

// Used by librarians and admin
public record ReturnBookRequestDto(
    string? Notes
);