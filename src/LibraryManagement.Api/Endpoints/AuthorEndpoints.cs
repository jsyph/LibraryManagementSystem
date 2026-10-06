using System.Threading;
using System.Threading.Tasks;
using LibraryManagement.Application.Common.Interfaces.Services;
using LibraryManagement.Application.DTOs;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace LibraryManagement.Api.Endpoints;

public static class AuthorEndpoints
{
    public static RouteGroupBuilder MapAuthorEndpoints(this WebApplication app)
    {
        var authorItems = app.MapGroup("/author");

        authorItems.MapGet("/search", SearchAuthorsAsync).WithSummary("Search authors");
        authorItems.MapGet("/{id:int}", GetAuthorByIdAsync).WithSummary("Get an author by ID");
        authorItems.MapPut("/{id:int}", UpdateAuthorAsync).WithSummary("Update an author");
        authorItems.MapDelete("/{id:int}", DeleteAuthorAsync).WithSummary("Delete an author");
        authorItems.MapGet("/", GetAllAuthorsAsync).WithSummary("Get all authors");
        authorItems.MapPost("/", AddAuthorAsync).WithSummary("Create an author");

        return authorItems;
    }

    private static async Task<IResult> SearchAuthorsAsync(
        [AsParameters] SearchAuthorDto dto,
        IAuthorService authorService,
        CancellationToken cancellationToken
    )
    {
        return TypedResults.Ok(await authorService.SearchAuthorsAsync(dto, cancellationToken));
    }

    private static async Task<IResult> GetAuthorByIdAsync(
        int id,
        IAuthorService authorService,
        CancellationToken cancellationToken
    )
    {
        var author = await authorService.GetAuthorByIdAsync(id, cancellationToken);
        return author is null ? TypedResults.NotFound() : TypedResults.Ok(author);
    }

    private static async Task<IResult> GetAllAuthorsAsync(
        IAuthorService authorService,
        CancellationToken cancellationToken
    ) => TypedResults.Ok(await authorService.GetAllAuthorsAsync(cancellationToken));

    private static async Task<IResult> AddAuthorAsync(
        CreateAuthorDto dto,
        IAuthorService authorService,
        CancellationToken cancellationToken
    )
    {
        var author = await authorService.AddAuthorAsync(dto, cancellationToken);
        return TypedResults.Created($"/author/{author.Id}", author);
    }

    private static async Task<IResult> UpdateAuthorAsync(
        int id,
        UpdateAuthorDto dto,
        IAuthorService authorService,
        CancellationToken cancellationToken
    )
    {
        await authorService.UpdateAuthorAsync(id, dto, cancellationToken);
        return TypedResults.NoContent();
    }

    private static async Task<IResult> DeleteAuthorAsync(
        int id,
        IAuthorService authorService,
        CancellationToken cancellationToken
    )
    {
        await authorService.DeleteAuthorAsync(id, cancellationToken);
        return TypedResults.NoContent();
    }
}
