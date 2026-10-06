using LibraryManagement.Application.Common.Interfaces.Persistence;
using LibraryManagement.Domain.Entities;
using LibraryManagement.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Infrastructure.Persistence.Repositories;

public sealed class PaymentRepository : IPaymentRepository
{
    private readonly DbSet<Payment> DbSet;

    public PaymentRepository(LibraryDbContext context)
    {
        DbSet = context.Set<Payment>();
    }

    public async Task<Payment?> GetByIdAsync(
        int id,
        CancellationToken cancellationToken = default
    ) => await DbSet.FirstOrDefaultAsync(payment => payment.Id == id, cancellationToken);

    public async Task<IEnumerable<Payment>> GetAllAsync(
        CancellationToken cancellationToken = default
    ) => await DbSet.AsNoTracking().ToListAsync(cancellationToken);

    public async Task AddAsync(Payment payment, CancellationToken cancellationToken = default) =>
        await DbSet.AddAsync(payment, cancellationToken);

    public Task UpdateAsync(Payment payment, CancellationToken cancellationToken = default)
    {
        DbSet.Update(payment);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(Payment payment, CancellationToken cancellationToken = default)
    {
        DbSet.Remove(payment);
        return Task.CompletedTask;
    }

    public Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default) =>
        DbSet.AnyAsync(payment => payment.Id == id, cancellationToken);

    public async Task<IEnumerable<Payment>> GetByBorrowRecordIdAsync(
        int borrowRecordId,
        CancellationToken cancellationToken = default
    ) =>
        await DbSet
            .AsNoTracking()
            .Where(payment => payment.BorrowRecordId == borrowRecordId)
            .ToListAsync(cancellationToken);

    public async Task<IEnumerable<Payment>> GetAllWithinDateRangeAsync(
        DateTime startDate,
        DateTime endDate,
        CancellationToken cancellationToken = default
    ) =>
        await DbSet
            .AsNoTracking()
            .Where(payment => payment.PaymentDate >= startDate && payment.PaymentDate <= endDate)
            .ToListAsync(cancellationToken);

    public async Task<IEnumerable<Payment>> SearchPaymentAsync(
        PaymentMethod? paymentMethod,
        DateTime? paymentDate,
        PaymentStatus? status,
        string? notes,
        CancellationToken cancellationToken = default
    )
    {
        var query = DbSet.AsNoTracking().AsQueryable();
        if (paymentMethod.HasValue)
            query = query.Where(payment => payment.PaymentMethod == paymentMethod.Value);
        if (paymentDate.HasValue)
            query = query.Where(payment => payment.PaymentDate.Date == paymentDate.Value.Date);
        if (status.HasValue)
            query = query.Where(payment => payment.Status == status.Value);
        if (!string.IsNullOrWhiteSpace(notes))
        {
            var payments = await query.ToListAsync(cancellationToken);
            return FuzzySearch.Rank(payments, notes, payment => [payment.Notes]);
        }
        return await query.ToListAsync(cancellationToken);
    }
}
