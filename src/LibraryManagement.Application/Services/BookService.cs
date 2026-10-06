namespace LibraryManagement.Application.Services;

using LibraryManagement.Application.Common.Exceptions;
using LibraryManagement.Application.Common.Interfaces.Persistence;
using LibraryManagement.Application.Common.Interfaces.Services;
using LibraryManagement.Application.DTOs;
using LibraryManagement.Domain.Entities;

public class BookService : IBookService
{
    private readonly IUnitOfWork unitOfWork;

    public BookService(IUnitOfWork unitOfWork)
    {
        this.unitOfWork = unitOfWork;
    }

    public async Task<BookDto?> GetBookByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var book = await unitOfWork.Books.GetByIdAsync(id, cancellationToken);
        return book is null ? null : MapToDto(book);
    }

    public async Task<IEnumerable<BookDto>> GetAllBooksAsync(CancellationToken cancellationToken = default)
    {
        var books = await unitOfWork.Books.GetAllAsync(cancellationToken);
        return books.Select(MapToDto).ToList();
    }

    public async Task<IEnumerable<BookDto>> SearchBooksAsync(
        TitleDescriptionBookSearchFilterDto dto,
        CancellationToken cancellationToken = default
    )
    {
        var books = await unitOfWork.Books.SearchAsync(dto.SearchTerm, cancellationToken);
        return books.Select(MapToDto).ToList();
    }

    public async Task<IEnumerable<BookDto>> GetBooksByAuthorIdAsync(
        int authorId,
        CancellationToken cancellationToken = default
    )
    {
        var books = await unitOfWork.Books.GetByAuthorIdAsync(authorId, cancellationToken);
        return books.Select(MapToDto).ToList();
    }

    public async Task<IEnumerable<BookDto>> GetBooksByCategory(
        int categoryId,
        CancellationToken cancellationToken = default
    )
    {
        var books = await unitOfWork.Books.GetByCategoryIdAsync(categoryId, cancellationToken);
        return books.Select(MapToDto).ToList();
    }

    public async Task<BookDto?> GetBookByIsbnAsync(string isbn, CancellationToken cancellationToken = default)
    {
        var book = await unitOfWork.Books.GetByIsbnAsync(isbn, cancellationToken);
        return book is null ? null : MapToDto(book);
    }

    public async Task<BookDto> AddBookAsync(
        CreateBookDto dto,
        CancellationToken cancellationToken = default
    )
    {
        await EnsureBookReferencesExistAsync(dto.AuthorId, dto.CategoryId, cancellationToken);
        if (!await unitOfWork.Books.IsIsbnUniqueAsync(dto.ISBN, cancellationToken))
        {
            throw new InvalidOperationException($"A book with ISBN '{dto.ISBN}' already exists.");
        }

        var book = new Book
        {
            Title = dto.Title,
            ISBN = dto.ISBN,
            Description = dto.Description,
            PublicationYear = dto.PublicationYear,
            Language = dto.Language,
            AuthorId = dto.AuthorId,
            CategoryId = dto.CategoryId,
            DateAdded = DateTime.UtcNow,
        };

        await unitOfWork.Books.AddAsync(book, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return (await GetBookByIdAsync(book.Id, cancellationToken))!;
    }

    public async Task UpdateBookAsync(
        int id,
        UpdateBookDto dto,
        CancellationToken cancellationToken = default
    )
    {
        var book = await GetRequiredBookAsync(id, cancellationToken);
        await EnsureBookReferencesExistAsync(dto.AuthorId, dto.CategoryId, cancellationToken);
        if (
            dto.ISBN != book.ISBN
            && !await unitOfWork.Books.IsIsbnUniqueAsync(dto.ISBN, cancellationToken)
        )
        {
            throw new InvalidOperationException($"A book with ISBN '{dto.ISBN}' already exists.");
        }

        book.Title = dto.Title;
        book.ISBN = dto.ISBN;
        book.Description = dto.Description;
        book.PublicationYear = dto.PublicationYear;
        book.Language = dto.Language;
        book.AuthorId = dto.AuthorId;
        book.CategoryId = dto.CategoryId;

        await unitOfWork.Books.UpdateAsync(book, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteBookAsync(int id, CancellationToken cancellationToken = default)
    {
        var book = await GetRequiredBookAsync(id, cancellationToken);
        await unitOfWork.Books.DeleteAsync(book, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }

    private async Task<Book> GetRequiredBookAsync(int id, CancellationToken cancellationToken) =>
        await unitOfWork.Books.GetByIdAsync(id, cancellationToken)
        ?? throw new NotFoundException(nameof(Book), id);

    private async Task EnsureBookReferencesExistAsync(
        int authorId,
        int categoryId,
        CancellationToken cancellationToken
    )
    {
        if (!await unitOfWork.Authors.ExistsAsync(authorId, cancellationToken))
        {
            throw new NotFoundException(nameof(Author), authorId);
        }

        if (!await unitOfWork.Categories.ExistsAsync(categoryId, cancellationToken))
        {
            throw new NotFoundException(nameof(Category), categoryId);
        }
    }

    private static BookDto MapToDto(Book book) =>
        new(
            book.Id,
            book.Title,
            book.ISBN,
            book.Language,
            book.Description,
            book.PublicationYear,
            book.AuthorId,
            book.Author != null ? $"{book.Author.FirstName} {book.Author.LastName}" : string.Empty,
            book.CategoryId,
            book.Category != null ? book.Category.Name : string.Empty,
            book.Copies?.Count ?? 0,
            book.Copies != null ? book.AvailableCopyCount : 0,
            book.DateAdded
        );
}