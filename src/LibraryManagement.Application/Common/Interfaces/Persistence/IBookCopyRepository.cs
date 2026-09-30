namespace LibraryManagement.Application.Common.Interfaces.Persistence;

using LibraryManagement.Domain.Entities;
using LibraryManagement.Domain.Enums;

public interface IBookCopyRepository
{
    Task<BookCopy?> GetByIdAsync(int id);
    Task<IEnumerable<BookCopy>> GetAllAsync();
    Task<IEnumerable<BookCopy>> GetByStatusAsync(BookStatus bookStatus);
    Task AddAsync(BookCopy bookCopy);
    Task UpdateAsync(BookCopy bookCopy);
    Task DeleteAsync(BookCopy bookCopy);
    Task<bool> ExistsAsync(int id);
    Task<bool> IsAvailableAsync(int id);
}