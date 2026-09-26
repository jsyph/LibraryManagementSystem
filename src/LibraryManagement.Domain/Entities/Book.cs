namespace LibraryManagement.Domain.Entities;

using LibraryManagement.Domain.Enums;

public class Book
{
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;
    public string ISBN { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int PublicationYear { get; set; }

    public BookStatus Status { get; set; } = BookStatus.Available;

    public int AuthorId { get; set; }
    public Author Author { get; set; } = null!;

    public int CategoryId { get; set; }
    public Category Category { get; set; } = null!;

    public ICollection<BorrowRecord> BorrowRecords { get; set; } = new List<BorrowRecord>();

    public bool IsAvailable() => Status == BookStatus.Available;
}