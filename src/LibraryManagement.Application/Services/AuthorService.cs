namespace LibraryManagement.Application.Services;

using LibraryManagement.Application.Common.Exceptions;
using LibraryManagement.Application.Common.Interfaces.Persistence;
using LibraryManagement.Application.Common.Interfaces.Services;
using LibraryManagement.Application.DTOs;
using LibraryManagement.Domain.Entities;
using LibraryManagement.Domain.Enums;
using AutoMapper;

public class AuthorService(IUnitOfWork unitOfWork, IMapper mapper) : IAuthorService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMapper _mapper = mapper;

    Task<AuthorDto> IAuthorService.CreateAsync(CreateAuthorDto dto, CancellationToken ct)
    {
        throw new NotImplementedException();
    }

    Task IAuthorService.DeleteAsync(int id, CancellationToken ct)
    {
        throw new NotImplementedException();
    }

    Task<IEnumerable<AuthorDto>> IAuthorService.GetAllAsync(CancellationToken ct)
    {
        throw new NotImplementedException();
    }

    Task<IEnumerable<BookDto>> IAuthorService.GetBooksByAuthorIdAsync(int authorId, CancellationToken ct)
    {
        throw new NotImplementedException();
    }

    Task<AuthorDto> IAuthorService.GetByIdAsync(int id, CancellationToken ct)
    {
        throw new NotImplementedException();
    }

    Task IAuthorService.UpdateAsync(int id, UpdateAuthorDto dto, CancellationToken ct)
    {
        throw new NotImplementedException();
    }
}