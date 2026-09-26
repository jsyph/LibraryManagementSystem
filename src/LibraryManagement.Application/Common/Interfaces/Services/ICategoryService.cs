namespace LibraryManagement.Application.Common.Interfaces.Services;

using LibraryManagement.Application.DTOs;

public interface ICategoryService
{
    Task<CategoryDto> GetByIdAsync(int id, CancellationToken ct = default);
    Task<IEnumerable<CategoryDto>> GetAllAsync(CancellationToken ct = default);
    Task<CategoryDto> CreateAsync(CreateCategoryDto dto, CancellationToken ct = default);
    Task UpdateAsync(int id, UpdateCategoryDto dto, CancellationToken ct = default);
    Task DeleteAsync(int id, CancellationToken ct = default);
    
    Task<IEnumerable<BookDto>> GetBooksByCategoryIdAsync(int categoryId, CancellationToken ct = default);
}