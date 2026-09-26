namespace LibraryManagement.Application.Services;

using LibraryManagement.Application.Common.Exceptions;
using LibraryManagement.Application.Common.Interfaces.Persistence;
using LibraryManagement.Application.Common.Interfaces.Services;
using LibraryManagement.Application.DTOs;
using LibraryManagement.Domain.Entities;
using LibraryManagement.Domain.Enums;
using AutoMapper;

class CategoryService(IUnitOfWork unitOfWork, IMapper mapper) : ICategoryService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMapper _mapper = mapper;

    Task<CategoryDto> ICategoryService.CreateAsync(CreateCategoryDto dto, CancellationToken ct)
    {
        throw new NotImplementedException();
    }

    Task ICategoryService.DeleteAsync(int id, CancellationToken ct)
    {
        throw new NotImplementedException();
    }

    Task<IEnumerable<CategoryDto>> ICategoryService.GetAllAsync(CancellationToken ct)
    {
        throw new NotImplementedException();
    }

    Task<IEnumerable<BookDto>> ICategoryService.GetBooksByCategoryIdAsync(int categoryId, CancellationToken ct)
    {
        throw new NotImplementedException();
    }

    Task<CategoryDto> ICategoryService.GetByIdAsync(int id, CancellationToken ct)
    {
        throw new NotImplementedException();
    }

    Task ICategoryService.UpdateAsync(int id, UpdateCategoryDto dto, CancellationToken ct)
    {
        throw new NotImplementedException();
    }
}