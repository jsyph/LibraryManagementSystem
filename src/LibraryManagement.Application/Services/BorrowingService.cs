namespace LibraryManagement.Application.Services;

using LibraryManagement.Application.Common.Exceptions;
using LibraryManagement.Application.Common.Interfaces.Persistence;
using LibraryManagement.Application.Common.Interfaces.Services;
using LibraryManagement.Application.DTOs;
using LibraryManagement.Domain.Entities;
using LibraryManagement.Domain.Enums;

public class BorrowingService : IBorrowingService
{
    private readonly IUnitOfWork unitOfWork;

    public BorrowingService(IUnitOfWork unitOfWork)
    {
        this.unitOfWork = unitOfWork;
    }

    public async Task<BorrowRecordDto?> GetRecordByIdAsync(
        int id,
        CancellationToken cancellationToken = default
    )
    {
        var record = await unitOfWork.BorrowRecords.GetByIdAsync(id, cancellationToken);
        return record is null ? null : MapToDto(record);
    }

    public async Task<BorrowRecordDto?> GetActiveRecordByBookCopyIdAsync(
        int bookCopyId,
        CancellationToken cancellationToken = default
    )
    {
        var record = await unitOfWork.BorrowRecords.GetActiveRecordByBookCopyIdAsync(
            bookCopyId,
            cancellationToken
        );
        return record is null ? null : MapToDto(record);
    }

    public async Task<IEnumerable<BorrowRecordDto>> GetAllRecordsByBookCopyIdAsync(
        int bookCopyId,
        CancellationToken cancellationToken = default
    )
    {
        var records = await unitOfWork.BorrowRecords.GetBorrowHistoryByBookCopyIdAsync(
            bookCopyId,
            cancellationToken
        );
        return records.Select(MapToDto).ToList();
    }

    public async Task<IEnumerable<BorrowRecordDto>> GetActiveRecordsByUserIdAsync(
        int userId,
        CancellationToken cancellationToken = default
    )
    {
        var records = await unitOfWork.BorrowRecords.GetActiveRecordsByUserIdAsync(
            userId,
            cancellationToken
        );
        return records.Select(MapToDto).ToList();
    }

    public async Task<IEnumerable<BorrowRecordDto>> GetAllRecordsByUserIdAsync(
        int userId,
        CancellationToken cancellationToken = default
    )
    {
        var records = await unitOfWork.BorrowRecords.GetAllRecordsByUserIdAsync(
            userId,
            cancellationToken
        );
        return records.Select(MapToDto).ToList();
    }

    public async Task<IEnumerable<BorrowRecordDto>> GetAllOverdueRecordsAsync(
        CancellationToken cancellationToken = default
    )
    {
        var records = await unitOfWork.BorrowRecords.GetOverdueRecordsAsync(cancellationToken);
        return records.Select(MapToDto).ToList();
    }

    public async Task<BorrowRecordDto> BorrowBookAsync(
        int bookCopyId,
        CreateBorrowRecordDto dto,
        CancellationToken cancellationToken = default
    )
    {
        var bookCopy = await unitOfWork.BookCopy.GetByIdAsync(bookCopyId, cancellationToken);
        if (bookCopy is null)
        {
            throw new NotFoundException(nameof(BookCopy), bookCopyId);
        }

        var user = await unitOfWork.Users.GetByIdAsync(dto.UserId, cancellationToken);
        if (user is null)
        {
            throw new NotFoundException(nameof(User), dto.UserId);
        }

        if (!user.IsActive)
        {
            throw new InvalidOperationException("Only active users can borrow books.");
        }

        if (!bookCopy.IsAvailable())
        {
            throw new InvalidOperationException("The book copy is not available for borrowing.");
        }

        var activeRecord = await unitOfWork.BorrowRecords.GetActiveRecordByBookCopyIdAsync(
            bookCopyId,
            cancellationToken
        );
        if (activeRecord is not null)
        {
            throw new InvalidOperationException(
                "The book copy already has an active borrowing record."
            );
        }

        var borrowDate = DateTime.UtcNow;
        var record = new BorrowRecord
        {
            BookCopyId = bookCopyId,
            BookCopy = bookCopy,
            UserId = dto.UserId,
            User = user,
            BorrowDate = borrowDate,
            DueDate = borrowDate.AddDays(dto.BorrowingPeriodDays),
            Notes = dto.Notes,
        };

        bookCopy.Status = BookStatus.Borrowed;
        await unitOfWork.BorrowRecords.AddAsync(record, cancellationToken);
        await unitOfWork.BookCopy.UpdateAsync(bookCopy, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return MapToDto(record);
    }

    public async Task ReturnBorrowedBookAsync(
        int bookCopyId,
        ReturnBorrowedBookDto dto,
        CancellationToken cancellationToken = default
    )
    {
        var bookCopy = await unitOfWork.BookCopy.GetByIdAsync(bookCopyId, cancellationToken);
        if (bookCopy is null)
        {
            throw new NotFoundException(nameof(BookCopy), bookCopyId);
        }

        var record = await unitOfWork.BorrowRecords.GetActiveRecordByBookCopyIdAsync(
            bookCopyId,
            cancellationToken
        );
        if (record is null)
        {
            throw new InvalidOperationException("The book copy has no active borrowing record.");
        }

        record.ReturnDate = DateTime.UtcNow;
        record.Notes = dto.Notes;
        bookCopy.Status = BookStatus.Available;

        await unitOfWork.BorrowRecords.UpdateAsync(record, cancellationToken);
        await unitOfWork.BookCopy.UpdateAsync(bookCopy, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteRecordAsync(int id, CancellationToken cancellationToken = default)
    {
        var record = await unitOfWork.BorrowRecords.GetByIdAsync(id, cancellationToken);
        if (record is null)
        {
            throw new NotFoundException(nameof(BorrowRecord), id);
        }

        if (record.ReturnDate is null)
        {
            record.BookCopy.Status = BookStatus.Available;
            await unitOfWork.BookCopy.UpdateAsync(record.BookCopy, cancellationToken);
        }

        await unitOfWork.BorrowRecords.DeleteAsync(id, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }

    private static BorrowRecordDto MapToDto(BorrowRecord record)
    {
        var bookCopy = record.BookCopy;
        var book = bookCopy?.Book;
        var user = record.User;

        return new BorrowRecordDto(
            record.Id,
            record.BorrowDate,
            record.DueDate,
            record.ReturnDate,
            bookCopy?.BookId ?? 0,
            record.BookCopyId,
            book?.Title ?? string.Empty,
            book?.ISBN ?? string.Empty,
            record.UserId,
            user?.FirstName ?? string.Empty,
            user?.LastName ?? string.Empty,
            record.Notes,
            record.IsOverdue
        );
    }
}
