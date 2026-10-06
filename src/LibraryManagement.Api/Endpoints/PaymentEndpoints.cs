using LibraryManagement.Application.Common.Interfaces.Services;
using LibraryManagement.Application.DTOs;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace LibraryManagement.Api.Endpoints;

public static class PaymentEndpoints
{
    public static RouteGroupBuilder MapPaymentEndpoints(this WebApplication app)
    {
        var paymentItems = app.MapGroup("/payment");

        paymentItems.MapGet("/search", SearchPaymentsAsync)
            .WithSummary("Search payments");
        paymentItems.MapGet("/{id:int}", GetPaymentByIdAsync)
            .WithSummary("Get a payment by ID");
        paymentItems.MapGet("/", GetAllPaymentsAsync)
            .WithSummary("Get all payments");
        paymentItems.MapGet("/borrow/{borrowRecordId:int}", GetPaymentsByBorrowRecordIdAsync)
            .WithSummary("Get payments for a borrowing record");
        paymentItems.MapGet("/date-range", GetAllPaymentsWithinDateRangeAsync)
            .WithSummary("Get payments within a date range");
        paymentItems.MapPost("/", AddPaymentAsync)
            .WithSummary("Create a payment");
        paymentItems.MapPatch("/{id:int}/status", ChangePaymentStatusAsync)
            .WithSummary("Change a payment status");
        paymentItems.MapDelete("/{id:int}", DeletePaymentAsync)
            .WithSummary("Delete a payment");

        return paymentItems;
    }

    private static async Task<IResult> GetPaymentByIdAsync(
        int id,
        IPaymentService paymentService,
        CancellationToken cancellationToken
    )
    {
        var payment = await paymentService.GetPaymentByIdAsync(id, cancellationToken);
        return payment is null ? TypedResults.NotFound() : TypedResults.Ok(payment);
    }

    private static async Task<IResult> GetAllPaymentsAsync(
        IPaymentService paymentService,
        CancellationToken cancellationToken
    ) => TypedResults.Ok(await paymentService.GetAllPaymentAsync(cancellationToken));

    private static async Task<IResult> GetPaymentsByBorrowRecordIdAsync(
        int borrowRecordId,
        IPaymentService paymentService,
        CancellationToken cancellationToken
    ) =>
        TypedResults.Ok(
            await paymentService.GetPaymentsByBorrowRecordIdAsync(borrowRecordId, cancellationToken)
        );

    private static async Task<IResult> GetAllPaymentsWithinDateRangeAsync(
        [AsParameters] PaymentsWithinDateRangeDto dto,
        IPaymentService paymentService,
        CancellationToken cancellationToken
    ) =>
        TypedResults.Ok(
            await paymentService.GetAllPaymentsWithinDateRangeAsync(dto, cancellationToken)
        );

    private static async Task<IResult> SearchPaymentsAsync(
        [AsParameters] SearchPaymentDto dto,
        IPaymentService paymentService,
        CancellationToken cancellationToken
    ) => TypedResults.Ok(await paymentService.SearchPaymentsAsync(dto, cancellationToken));

    private static async Task<IResult> AddPaymentAsync(
        CreatePaymentDto dto,
        IPaymentService paymentService,
        CancellationToken cancellationToken
    )
    {
        var payment = await paymentService.AddPaymentAsync(dto, cancellationToken);
        return TypedResults.Created($"/payment/{payment.Id}", payment);
    }

    private static async Task<IResult> ChangePaymentStatusAsync(
        int id,
        ChangePaymentStatusDto dto,
        IPaymentService paymentService,
        CancellationToken cancellationToken
    )
    {
        await paymentService.ChangePaymentStatusAsync(id, dto, cancellationToken);
        return TypedResults.NoContent();
    }

    private static async Task<IResult> DeletePaymentAsync(
        int id,
        IPaymentService paymentService,
        CancellationToken cancellationToken
    )
    {
        await paymentService.DeletePaymentAsync(id, cancellationToken);
        return TypedResults.NoContent();
    }
}
