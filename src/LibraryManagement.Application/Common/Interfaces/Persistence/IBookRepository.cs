namespace LibraryManagement.Application.Common.Interfaces.Persistence;

using LibraryManagement.Domain.Entities;

public interface IBookRepository
{
    Task<Book?> GetByIdAsync(int id);
    Task<IEnumerable<Book>> GetAllAsync();
    Task<IEnumerable<Book>> SearchByTitleAsync(string title);
    Task<IEnumerable<Book>> SearchByDescriptionContentAsync(string descriptionContent);
    Task<Book?> GetByIsbnAsync(string isbn);
    Task<IEnumerable<Book>> GetByCategoryIdAsync(int categoryId);
    Task<IEnumerable<Book>> GetByAuthorIdAsync(int authorId);
    Task AddAsync(Book book);
    Task UpdateAsync(Book book);
    Task DeleteAsync(Book book);
    Task<bool> ExistsAsync(int id);
    Task<bool> IsIsbnUniqueAsync(string isbn);
}