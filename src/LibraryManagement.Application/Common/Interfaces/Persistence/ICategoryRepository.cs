namespace LibraryManagement.Application.Common.Interfaces;

using LibraryManagement.Domain.Entities;

public interface ICategoryRepository
{
    Task<Category?> GetByIdAsync(int id);
    Task<IEnumerable<Category>> GetAllAsync();
    Task AddAsync(Category category);
    Task UpdateAsync(Category category);
    Task DeleteAsync(Category category);
    Task<bool> ExistsAsync(int id);
    Task<bool> ExistsByNameAsync(string name);
}