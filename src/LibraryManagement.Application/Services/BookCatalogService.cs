namespace LibraryManagement.Application.Services;

using LibraryManagement.Application.Common.Exceptions;
using LibraryManagement.Application.Common.Interfaces.Persistence;
using LibraryManagement.Application.Common.Interfaces.Services;
using LibraryManagement.Application.DTOs;
using LibraryManagement.Domain.Entities;
using LibraryManagement.Domain.Enums;

public class BookCatalogService : IBookCatalogService
{
    private readonly IUnitOfWork unitOfWork;

    public BookCatalogService(IUnitOfWork unitOfWork)
    {
        this.unitOfWork = unitOfWork;
    }

    public async Task<BookDto?> GetBookByIdAsync(
        int id,
        CancellationToken cancellationToken = default
    )
    {
        var book = await unitOfWork.Books.GetByIdAsync(id, cancellationToken);
        return book is null ? null : MapToDto(book);
    }

    public async Task<IEnumerable<BookDto>> GetAllBooksAsync(
        CancellationToken cancellationToken = default
    )
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

    public async Task<BookDto?> GetBookByIsbnAsync(
        string isbn,
        CancellationToken cancellationToken = default
    )
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
        return MapToDto(book);
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

    public async Task<BookCopyDto?> GetBookCopyByIdAsync(
        int id,
        CancellationToken cancellationToken = default
    )
    {
        var bookCopy = await unitOfWork.BookCopy.GetByIdAsync(id, cancellationToken);
        return bookCopy is null ? null : MapToDto(bookCopy);
    }

    public async Task<IEnumerable<BookCopyDto>> GetAllBookCopiesAsync(
        int bookId,
        CancellationToken cancellationToken = default
    )
    {
        var bookCopies = await unitOfWork.BookCopy.GetAllByBookIdAsync(bookId, cancellationToken);
        return bookCopies.Select(MapToDto).ToList();
    }

    public async Task<IEnumerable<BookCopyDto>> GetAllBookCopiesByBookIdAsync(
        int bookId,
        CancellationToken cancellationToken = default
    )
    {
        var bookCopies = await unitOfWork.BookCopy.GetAllByBookIdAsync(bookId, cancellationToken);
        return bookCopies.Select(MapToDto).ToList();
    }

    public async Task<IEnumerable<BookCopyDto>> GetBookCopiesByStatusAsync(
        int bookStatus,
        CancellationToken cancellationToken = default
    )
    {
        if (!Enum.IsDefined(typeof(BookStatus), bookStatus))
        {
            throw new ArgumentOutOfRangeException(nameof(bookStatus));
        }

        var bookCopies = await unitOfWork.BookCopy.GetByStatusAsync(
            (BookStatus)bookStatus,
            cancellationToken
        );
        return bookCopies.Select(MapToDto).ToList();
    }

    public async Task<IEnumerable<BookCopyDto>> GetAvilableBookCopiesAsync(
        int bookId,
        CancellationToken cancellationToken = default
    )
    {
        var bookCopies = await unitOfWork.BookCopy.GetAllByBookIdAsync(bookId, cancellationToken);
        return bookCopies
            .Where(copy => copy.Status == BookStatus.Available)
            .Select(MapToDto)
            .ToList();
    }

    public async Task<IEnumerable<BookCopyDto>> AddBookCopiesAsync(
        int bookId,
        AddBookCopiesDto dto,
        CancellationToken cancellationToken = default
    )
    {
        if (dto.Count <= 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(dto.Count),
                "Count must be greater than zero."
            );
        }

        var book = await GetRequiredBookAsync(bookId, cancellationToken);
        var bookCopies = Enumerable
            .Range(0, dto.Count)
            .Select(_ => new BookCopy
            {
                BookId = bookId,
                Book = book,
                Status = dto.Status,
            })
            .ToList();

        foreach (var bookCopy in bookCopies)
        {
            await unitOfWork.BookCopy.AddAsync(bookCopy, cancellationToken);
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);
        return bookCopies.Select(MapToDto).ToList();
    }

    public async Task UpdateBookCopyStatusAsync(
        int id,
        ChangeBookCopyStatusDto dto,
        CancellationToken cancellationToken = default
    )
    {
        var bookCopy = await GetRequiredBookCopyAsync(id, cancellationToken);
        bookCopy.Status = dto.Status;
        await unitOfWork.BookCopy.UpdateAsync(bookCopy, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteBookCopyAsync(int id, CancellationToken cancellationToken = default)
    {
        var bookCopy = await GetRequiredBookCopyAsync(id, cancellationToken);
        await unitOfWork.BookCopy.DeleteAsync(bookCopy, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task<AuthorDto?> GetAuthorByIdAsync(
        int id,
        CancellationToken cancellationToken = default
    )
    {
        var author = await unitOfWork.Authors.GetByIdAsync(id, cancellationToken);
        return author is null ? null : MapToDto(author);
    }

    public async Task<IEnumerable<AuthorDto>> GetAllAuthorsAsync(
        CancellationToken cancellationToken = default
    )
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

    public async Task<CategoryDto?> GetCategoryByIdAsync(
        int id,
        CancellationToken cancellationToken = default
    )
    {
        var category = await unitOfWork.Categories.GetByIdAsync(id, cancellationToken);
        return category is null ? null : MapToDto(category);
    }

    public async Task<IEnumerable<CategoryDto>> GetAllCategoriesAsync(
        CancellationToken cancellationToken = default
    )
    {
        var categories = await unitOfWork.Categories.GetAllAsync(cancellationToken);
        return categories.Select(MapToDto).ToList();
    }

    public async Task<CategoryDto> AddCategoryAsync(
        CreateCategoryDto dto,
        CancellationToken cancellationToken = default
    )
    {
        var category = new Category { Name = dto.Name, Description = dto.Description };
        await unitOfWork.Categories.AddAsync(category, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return MapToDto(category);
    }

    public async Task UpdateAsync(
        int id,
        UpdateCategoryDto dto,
        CancellationToken cancellationToken = default
    )
    {
        var category = await GetRequiredCategoryAsync(id, cancellationToken);
        category.Name = dto.Name;
        category.Description = dto.Description;
        await unitOfWork.Categories.UpdateAsync(category, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteCategoryAsync(int id, CancellationToken cancellationToken = default)
    {
        var category = await GetRequiredCategoryAsync(id, cancellationToken);
        await unitOfWork.Categories.DeleteAsync(category, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }

    private async Task<Book> GetRequiredBookAsync(int id, CancellationToken cancellationToken) =>
        await unitOfWork.Books.GetByIdAsync(id, cancellationToken)
        ?? throw new NotFoundException(nameof(Book), id);

    private async Task<BookCopy> GetRequiredBookCopyAsync(
        int id,
        CancellationToken cancellationToken
    ) =>
        await unitOfWork.BookCopy.GetByIdAsync(id, cancellationToken)
        ?? throw new NotFoundException(nameof(BookCopy), id);

    private async Task<Author> GetRequiredAuthorAsync(
        int id,
        CancellationToken cancellationToken
    ) =>
        await unitOfWork.Authors.GetByIdAsync(id, cancellationToken)
        ?? throw new NotFoundException(nameof(Author), id);

    private async Task<Category> GetRequiredCategoryAsync(
        int id,
        CancellationToken cancellationToken
    ) =>
        await unitOfWork.Categories.GetByIdAsync(id, cancellationToken)
        ?? throw new NotFoundException(nameof(Category), id);

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
            $"{book.Author.FirstName} {book.Author.LastName}",
            book.CategoryId,
            book.Category.Name,
            book.Copies.Count,
            book.AvailableCopyCount,
            book.DateAdded
        );

    private static BookCopyDto MapToDto(BookCopy bookCopy) =>
        new(
            bookCopy.Id,
            bookCopy.BookId,
            bookCopy.Book.Title,
            bookCopy.Book.ISBN,
            bookCopy.Status,
            bookCopy.DateAdded,
            bookCopy.IsAvailable()
        );

    private static AuthorDto MapToDto(Author author) =>
        new(author.Id, author.FirstName, author.LastName, author.Biography, author.Nationality);

    private static CategoryDto MapToDto(Category category) =>
        new(category.Id, category.Name, category.Description);
}
