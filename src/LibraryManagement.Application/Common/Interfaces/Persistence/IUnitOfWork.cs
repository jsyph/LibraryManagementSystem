namespace LibraryManagement.Application.Common.Interfaces;

public interface IUnitOfWork
{
    IBookRepository Books { get; }
    IAuthorRepository Authors { get; }
    ICategoryRepository Categories { get; }
    IMemberRepository Members { get; }
    IBorrowRecordRepository BorrowRecords { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}