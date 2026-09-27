namespace LibraryManagement.Domain.Entities;

using LibraryManagement.Domain.Enums;

public class Payment
{
    public int Id { get; set; }

    public int BorrowRecordId { get; set; }
    public BorrowRecord BorrowRecord { get; set; } = null!;

    public decimal Amount { get; set; }
    public PaymentMethod PaymentMethod { get; set; }
    public DateTime PaymentDate { get; set; } = DateTime.UtcNow;
    public PaymentStatus Status { get; set; } = PaymentStatus.Pending;
    public string? Notes { get; set; }
}