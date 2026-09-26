namespace LibraryManagement.Application.DTOs;

public record BorrowRecordDto(
    int Id,
    DateTime BorrowDate,
    DateTime DueDate,
    DateTime? ReturnDate,
    int BookId,
    string BookTitle,
    string BookISBN,
    int MemberId,
    string MemberFirstName,
    string MemberLastName,
    string? Notes,
    bool IsOverdue
);

public record BorrowBookRequestDto(
    int MemberId,
    int BookId,
    string? Notes
);

public record ReturnBookRequestDto(
    int BorrowRecordId,
    string? Notes
);