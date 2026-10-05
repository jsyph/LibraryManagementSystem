namespace LibraryManagement.Application.Services;

using LibraryManagement.Application.Common.Exceptions;
using LibraryManagement.Application.Common.Interfaces.Persistence;
using LibraryManagement.Application.Common.Interfaces.Services;
using LibraryManagement.Application.DTOs;
using LibraryManagement.Domain.Entities;

public class AuthorService : IAuthorService
{
    private readonly IUnitOfWork unitOfWork;

    public AuthorService(IUnitOfWork unitOfWork)
    {
        this.unitOfWork = unitOfWork;
    }

    public async Task<AuthorDto?> GetAuthorByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var author = await unitOfWork.Authors.GetByIdAsync(id, cancellationToken);
        return author is null ? null : MapToDto(author);
    }

    public async Task<IEnumerable<AuthorDto>> GetAllAuthorsAsync(CancellationToken cancellationToken = default)
    {
        var authors = await unitOfWork.Authors.GetAllAsync(cancellationToken);
        return authors.Select(MapToDto).ToList();
    }

    public async Task<IEnumerable<AuthorDto>> SearchAuthorsAsync(
        SearchAuthorDto dto,
        CancellationToken cancellationToken = default
    )
    {
        var authors = await unitOfWork.Authors.SearchAsync(
            dto.FirstName,
            dto.LastName,
            dto.Nationality,
            cancellationToken
        );
        return authors.Select(MapToDto).ToList();
    }

    public async Task<AuthorDto> AddAuthorAsync(
        CreateAuthorDto dto,
        CancellationToken cancellationToken = default
    )
    {
        var author = new Author
        {
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Biography = dto.Biography,
            Nationality = dto.Nationality,
        };

        await unitOfWork.Authors.AddAsync(author, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return MapToDto(author);
    }

    public async Task UpdateAuthorAsync(
        int id,
        UpdateAuthorDto dto,
        CancellationToken cancellationToken = default
    )
    {
        var author = await GetRequiredAuthorAsync(id, cancellationToken);
        author.FirstName = dto.FirstName;
        author.LastName = dto.LastName;
        author.Biography = dto.Biography;
        author.Nationality = dto.Nationality;
        await unitOfWork.Authors.UpdateAsync(author, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAuthorAsync(int id, CancellationToken cancellationToken = default)
    {
        var author = await GetRequiredAuthorAsync(id, cancellationToken);
        await unitOfWork.Authors.DeleteAsync(author, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }

    private async Task<Author> GetRequiredAuthorAsync(
        int id,
        CancellationToken cancellationToken
    ) =>
        await unitOfWork.Authors.GetByIdAsync(id, cancellationToken)
        ?? throw new NotFoundException(nameof(Author), id);

    private static AuthorDto MapToDto(Author author) =>
        new(author.Id, author.FirstName, author.LastName, author.Biography, author.Nationality);
}