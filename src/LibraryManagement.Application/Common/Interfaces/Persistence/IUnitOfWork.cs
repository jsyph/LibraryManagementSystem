namespace LibraryManagement.Application.Common.Interfaces.Persistence;

public interface IUnitOfWork
{
    IBookRepository Books { get; }
    IBookCopyRepository BookCopy { get; }
    IBorrowRecordRepository BorrowRecords { get; }
    ICategoryRepository Categories { get; }
    IAuthorRepository Authors { get; }
    IUserRepository Users { get; }
    IPaymentRepository Payments { get; }
    IRoleRepository Roles { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
