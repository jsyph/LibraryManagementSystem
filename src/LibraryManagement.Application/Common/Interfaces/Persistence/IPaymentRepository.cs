namespace LibraryManagement.Application.Common.Interfaces.Persistence;

using LibraryManagement.Domain.Entities;
using LibraryManagement.Domain.Enums;

public interface IPaymentRepository
{
    Task<Payment?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<Payment>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<Payment>> GetByBorrowRecordIdAsync(
        int borrowRecordId,
        CancellationToken cancellationToken = default
    );
    Task<IEnumerable<Payment>> GetAllWithinDateRangeAsync(
        DateTime startDate,
        DateTime endDate,
        CancellationToken cancellationToken = default
    );
    Task<IEnumerable<Payment>> SearchPaymentAsync(
        PaymentMethod? PaymentMethod,
        DateTime? PaymentDate,
        PaymentStatus? Status,
        string? Notes,
        CancellationToken cancellationToken = default
    );

    Task AddAsync(Payment payment, CancellationToken cancellationToken = default);
    Task UpdateAsync(Payment payment, CancellationToken cancellationToken = default);
    Task DeleteAsync(Payment payment, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default);
}
