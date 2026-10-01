namespace LibraryManagement.Application.Common.Interfaces.Services;

using LibraryManagement.Application.DTOs;

public interface IPaymentService
{
    Task<PaymentDto?> GetPaymentByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<PaymentDto>> GetAllPaymentAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<PaymentDto>> GetPaymentsByBorrowRecordIdAsync(
        int borrowRecordId,
        CancellationToken cancellationToken = default
    );
    Task<IEnumerable<PaymentDto>> GetAllPaymentsWithinDateRangeAsync(
        PaymentsWithinDateRangeDto dto,
        CancellationToken cancellationToken = default
    );
    Task<IEnumerable<PaymentDto>> SearchPaymentsAsync(
        SearchPaymentDto dto,
        CancellationToken cancellationToken = default
    );

    Task<PaymentDto> AddPaymentAsync(
        CreatePaymentDto dto,
        CancellationToken cancellationToken = default
    );
    Task ChangePaymentStatusAsync(
        int id,
        ChangePaymentStatusDto dto,
        CancellationToken cancellationToken = default
    );
    Task DeletePaymentAsync(int id, CancellationToken cancellationToken = default);
}
