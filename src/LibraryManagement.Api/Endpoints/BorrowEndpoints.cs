using LibraryManagement.Application.Common.Interfaces.Services;
using LibraryManagement.Application.DTOs;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace LibraryManagement.Api.Endpoints;

public static class BorrowEndpoints
{
    public static RouteGroupBuilder MapBorrowEndpoints(this WebApplication app)
    {
        var borrowItems = app.MapGroup("/book/borrow");

        borrowItems.MapGet("/{id:int}", GetRecordByIdAsync)
            .WithSummary("Get a borrowing record by ID");
        borrowItems.MapGet("/copy/{bookCopyId:int}/active", GetActiveRecordByBookCopyIdAsync)
            .WithSummary("Get the active borrowing record for a book copy");
        borrowItems.MapGet("/copy/{bookCopyId:int}", GetAllRecordsByBookCopyIdAsync)
            .WithSummary("Get borrowing records for a book copy");
        borrowItems.MapGet("/user/{userId:int}/active", GetActiveRecordsByUserIdAsync)
            .WithSummary("Get active borrowing records for a user");
        borrowItems.MapGet("/user/{userId:int}", GetAllRecordsByUserIdAsync)
            .WithSummary("Get borrowing records for a user");
        borrowItems.MapGet("/overdue", GetAllOverdueRecordsAsync)
            .WithSummary("Get all overdue borrowing records");
        borrowItems.MapPost("/copy/{bookCopyId:int}", BorrowBookAsync)
            .WithSummary("Borrow a book copy");
        borrowItems.MapPut("/copy/{bookCopyId:int}/return", ReturnBorrowedBookAsync)
            .WithSummary("Return a borrowed book copy");
        borrowItems.MapDelete("/{id:int}", DeleteRecordAsync)
            .WithSummary("Delete a borrowing record");

        return borrowItems;
    }

    private static async Task<IResult> GetRecordByIdAsync(
        int id,
        IBorrowingService borrowingService,
        CancellationToken cancellationToken
    )
    {
        var record = await borrowingService.GetRecordByIdAsync(id, cancellationToken);
        return record is null ? TypedResults.NotFound() : TypedResults.Ok(record);
    }

    private static async Task<IResult> GetActiveRecordByBookCopyIdAsync(
        int bookCopyId,
        IBorrowingService borrowingService,
        CancellationToken cancellationToken
    )
    {
        var record = await borrowingService.GetActiveRecordByBookCopyIdAsync(
            bookCopyId,
            cancellationToken
        );
        return record is null ? TypedResults.NotFound() : TypedResults.Ok(record);
    }

    private static async Task<IResult> GetAllRecordsByBookCopyIdAsync(
        int bookCopyId,
        IBorrowingService borrowingService,
        CancellationToken cancellationToken
    ) =>
        TypedResults.Ok(
            await borrowingService.GetAllRecordsByBookCopyIdAsync(bookCopyId, cancellationToken)
        );

    private static async Task<IResult> GetActiveRecordsByUserIdAsync(
        int userId,
        IBorrowingService borrowingService,
        CancellationToken cancellationToken
    ) =>
        TypedResults.Ok(
            await borrowingService.GetActiveRecordsByUserIdAsync(userId, cancellationToken)
        );

    private static async Task<IResult> GetAllRecordsByUserIdAsync(
        int userId,
        IBorrowingService borrowingService,
        CancellationToken cancellationToken
    ) =>
        TypedResults.Ok(
            await borrowingService.GetAllRecordsByUserIdAsync(userId, cancellationToken)
        );

    private static async Task<IResult> GetAllOverdueRecordsAsync(
        IBorrowingService borrowingService,
        CancellationToken cancellationToken
    ) => TypedResults.Ok(await borrowingService.GetAllOverdueRecordsAsync(cancellationToken));

    private static async Task<IResult> BorrowBookAsync(
        int bookCopyId,
        CreateBorrowRecordDto dto,
        IBorrowingService borrowingService,
        CancellationToken cancellationToken
    )
    {
        var record = await borrowingService.BorrowBookAsync(bookCopyId, dto, cancellationToken);
        return TypedResults.Created($"/book/borrow/{record.Id}", record);
    }

    private static async Task<IResult> ReturnBorrowedBookAsync(
        int bookCopyId,
        ReturnBorrowedBookDto dto,
        IBorrowingService borrowingService,
        CancellationToken cancellationToken
    )
    {
        await borrowingService.ReturnBorrowedBookAsync(bookCopyId, dto, cancellationToken);
        return TypedResults.NoContent();
    }

    private static async Task<IResult> DeleteRecordAsync(
        int id,
        IBorrowingService borrowingService,
        CancellationToken cancellationToken
    )
    {
        await borrowingService.DeleteRecordAsync(id, cancellationToken);
        return TypedResults.NoContent();
    }
}
