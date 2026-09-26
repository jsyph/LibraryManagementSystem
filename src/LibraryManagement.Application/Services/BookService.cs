namespace LibraryManagement.Application.Services;

using LibraryManagement.Application.Common.Exceptions;
using LibraryManagement.Application.Common.Interfaces.Persistence;
using LibraryManagement.Application.Common.Interfaces.Services;
using LibraryManagement.Application.DTOs;
using LibraryManagement.Domain.Entities;
using LibraryManagement.Domain.Enums;
using AutoMapper;

public class BookService(IUnitOfWork unitOfWork, IMapper mapper) : IBookService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMapper _mapper = mapper;

    Task<BookDto> IBookService.CreateAsync(CreateBookDto dto, CancellationToken ct)
    {
        throw new NotImplementedException();
    }

    Task IBookService.DeleteAsync(int id, CancellationToken ct)
    {
        throw new NotImplementedException();
    }

    Task<IEnumerable<BookDto>> IBookService.GetAllAsync(CancellationToken ct)
    {
        throw new NotImplementedException();
    }

    Task<IEnumerable<BorrowRecordDto>> IBookService.GetBorrowHistoryAsync(int bookId, CancellationToken ct)
    {
        throw new NotImplementedException();
    }

    Task<IEnumerable<BookDto>> IBookService.GetByAuthorIdAsync(int authorId, CancellationToken ct)
    {
        throw new NotImplementedException();
    }

    Task<IEnumerable<BookDto>> IBookService.GetByCategoryIdAsync(int categoryId, CancellationToken ct)
    {
        throw new NotImplementedException();
    }

    Task<BookDto> IBookService.GetByIdAsync(int id, CancellationToken ct)
    {
        throw new NotImplementedException();
    }

    Task<BookDto?> IBookService.GetByIsbnAsync(string isbn, CancellationToken ct)
    {
        throw new NotImplementedException();
    }

    Task<bool> IBookService.IsAvailableAsync(int bookId, CancellationToken ct)
    {
        throw new NotImplementedException();
    }

    Task<IEnumerable<BookDto>> IBookService.SearchAsync(string searchTerm, CancellationToken ct)
    {
        throw new NotImplementedException();
    }

    Task<IEnumerable<BookDto>> IBookService.SearchByTitleAsync(string title, CancellationToken ct)
    {
        throw new NotImplementedException();
    }

    Task IBookService.UpdateAsync(int id, UpdateBookDto dto, CancellationToken ct)
    {
        throw new NotImplementedException();
    }
}