namespace LibraryManagement.Application.Services;

using LibraryManagement.Application.Common.Exceptions;
using LibraryManagement.Application.Common.Interfaces.Persistence;
using LibraryManagement.Application.Common.Interfaces.Services;
using LibraryManagement.Application.DTOs;
using LibraryManagement.Domain.Entities;
using LibraryManagement.Domain.Enums;

public class BookCopyService : IBookCopyService
{
    private readonly IUnitOfWork unitOfWork;

    public BookCopyService(IUnitOfWork unitOfWork)
    {
        this.unitOfWork = unitOfWork;
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
    ) => await GetAllBookCopiesByBookIdAsync(bookId, cancellationToken);

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
        return bookCopies.Where(copy => copy.Status == BookStatus.Available).Select(MapToDto).ToList();
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

        var book = await unitOfWork.Books.GetByIdAsync(bookId, cancellationToken)
            ?? throw new NotFoundException(nameof(Book), bookId);
        var bookCopies = Enumerable.Range(0, dto.Count).Select(_ => new BookCopy
        {
            BookId = bookId,
            Book = book,
            Status = dto.Status,
        }).ToList();

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

    private async Task<BookCopy> GetRequiredBookCopyAsync(
        int id,
        CancellationToken cancellationToken
    ) =>
        await unitOfWork.BookCopy.GetByIdAsync(id, cancellationToken)
        ?? throw new NotFoundException(nameof(BookCopy), id);

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
}