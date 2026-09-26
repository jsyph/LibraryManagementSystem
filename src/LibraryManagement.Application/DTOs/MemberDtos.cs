namespace LibraryManagement.Application.DTOs;

using LibraryManagement.Domain.Enums;

public record MemberDto(
    int Id,
    string FirstName,
    string LastName,
    string Email,
    string Phone,
    DateTime DateOfBirth,
    int Age,
    MemberType Type,
    string? NationalId,
    string? PassportNumber,
    string? StudentId,
    string? InstitutionName,
    DateTime RegistrationDate,
    DateTime MembershipEndDate,
    MemberStatus Status,
    string? BanReason,
    DateTime? BanDate,
    bool CanBorrow
);

public record CreateMemberDto(
    string FirstName,
    string LastName,
    string Email,
    string Phone,
    DateTime DateOfBirth,
    MemberType Type,
    string? NationalId,
    string? PassportNumber,
    string? StudentId,
    string? InstitutionName,
    int InitialMembershipMonths = 1
);

public record UpdateMemberDto(
    string FirstName,
    string LastName,
    string Email,
    string Phone,
    string? StudentId,
    string? InstitutionName
);

public record BanMemberDto(
    string Reason
);

public record RenewMembershipDto(
    int Months = 1
);