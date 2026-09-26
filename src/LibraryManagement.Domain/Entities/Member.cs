namespace LibraryManagement.Domain.Entities;

public class Member
{
    public int Id { get; set; }

    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;

    public DateTime RegistrationDate { get; set; } = DateTime.UtcNow;
    public ICollection<BorrowRecord> BorrowRecords { get; set; } = new List<BorrowRecord>();
    
}