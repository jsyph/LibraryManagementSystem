using System.Threading;
using System.Threading.Tasks;
using LibraryManagement.Application.Common.Interfaces.Services;
using LibraryManagement.Application.DTOs;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace LibraryManagement.Api.Endpoints;

public static class BookCopyEndpoints
{
    public static RouteGroupBuilder MapBookCopyEndpoints(this WebApplication app)
    {
        var bookCopyItems = app.MapGroup("/book/copies");

        bookCopyItems
            .MapGet("/{id:int}", GetBookCopyByIdAsync)
            .WithSummary("Get a book copy by ID");
        bookCopyItems
            .MapGet("/b/{bookId:int}", GetAllBookCopiesByBookIdAsync)
            .WithSummary("Get book copies by book ID");
        bookCopyItems
            .MapGet("/b/{bookId:int}/all", GetAllBookCopiesAsync)
            .WithSummary("Get all copies of a book");
        bookCopyItems
            .MapGet("/status/{bookStatus:int}", GetBookCopiesByStatusAsync)
            .WithSummary("Get book copies by status");
        bookCopyItems
            .MapGet("/b/{bookId:int}/available", GetAvailableBookCopiesAsync)
            .WithSummary("Get available copies of a book");
        bookCopyItems
            .MapPost("/b/{bookId:int}", AddBookCopiesAsync)
            .WithSummary("Add copies of a book");
        bookCopyItems
            .MapPatch("/{id:int}/status", UpdateBookCopyStatusAsync)
            .WithSummary("Update a book copy status");
        bookCopyItems.MapDelete("/{id:int}", DeleteBookCopyAsync).WithSummary("Delete a book copy");

        return bookCopyItems;
    }

    private static async Task<IResult> GetBookCopyByIdAsync(
        int id,
        IBookCopyService bookCopyService,
        CancellationToken cancellationToken
    )
    {
        var bookCopy = await bookCopyService.GetBookCopyByIdAsync(id, cancellationToken);
        return bookCopy is null ? TypedResults.NotFound() : TypedResults.Ok(bookCopy);
    }

    private static async Task<IResult> GetAllBookCopiesAsync(
        int bookId,
        IBookCopyService bookCopyService,
        CancellationToken cancellationToken
    ) => TypedResults.Ok(await bookCopyService.GetAllBookCopiesAsync(bookId, cancellationToken));

    private static async Task<IResult> GetAllBookCopiesByBookIdAsync(
        int bookId,
        IBookCopyService bookCopyService,
        CancellationToken cancellationToken
    ) =>
        TypedResults.Ok(
            await bookCopyService.GetAllBookCopiesByBookIdAsync(bookId, cancellationToken)
        );

    private static async Task<IResult> GetBookCopiesByStatusAsync(
        int bookStatus,
        IBookCopyService bookCopyService,
        CancellationToken cancellationToken
    ) =>
        TypedResults.Ok(
            await bookCopyService.GetBookCopiesByStatusAsync(bookStatus, cancellationToken)
        );

    private static async Task<IResult> GetAvailableBookCopiesAsync(
        int bookId,
        IBookCopyService bookCopyService,
        CancellationToken cancellationToken
    ) =>
        TypedResults.Ok(
            await bookCopyService.GetAvilableBookCopiesAsync(bookId, cancellationToken)
        );

    private static async Task<IResult> AddBookCopiesAsync(
        int bookId,
        AddBookCopiesDto dto,
        IBookCopyService bookCopyService,
        CancellationToken cancellationToken
    )
    {
        var bookCopies = await bookCopyService.AddBookCopiesAsync(bookId, dto, cancellationToken);
        return TypedResults.Created($"/book/copies/book/{bookId}", bookCopies);
    }

    private static async Task<IResult> UpdateBookCopyStatusAsync(
        int id,
        ChangeBookCopyStatusDto dto,
        IBookCopyService bookCopyService,
        CancellationToken cancellationToken
    )
    {
        await bookCopyService.UpdateBookCopyStatusAsync(id, dto, cancellationToken);
        return TypedResults.NoContent();
    }

    private static async Task<IResult> DeleteBookCopyAsync(
        int id,
        IBookCopyService bookCopyService,
        CancellationToken cancellationToken
    )
    {
        await bookCopyService.DeleteBookCopyAsync(id, cancellationToken);
        return TypedResults.NoContent();
    }
}
