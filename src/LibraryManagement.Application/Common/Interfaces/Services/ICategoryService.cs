namespace LibraryManagement.Application.Common.Interfaces.Services;

using LibraryManagement.Application.DTOs;

public interface ICategoryService
{
    Task<CategoryDto?> GetCategoryByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<CategoryDto>> GetAllCategoriesAsync(
        CancellationToken cancellationToken = default
    );
    Task<CategoryDto> AddCategoryAsync(
        CreateCategoryDto dto,
        CancellationToken cancellationToken = default
    );
    Task UpdateAsync(int id, UpdateCategoryDto dto, CancellationToken cancellationToken = default);
    Task DeleteCategoryAsync(int id, CancellationToken cancellationToken = default);
}
