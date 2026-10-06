using LibraryManagement.Application.Common.Interfaces.Persistence;
using LibraryManagement.Domain.Entities;
using LibraryManagement.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Infrastructure.Persistence.Repositories;

public sealed class UserRepository : IUserRepository
{
    private readonly DbSet<User> DbSet;

    public UserRepository(LibraryDbContext context)
    {
        DbSet = context.Set<User>();
    }

    public async Task<User?> GetByIdAsync(int id, CancellationToken cancellationToken = default) =>
        await DbSet.FirstOrDefaultAsync(user => user.Id == id, cancellationToken);

    public async Task<IEnumerable<User>> GetAllAsync(
        CancellationToken cancellationToken = default
    ) => await DbSet.AsNoTracking().ToListAsync(cancellationToken);

    public async Task AddAsync(User user, CancellationToken cancellationToken = default) =>
        await DbSet.AddAsync(user, cancellationToken);

    public Task UpdateAsync(User user, CancellationToken cancellationToken = default)
    {
        DbSet.Update(user);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(User user, CancellationToken cancellationToken = default)
    {
        DbSet.Remove(user);
        return Task.CompletedTask;
    }

    public Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default) =>
        DbSet.AnyAsync(user => user.Id == id, cancellationToken);

    public async Task<IEnumerable<User>> GetByRoleAsync(
        UserRole role,
        CancellationToken cancellationToken = default
    ) => await UsersForRole(role).AsNoTracking().ToListAsync(cancellationToken);

    public async Task<IEnumerable<User>> SearchAsync(
        string? firstName,
        string? lastName,
        string? email,
        string? phoneNumber,
        bool? isActive,
        DateTime? dateAdded,
        UserRole? role,
        CancellationToken cancellationToken = default
    )
    {
        var query = role.HasValue ? UsersForRole(role.Value) : DbSet.AsQueryable();
        if (!string.IsNullOrWhiteSpace(firstName))
        {
            var pattern = $"%{firstName}%";
            query = query.Where(user => EF.Functions.Like(user.FirstName, pattern));
        }
        if (!string.IsNullOrWhiteSpace(lastName))
        {
            var pattern = $"%{lastName}%";
            query = query.Where(user => EF.Functions.Like(user.LastName, pattern));
        }
        if (!string.IsNullOrWhiteSpace(email))
        {
            var pattern = $"%{email}%";
            query = query.Where(user => EF.Functions.Like(user.Email, pattern));
        }
        if (!string.IsNullOrWhiteSpace(phoneNumber))
        {
            var pattern = $"%{phoneNumber}%";
            query = query.Where(user => EF.Functions.Like(user.PhoneNumber, pattern));
        }
        if (isActive.HasValue)
            query = query.Where(user => user.IsActive == isActive.Value);
        if (dateAdded.HasValue)
            query = query.Where(user => user.DateAdded.Date == dateAdded.Value.Date);

        var users = await query.AsNoTracking().ToListAsync(cancellationToken);
        IEnumerable<User> results = users;
        if (!string.IsNullOrWhiteSpace(firstName))
            results = FuzzySearch.Rank(results, firstName, user => [user.FirstName]);
        if (!string.IsNullOrWhiteSpace(lastName))
            results = FuzzySearch.Rank(results, lastName, user => [user.LastName]);

        return results;
    }

    public async Task<bool> EmailExistsAsync(
        string email,
        CancellationToken cancellationToken = default
    ) => await DbSet.AnyAsync(user => user.Email == email, cancellationToken);

    public async Task<bool> PhoneExistsAsync(
        string phone,
        CancellationToken cancellationToken = default
    ) => await DbSet.AnyAsync(user => user.PhoneNumber == phone, cancellationToken);

    private IQueryable<User> UsersForRole(UserRole role) =>
        role switch
        {
            UserRole.Member => DbSet.OfType<Member>(),
            UserRole.Librarian => DbSet.OfType<Librarian>(),
            UserRole.Admin => DbSet.OfType<Admin>(),
            _ => DbSet.Where(_ => false),
        };
}
