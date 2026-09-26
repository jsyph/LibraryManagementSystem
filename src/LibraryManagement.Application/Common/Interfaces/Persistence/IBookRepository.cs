namespace LibraryManagement.Application.Common.Interfaces;

using LibraryManagement.Domain.Entities;

public interface IBookRepository
{
    Task<Book?> GetByIdAsync(int id);
    Task<IEnumerable<Book>> GetAllAsync();
    Task<IEnumerable<Book>> SearchAsync(string? title, string? isbn, int? authorId, int? categoryId);
    Task AddAsync(Book book);
    Task UpdateAsync(Book book);
    Task DeleteAsync(Book book);
    Task<bool> ExistsAsync(int id);
    Task<bool> IsISBNUniqueAsync(string isbn);
}