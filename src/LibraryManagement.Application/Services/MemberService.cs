namespace LibraryManagement.Application.Services;

using LibraryManagement.Application.Common.Exceptions;
using LibraryManagement.Application.Common.Interfaces.Persistence;
using LibraryManagement.Application.Common.Interfaces.Services;
using LibraryManagement.Application.DTOs;
using LibraryManagement.Domain.Entities;
using LibraryManagement.Domain.Enums;
using AutoMapper;

class MemberService(IUnitOfWork unitOfWork, IMapper mapper) : IMemberService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMapper _mapper = mapper;

    Task IMemberService.ActivateAsync(int id, CancellationToken ct)
    {
        throw new NotImplementedException();
    }

    Task IMemberService.BanAsync(int id, BanMemberDto dto, CancellationToken ct)
    {
        throw new NotImplementedException();
    }

    Task<MemberDto> IMemberService.CreateAsync(CreateMemberDto dto, CancellationToken ct)
    {
        throw new NotImplementedException();
    }

    Task IMemberService.DeactivateAsync(int id, CancellationToken ct)
    {
        throw new NotImplementedException();
    }

    Task IMemberService.DeleteAsync(int id, CancellationToken ct)
    {
        throw new NotImplementedException();
    }

    Task<IEnumerable<BorrowRecordDto>> IMemberService.GetActiveBorrowsAsync(int memberId, CancellationToken ct)
    {
        throw new NotImplementedException();
    }

    Task<IEnumerable<MemberDto>> IMemberService.GetAllAsync(CancellationToken ct)
    {
        throw new NotImplementedException();
    }

    Task<IEnumerable<BorrowRecordDto>> IMemberService.GetBorrowHistoryAsync(int memberId, CancellationToken ct)
    {
        throw new NotImplementedException();
    }

    Task<MemberDto?> IMemberService.GetByEmailAsync(string email, CancellationToken ct)
    {
        throw new NotImplementedException();
    }

    Task<MemberDto> IMemberService.GetByIdAsync(int id, CancellationToken ct)
    {
        throw new NotImplementedException();
    }

    Task<MemberDto?> IMemberService.GetByNationalIdAsync(string nationalId, CancellationToken ct)
    {
        throw new NotImplementedException();
    }

    Task<MemberDto?> IMemberService.GetByPhoneAsync(string phone, CancellationToken ct)
    {
        throw new NotImplementedException();
    }

    Task IMemberService.RenewMembershipAsync(int id, RenewMembershipDto dto, CancellationToken ct)
    {
        throw new NotImplementedException();
    }

    Task IMemberService.UnbanAsync(int id, CancellationToken ct)
    {
        throw new NotImplementedException();
    }

    Task IMemberService.UpdateAsync(int id, UpdateMemberDto dto, CancellationToken ct)
    {
        throw new NotImplementedException();
    }
}