namespace LibraryManagement.Domain.Enums;

public enum MemberStatus
{
    Active = 1,       // Normal account
    Inactive = 2,     // Account closed / soft deleted
    Banned = 3        // Account suspended
}