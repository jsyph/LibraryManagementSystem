namespace LibraryManagement.Application.Services;

using LibraryManagement.Application.Common.Exceptions;
using LibraryManagement.Application.Common.Interfaces.Persistence;
using LibraryManagement.Application.Common.Interfaces.Services;
using LibraryManagement.Application.DTOs;
using LibraryManagement.Domain.Entities;

public class CategoryService : ICategoryService
{
    private readonly IUnitOfWork unitOfWork;

    public CategoryService(IUnitOfWork unitOfWork)
    {
        this.unitOfWork = unitOfWork;
    }

    public async Task<CategoryDto?> GetCategoryByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var category = await unitOfWork.Categories.GetByIdAsync(id, cancellationToken);
        return category is null ? null : MapToDto(category);
    }

    public async Task<IEnumerable<CategoryDto>> GetAllCategoriesAsync(
        CancellationToken cancellationToken = default
    )
    {
        var categories = await unitOfWork.Categories.GetAllAsync(cancellationToken);
        return categories.Select(MapToDto).ToList();
    }

    public async Task<CategoryDto> AddCategoryAsync(
        CreateCategoryDto dto,
        CancellationToken cancellationToken = default
    )
    {
        var category = new Category { Name = dto.Name, Description = dto.Description };
        await unitOfWork.Categories.AddAsync(category, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return MapToDto(category);
    }

    public async Task UpdateAsync(
        int id,
        UpdateCategoryDto dto,
        CancellationToken cancellationToken = default
    )
    {
        var category = await GetRequiredCategoryAsync(id, cancellationToken);
        category.Name = dto.Name;
        category.Description = dto.Description;
        await unitOfWork.Categories.UpdateAsync(category, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteCategoryAsync(int id, CancellationToken cancellationToken = default)
    {
        var category = await GetRequiredCategoryAsync(id, cancellationToken);
        await unitOfWork.Categories.DeleteAsync(category, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }

    private async Task<Category> GetRequiredCategoryAsync(
        int id,
        CancellationToken cancellationToken
    ) =>
        await unitOfWork.Categories.GetByIdAsync(id, cancellationToken)
        ?? throw new NotFoundException(nameof(Category), id);

    private static CategoryDto MapToDto(Category category) =>
        new(category.Id, category.Name, category.Description);
}