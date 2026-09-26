namespace LibraryManagement.Domain.Entities;

public class Member
{
    public int Id { get; set; }

    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;

    public DateTime RegistrationDate { get; set; } = DateTime.UtcNow;
    public DateTime MembershipEndDate { get; set; }

    public MemberStatus Status { get; private set; } = MemberStatus.Active;
    public string? BanReason { get; private set; }
    public DateTime? BanDate { get; private set; }

    public ICollection<BorrowRecord> BorrowRecords { get; set; } = new List<BorrowRecord>();

    public bool CanBorrow()
    {
        return Status == MemberStatus.Active && DateTime.UtcNow <= MembershipEndDate;
    }

    public void Ban(string reason)
    {
        Status = MemberStatus.Banned;
        BanReason = reason;
        BanDate = DateTime.UtcNow;
    }

    public void UnBan()
    {
        Status = MemberStatus.Active;
        BanReason = null;
        BanDate = null;
    }

    public void DeactivateMembership()
    {
        Status = MemberStatus.Inactive;
    }

    public void ActivateMembership()
    {
        Status = MemberStatus.Active;
    }

    public void RenewMembership(int months)
    {
        var baseDate = DateTime.UtcNow > MembershipEndDate ? DateTime.UtcNow : MembershipEndDate;
        MembershipEndDate = baseDate.AddMonths(months);

        // If account was inactive/expired, renewing reactivates it (unless blocked)
        if (Status == MemberStatus.Inactive)
        {
            Status = MemberStatus.Active;
        }
    }
}