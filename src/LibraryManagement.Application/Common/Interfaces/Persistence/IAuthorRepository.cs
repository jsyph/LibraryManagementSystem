namespace LibraryManagement.Application.Common.Interfaces.Persistence;

using LibraryManagement.Domain.Entities;

public interface IAuthorRepository
{
    Task<Author?> GetByIdAsync(int id);
    Task<IEnumerable<Author>> GetAllAsync();
    Task<IEnumerable<Author>> SearchByNameAsync(string name);
    Task AddAsync(Author author);
    Task UpdateAsync(Author author);
    Task DeleteAsync(Author author);
    Task<bool> ExistsAsync(int id);
}