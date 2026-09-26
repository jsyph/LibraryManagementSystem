namespace LibraryManagement.Domain.Entities;

public class Author
{
    public int Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string? Biography { get; set; }

    public string? Nationality { get; set; }

    public ICollection<Book> Books { get; set; } = new List<Book>();
}