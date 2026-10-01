namespace LibraryManagement.Application.Common.Interfaces.Services;

using LibraryManagement.Application.DTOs;

// handles books, authors, CategoryDto and copies
public interface IBookCatalogService
{
    #region Book
    Task<BookDto?> GetBookByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<BookDto>> GetAllBooksAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<BookDto>> SearchBooksAsync(
        TitleDescriptionBookSearchFilterDto dto,
        CancellationToken cancellationToken = default
    );
    Task<IEnumerable<BookDto>> GetBooksByAuthorIdAsync(
        int authorId,
        CancellationToken cancellationToken = default
    );
    Task<IEnumerable<BookDto>> GetBooksByCategory(
        int categoryId,
        CancellationToken cancellationToken = default
    );
    Task<BookDto?> GetBookByIsbnAsync(string isbn, CancellationToken cancellationToken = default);
    Task<BookDto> AddBookAsync(CreateBookDto dto, CancellationToken cancellationToken = default);
    Task UpdateBookAsync(int id, UpdateBookDto dto, CancellationToken cancellationToken = default);
    Task DeleteBookAsync(int id, CancellationToken cancellationToken = default);
    #endregion

    #region Book Copies
    Task<BookCopyDto?> GetBookCopyByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<BookCopyDto>> GetAllBookCopiesAsync(
        int bookId,
        CancellationToken cancellationToken = default
    );
    Task<IEnumerable<BookCopyDto>> GetAllBookCopiesByBookIdAsync(
        int bookId,
        CancellationToken cancellationToken = default
    );

    Task<IEnumerable<BookCopyDto>> GetBookCopiesByStatusAsync(
        int bookStatus,
        CancellationToken cancellationToken = default
    );
    Task<IEnumerable<BookCopyDto>> GetAvilableBookCopiesAsync(
        int bookId,
        CancellationToken cancellationToken = default
    );
    Task<IEnumerable<BookCopyDto>> AddBookCopiesAsync(
        int bookId,
        AddBookCopiesDto dto,
        CancellationToken cancellationToken = default
    );
    Task UpdateBookCopyStatusAsync(
        int id,
        ChangeBookCopyStatusDto dto,
        CancellationToken cancellationToken = default
    );
    Task DeleteBookCopyAsync(int id, CancellationToken cancellationToken = default);
    #endregion

    #region Authors
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
    #endregion

    #region CategoryDto
    Task<CategoryDto?> GetCategoryByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<CategoryDto>> GetAllCategoriesAsync(
        CancellationToken cancellationToken = default
    );
    Task<CategoryDto> AddCategoryAsync(
        CreateCategoryDto dto,
        CancellationToken cancellationToken = default
    );
    Task UpdateAsync(int id, UpdateCategoryDto dto, CancellationToken cancellationToken = default);
    Task DeleteCategoryAsync(int id, CancellationToken cancellationToken = default);
    #endregion
}
