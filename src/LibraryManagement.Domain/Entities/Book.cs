namespace LibraryManagement.Domain.Entities;

public class Book
{
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;
    public string ISBN { get; set; } = string.Empty;
    public string Language { get; set; } = "en";
    public string? Description { get; set; }
    public int PublicationYear { get; set; }

    public int AuthorId { get; set; }
    public Author Author { get; set; } = null!;

    public int CategoryId { get; set; }
    public Category Category { get; set; } = null!;

    public ICollection<BookCopy> Copies { get; set; } = new List<BookCopy>();

    public int AvailableCopyCount => Copies.Count(copy => copy.IsAvailable());

    public bool IsAvailable() => Copies.Any(copy => copy.IsAvailable());
}