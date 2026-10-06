using LibraryManagement.Application.Common.Interfaces.Persistence;
using LibraryManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Infrastructure.Persistence.Repositories;

public sealed class BorrowRecordRepository : IBorrowRecordRepository
{
    private readonly DbSet<BorrowRecord> DbSet;

    public BorrowRecordRepository(LibraryDbContext context)
    {
        DbSet = context.Set<BorrowRecord>();
    }

    private IQueryable<BorrowRecord> WithDetails() =>
        DbSet
            .Include(record => record.BookCopy)
                .ThenInclude(copy => copy.Book)
            .Include(record => record.User);

    public async Task<BorrowRecord?> GetByIdAsync(
        int id,
        CancellationToken cancellationToken = default
    ) => await WithDetails().FirstOrDefaultAsync(record => record.Id == id, cancellationToken);

    public async Task<BorrowRecord?> GetActiveRecordByBookCopyIdAsync(
        int bookCopyId,
        CancellationToken cancellationToken = default
    ) =>
        await WithDetails()
            .FirstOrDefaultAsync(
                record => record.BookCopyId == bookCopyId && record.ReturnDate == null,
                cancellationToken
            );

    public async Task<IEnumerable<BorrowRecord>> GetBorrowHistoryByBookCopyIdAsync(
        int bookCopyId,
        CancellationToken cancellationToken = default
    ) =>
        await WithDetails()
            .AsNoTracking()
            .Where(record => record.BookCopyId == bookCopyId)
            .OrderByDescending(record => record.BorrowDate)
            .ToListAsync(cancellationToken);

    public async Task<IEnumerable<BorrowRecord>> GetActiveRecordsByUserIdAsync(
        int userId,
        CancellationToken cancellationToken = default
    ) =>
        await WithDetails()
            .AsNoTracking()
            .Where(record => record.UserId == userId && record.ReturnDate == null)
            .ToListAsync(cancellationToken);

    public async Task<IEnumerable<BorrowRecord>> GetAllRecordsByUserIdAsync(
        int userId,
        CancellationToken cancellationToken = default
    ) =>
        await WithDetails()
            .AsNoTracking()
            .Where(record => record.UserId == userId)
            .ToListAsync(cancellationToken);

    public async Task<IEnumerable<BorrowRecord>> GetOverdueRecordsAsync(
        CancellationToken cancellationToken = default
    ) =>
        await WithDetails()
            .AsNoTracking()
            .Where(record => record.ReturnDate == null && record.DueDate < DateTime.UtcNow)
            .ToListAsync(cancellationToken);

    public async Task<IEnumerable<BorrowRecord>> GetAllAsync(
        CancellationToken cancellationToken = default
    ) => await WithDetails().AsNoTracking().ToListAsync(cancellationToken);

    public async Task AddAsync(
        BorrowRecord record,
        CancellationToken cancellationToken = default
    ) => await DbSet.AddAsync(record, cancellationToken);

    public Task UpdateAsync(BorrowRecord record, CancellationToken cancellationToken = default)
    {
        DbSet.Update(record);
        return Task.CompletedTask;
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var record = await DbSet.FindAsync([id], cancellationToken);
        if (record is not null)
            DbSet.Remove(record);
    }
}
