namespace LibraryManagement.Domain.Entities;

using LibraryManagement.Domain.Enums;

public class BookCopy
{
    public int Id { get; set; }

    public int BookId { get; set; }
    public Book Book { get; set; } = null!;

    public BookStatus Status { get; set; } = BookStatus.Available;

    public ICollection<BorrowRecord> BorrowRecords { get; set; } = new List<BorrowRecord>();

    public bool IsAvailable() => Status == BookStatus.Available;
}