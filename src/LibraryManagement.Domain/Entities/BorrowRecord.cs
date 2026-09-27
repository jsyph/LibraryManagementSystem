namespace LibraryManagement.Domain.Entities;

public class BorrowRecord
{
    public int Id { get; set; }

    public DateTime BorrowDate { get; set; } = DateTime.UtcNow;
    public DateTime DueDate { get; set; }
    public DateTime? ReturnDate { get; set; }

    public int BookCopyId { get; set; }
    public BookCopy BookCopy { get; set; } = null!;

    public int UserId { get; set; }
    public User User { get; set; } = null!;

    public string? Notes { get; set; }

    public ICollection<Payment> Payments { get; set; } = new List<Payment>();

    public bool IsOverdue => ReturnDate == null && DateTime.UtcNow > DueDate;
}