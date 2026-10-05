namespace LibraryManagement.Application.Common.Interfaces.Services;

using LibraryManagement.Application.DTOs;

public interface IAuthorService
{
    Task<AuthorDto?> GetAuthorByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<AuthorDto>> GetAllAuthorsAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<AuthorDto>> SearchAuthorsAsync(
        SearchAuthorDto dto,
        CancellationToken cancellationToken = default
    );
    Task<AuthorDto> AddAuthorAsync(
        CreateAuthorDto dto,
        CancellationToken cancellationToken = default
    );
    Task UpdateAuthorAsync(
        int id,
        UpdateAuthorDto dto,
        CancellationToken cancellationToken = default
    );
    Task DeleteAuthorAsync(int id, CancellationToken cancellationToken = default);
}
