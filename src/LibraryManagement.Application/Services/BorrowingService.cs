namespace LibraryManagement.Application.Services;

using LibraryManagement.Application.Common.Exceptions;
using LibraryManagement.Application.Common.Interfaces.Persistence;
using LibraryManagement.Application.Common.Interfaces.Services;
using LibraryManagement.Application.DTOs;
using LibraryManagement.Domain.Entities;
using LibraryManagement.Domain.Enums;
using AutoMapper;

class BorrowingService(IUnitOfWork unitOfWork, IMapper mapper) : IBorrowingService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMapper _mapper = mapper;

    Task<BorrowRecordDto> IBorrowingService.BorrowBookAsync(BorrowBookRequestDto dto, CancellationToken ct)
    {
        throw new NotImplementedException();
    }

    Task<IEnumerable<BorrowRecordDto>> IBorrowingService.GetActiveBorrowsByMemberAsync(int memberId, CancellationToken ct)
    {
        throw new NotImplementedException();
    }

    Task<IEnumerable<BorrowRecordDto>> IBorrowingService.GetBookBorrowHistoryAsync(int bookId, CancellationToken ct)
    {
        throw new NotImplementedException();
    }

    Task<BorrowRecordDto> IBorrowingService.GetByIdAsync(int id, CancellationToken ct)
    {
        throw new NotImplementedException();
    }

    Task<IEnumerable<BorrowRecordDto>> IBorrowingService.GetMemberBorrowHistoryAsync(int memberId, CancellationToken ct)
    {
        throw new NotImplementedException();
    }

    Task<IEnumerable<BorrowRecordDto>> IBorrowingService.GetOverdueBorrowsAsync(CancellationToken ct)
    {
        throw new NotImplementedException();
    }

    Task<BorrowRecordDto> IBorrowingService.ReturnBookAsync(ReturnBookRequestDto dto, CancellationToken ct)
    {
        throw new NotImplementedException();
    }
}