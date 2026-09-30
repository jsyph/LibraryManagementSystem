using LibraryManagement.Domain.Enums;

namespace LibraryManagement.Domain.Entities;

public abstract class User
{
    public int Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;

    public abstract UserRole Role { get; }

    public string PhoneNumber { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
    public DateTime DateAdded { get; set; } = DateTime.UtcNow;
}

public class Member : User
{
    public override UserRole Role => UserRole.Member;

    public DateTime MembershipStartDate { get; set; } = DateTime.UtcNow;
    public DateTime? MembershipExpiryDate { get; set; } = null;

    public ICollection<BorrowRecord> BorrowRecords { get; set; } = new List<BorrowRecord>();
}

public class Librarian : User
{
    public override UserRole Role => UserRole.Librarian;

    public DateTime HireDate { get; set; } = DateTime.UtcNow;
    public string Department { get; set; } = string.Empty;
}

public class Admin : User
{
    public override UserRole Role => UserRole.Admin;
}