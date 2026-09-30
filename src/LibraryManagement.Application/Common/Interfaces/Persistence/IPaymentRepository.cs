namespace LibraryManagement.Application.Common.Interfaces.Persistence;

using LibraryManagement.Domain.Entities;
using LibraryManagement.Domain.Enums;

public interface IPaymentRepository
{
    Task<Payment?> GetByIdAsync(int id);
    Task<IEnumerable<Payment>> GetAllAsync();
    Task<IEnumerable<Payment>> GetByBorrowRecordIdAsync(int borrowRecordId);
    Task<IEnumerable<Payment>> GetByPaymentMethodAsync(PaymentMethod paymentMethod);
    Task<IEnumerable<Payment>> GetByDateAsync(DateTime date);
    Task<IEnumerable<Payment>> GetByStatusAsync(PaymentStatus status);
    Task AddAsync(Payment payment);
    Task UpdateAsync(Payment payment);
    Task DeleteAsync(Payment payment);
    Task<bool> ExistsAsync(int id);
}