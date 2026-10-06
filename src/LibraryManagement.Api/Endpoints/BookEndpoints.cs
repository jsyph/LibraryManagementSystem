using System.Threading;
using System.Threading.Tasks;
using LibraryManagement.Application.Common.Interfaces.Services;
using LibraryManagement.Application.DTOs;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace LibraryManagement.Api.Endpoints;

public static class BookEndpoints
{
    public static RouteGroupBuilder MapBookEndpoints(this WebApplication app)
    {
        var bookItems = app.MapGroup("/book");

        bookItems.MapGet("/search", SearchBooksAsync).WithSummary("Search books");
        bookItems.MapGet("/{id:int}", GetBookByIdAsync).WithSummary("Get a book by ID");
        bookItems.MapGet("/", GetAllBooksAsync).WithSummary("Get all books");
        bookItems
            .MapGet("/author/{authorId:int}", GetBooksByAuthorIdAsync)
            .WithSummary("Get books by author ID");
        bookItems
            .MapGet("/category/{categoryId:int}", GetBooksByCategoryAsync)
            .WithSummary("Get books by category ID");
        bookItems.MapGet("/isbn/{isbn}", GetBookByIsbnAsync).WithSummary("Get a book by ISBN");
        bookItems.MapPost("/", AddBookAsync).WithSummary("Create a book");
        bookItems.MapPut("/{id:int}", UpdateBookAsync).WithSummary("Update a book");
        bookItems.MapDelete("/{id:int}", DeleteBookAsync).WithSummary("Delete a book");

        return bookItems;
    }

    private static async Task<IResult> SearchBooksAsync(
        [AsParameters] TitleDescriptionBookSearchFilterDto dto,
        IBookService bookService,
        CancellationToken cancellationToken
    ) => TypedResults.Ok(await bookService.SearchBooksAsync(dto, cancellationToken));

    private static async Task<IResult> GetBookByIdAsync(
        int id,
        IBookService bookService,
        CancellationToken cancellationToken
    )
    {
        var book = await bookService.GetBookByIdAsync(id, cancellationToken);
        return book is null ? TypedResults.NotFound() : TypedResults.Ok(book);
    }

    private static async Task<IResult> GetAllBooksAsync(
        IBookService bookService,
        CancellationToken cancellationToken
    ) => TypedResults.Ok(await bookService.GetAllBooksAsync(cancellationToken));

    private static async Task<IResult> GetBooksByAuthorIdAsync(
        int authorId,
        IBookService bookService,
        CancellationToken cancellationToken
    ) => TypedResults.Ok(await bookService.GetBooksByAuthorIdAsync(authorId, cancellationToken));

    private static async Task<IResult> GetBooksByCategoryAsync(
        int categoryId,
        IBookService bookService,
        CancellationToken cancellationToken
    ) => TypedResults.Ok(await bookService.GetBooksByCategory(categoryId, cancellationToken));

    private static async Task<IResult> GetBookByIsbnAsync(
        string isbn,
        IBookService bookService,
        CancellationToken cancellationToken
    )
    {
        var book = await bookService.GetBookByIsbnAsync(isbn, cancellationToken);
        return book is null ? TypedResults.NotFound() : TypedResults.Ok(book);
    }

    private static async Task<IResult> AddBookAsync(
        CreateBookDto dto,
        IBookService bookService,
        CancellationToken cancellationToken
    )
    {
        var book = await bookService.AddBookAsync(dto, cancellationToken);
        return TypedResults.Created($"/book/{book.Id}", book);
    }

    private static async Task<IResult> UpdateBookAsync(
        int id,
        UpdateBookDto dto,
        IBookService bookService,
        CancellationToken cancellationToken
    )
    {
        await bookService.UpdateBookAsync(id, dto, cancellationToken);
        return TypedResults.NoContent();
    }

    private static async Task<IResult> DeleteBookAsync(
        int id,
        IBookService bookService,
        CancellationToken cancellationToken
    )
    {
        await bookService.DeleteBookAsync(id, cancellationToken);
        return TypedResults.NoContent();
    }
}
