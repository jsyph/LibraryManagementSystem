namespace LibraryManagement.Application.Common.Interfaces.Persistence;

using LibraryManagement.Domain.Entities;

public interface IAuthorRepository
{
    Task<Author?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<Author>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<Author>> SearchAsync(
        string? FirstName,
        string? LastName,
        string? Nationality,
        CancellationToken cancellationToken = default
    );
    Task AddAsync(Author author, CancellationToken cancellationToken = default);
    Task UpdateAsync(Author author, CancellationToken cancellationToken = default);
    Task DeleteAsync(Author author, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default);
}
