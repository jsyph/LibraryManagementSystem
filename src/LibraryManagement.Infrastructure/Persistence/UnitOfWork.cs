using LibraryManagement.Application.Common.Interfaces.Persistence;
using LibraryManagement.Infrastructure.Persistence.Repositories;

namespace LibraryManagement.Infrastructure.Persistence;

public sealed class UnitOfWork : IUnitOfWork
{
    private readonly LibraryDbContext context;

    public UnitOfWork(LibraryDbContext context)
    {
        this.context = context;
        Books = new BookRepository(context);
        BookCopy = new BookCopyRepository(context);
        BorrowRecords = new BorrowRecordRepository(context);
        Categories = new CategoryRepository(context);
        Authors = new AuthorRepository(context);
        Users = new UserRepository(context);
        Payments = new PaymentRepository(context);
        Roles = new RoleRepository(context);
    }

    public IBookRepository Books { get; }
    public IBookCopyRepository BookCopy { get; }
    public IBorrowRecordRepository BorrowRecords { get; }
    public ICategoryRepository Categories { get; }
    public IAuthorRepository Authors { get; }
    public IUserRepository Users { get; }
    public IPaymentRepository Payments { get; }
    public IRoleRepository Roles { get; }

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) =>
        context.SaveChangesAsync(cancellationToken);
}
