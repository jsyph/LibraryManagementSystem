namespace LibraryManagement.Application.Services;

using LibraryManagement.Application.Common.Exceptions;
using LibraryManagement.Application.Common.Interfaces.Persistence;
using LibraryManagement.Application.Common.Interfaces.Services;
using LibraryManagement.Application.DTOs;
using LibraryManagement.Domain.Entities;

public class PaymentService : IPaymentService
{
    private readonly IUnitOfWork unitOfWork;

    public PaymentService(IUnitOfWork unitOfWork)
    {
        this.unitOfWork = unitOfWork;
    }

    public async Task<PaymentDto?> GetPaymentByIdAsync(
        int id,
        CancellationToken cancellationToken = default
    )
    {
        var payment = await unitOfWork.Payments.GetByIdAsync(id, cancellationToken);
        return payment is null ? null : MapToDto(payment);
    }

    public async Task<IEnumerable<PaymentDto>> GetAllPaymentAsync(
        CancellationToken cancellationToken = default
    )
    {
        var payments = await unitOfWork.Payments.GetAllAsync(cancellationToken);
        return payments.Select(MapToDto).ToList();
    }

    public async Task<IEnumerable<PaymentDto>> GetPaymentsByBorrowRecordIdAsync(
        int borrowRecordId,
        CancellationToken cancellationToken = default
    )
    {
        var payments = await unitOfWork.Payments.GetByBorrowRecordIdAsync(
            borrowRecordId,
            cancellationToken
        );
        return payments.Select(MapToDto).ToList();
    }

    public async Task<IEnumerable<PaymentDto>> GetAllPaymentsWithinDateRangeAsync(
        PaymentsWithinDateRangeDto dto,
        CancellationToken cancellationToken = default
    )
    {
        var payments = await unitOfWork.Payments.GetAllWithinDateRangeAsync(
            dto.StartTime,
            dto.EndTime,
            cancellationToken
        );
        return payments.Select(MapToDto).ToList();
    }

    public async Task<IEnumerable<PaymentDto>> SearchPaymentsAsync(
        SearchPaymentDto dto,
        CancellationToken cancellationToken = default
    )
    {
        var payments = await unitOfWork.Payments.SearchPaymentAsync(
            dto.PaymentMethod,
            dto.PaymentDate,
            dto.Status,
            dto.Notes,
            cancellationToken
        );

        if (dto.LastStatusChange is not null)
        {
            payments = payments.Where(payment =>
                payment.LastStatusChange.Date == dto.LastStatusChange.Value.Date
            );
        }

        return payments.Select(MapToDto).ToList();
    }

    public async Task<PaymentDto> AddPaymentAsync(
        CreatePaymentDto dto,
        CancellationToken cancellationToken = default
    )
    {
        var borrowRecord = await unitOfWork.BorrowRecords.GetByIdAsync(
            dto.BorrowRecordId,
            cancellationToken
        );
        if (borrowRecord is null)
        {
            throw new NotFoundException(nameof(BorrowRecord), dto.BorrowRecordId);
        }

        var payment = new Payment
        {
            BorrowRecordId = dto.BorrowRecordId,
            BorrowRecord = borrowRecord,
            Amount = dto.Amount,
            PaymentMethod = dto.PaymentMethod,
            Notes = dto.Notes,
        };

        await unitOfWork.Payments.AddAsync(payment, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return MapToDto(payment);
    }

    public async Task ChangePaymentStatusAsync(
        int id,
        ChangePaymentStatusDto dto,
        CancellationToken cancellationToken = default
    )
    {
        var payment = await unitOfWork.Payments.GetByIdAsync(id, cancellationToken);
        if (payment is null)
        {
            throw new NotFoundException(nameof(Payment), id);
        }

        payment.Status = dto.Status;
        payment.LastStatusChange = DateTime.UtcNow;

        await unitOfWork.Payments.UpdateAsync(payment, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task DeletePaymentAsync(int id, CancellationToken cancellationToken = default)
    {
        var payment = await unitOfWork.Payments.GetByIdAsync(id, cancellationToken);
        if (payment is null)
        {
            throw new NotFoundException(nameof(Payment), id);
        }

        await unitOfWork.Payments.DeleteAsync(payment, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }

    private static PaymentDto MapToDto(Payment payment)
    {
        return new PaymentDto(
            payment.Id,
            payment.BorrowRecordId,
            payment.Amount,
            payment.PaymentMethod,
            payment.PaymentDate,
            payment.LastStatusChange,
            payment.Status,
            payment.Notes
        );
    }
}
