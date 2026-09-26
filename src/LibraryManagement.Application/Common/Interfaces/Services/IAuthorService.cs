namespace LibraryManagement.Application.Common.Interfaces.Services;

using LibraryManagement.Application.DTOs;

public interface IAuthorService
{
    Task<AuthorDto> GetByIdAsync(int id, CancellationToken ct = default);
    Task<IEnumerable<AuthorDto>> GetAllAsync(CancellationToken ct = default);
    Task<AuthorDto> CreateAsync(CreateAuthorDto dto, CancellationToken ct = default);
    Task UpdateAsync(int id, UpdateAuthorDto dto, CancellationToken ct = default);
    Task DeleteAsync(int id, CancellationToken ct = default);
    
    Task<IEnumerable<BookDto>> GetBooksByAuthorIdAsync(int authorId, CancellationToken ct = default);
}