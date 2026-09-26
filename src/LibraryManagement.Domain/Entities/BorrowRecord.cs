namespace LibraryManagement.Domain.Entities;

public class BorrowRecord
{
    public int Id { get; set; }

    public DateTime BorrowDate { get; set; } = DateTime.UtcNow;
    public DateTime DueDate { get; set; }
    public DateTime? ReturnDate { get; set; }

    public int BookId { get; set; }
    public Book Book { get; set; } = null!;

    public int MemberId { get; set; }
    public Member Member { get; set; } = null!;

    public string? Notes { get; set; }

    public bool IsOverdue => ReturnDate == null && DateTime.UtcNow > DueDate;
}