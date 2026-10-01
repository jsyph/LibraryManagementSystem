namespace LibraryManagement.Application.DTOs;

using LibraryManagement.Domain.Enums;

public record PaymentDto(
    int Id,
    int BorrowRecordId,
    decimal Amount,
    PaymentMethod PaymentMethod,
    DateTime PaymentDate,
    DateTime LastStatusChange,
    PaymentStatus Status,
    string? Notes
);

// Used by librarians and admin
public record CreatePaymentDto(
    int BorrowRecordId,
    decimal Amount,
    PaymentMethod PaymentMethod,
    string? Notes
);

// Used by librarians and admin
public record ChangePaymentStatusDto(PaymentStatus Status);

public record SearchPaymentDto(
    PaymentMethod? PaymentMethod,
    DateTime? PaymentDate,
    DateTime? LastStatusChange,
    PaymentStatus? Status,
    string? Notes
);

public record PaymentsWithinDateRangeDto(DateTime StartTime, DateTime EndTime);
