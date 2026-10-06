using System.Threading;
using System.Threading.Tasks;
using LibraryManagement.Application.Common.Interfaces.Services;
using LibraryManagement.Application.DTOs;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace LibraryManagement.Api.Endpoints;

public static class CategoryEndpoints
{
    public static RouteGroupBuilder MapCategoryEndpoints(this WebApplication app)
    {
        var categoryItems = app.MapGroup("/category");

        categoryItems.MapGet("/{id:int}", GetCategoryByIdAsync)
            .WithSummary("Get a category by ID");
        categoryItems.MapGet("/", GetAllCategoriesAsync)
            .WithSummary("Get all categories");
        categoryItems.MapPost("/", AddCategoryAsync)
            .WithSummary("Create a category");
        categoryItems.MapPut("/{id:int}", UpdateCategoryAsync)
            .WithSummary("Update a category");
        categoryItems.MapDelete("/{id:int}", DeleteCategoryAsync)
            .WithSummary("Delete a category");

        return categoryItems;
    }

    private static async Task<IResult> GetCategoryByIdAsync(
        int id,
        ICategoryService categoryService,
        CancellationToken cancellationToken
    )
    {
        var category = await categoryService.GetCategoryByIdAsync(id, cancellationToken);
        return category is null ? TypedResults.NotFound() : TypedResults.Ok(category);
    }

    private static async Task<IResult> GetAllCategoriesAsync(
        ICategoryService categoryService,
        CancellationToken cancellationToken
    ) => TypedResults.Ok(await categoryService.GetAllCategoriesAsync(cancellationToken));

    private static async Task<IResult> AddCategoryAsync(
        CreateCategoryDto dto,
        ICategoryService categoryService,
        CancellationToken cancellationToken
    )
    {
        var category = await categoryService.AddCategoryAsync(dto, cancellationToken);
        return TypedResults.Created($"/category/{category.Id}", category);
    }

    private static async Task<IResult> UpdateCategoryAsync(
        int id,
        UpdateCategoryDto dto,
        ICategoryService categoryService,
        CancellationToken cancellationToken
    )
    {
        await categoryService.UpdateAsync(id, dto, cancellationToken);
        return TypedResults.NoContent();
    }

    private static async Task<IResult> DeleteCategoryAsync(
        int id,
        ICategoryService categoryService,
        CancellationToken cancellationToken
    )
    {
        await categoryService.DeleteCategoryAsync(id, cancellationToken);
        return TypedResults.NoContent();
    }
}
