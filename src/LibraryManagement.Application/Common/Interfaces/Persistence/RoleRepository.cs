namespace LibraryManagement.Application.Common.Interfaces.Persistence;

using LibraryManagement.Domain.Entities;

public interface IRoleRepository
{
    Task<Role?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<Role>> GetAllAsync(CancellationToken cancellationToken = default);
    Task AddAsync(Role role, CancellationToken cancellationToken = default);
    Task UpdateAsync(Role role, CancellationToken cancellationToken = default);
    Task DeleteAsync(Role role, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<Role>> SearchByTitle(string title, CancellationToken cancellationToken = default);
}