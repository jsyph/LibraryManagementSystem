using System.Threading;
using System.Threading.Tasks;
using LibraryManagement.Application.Common.Interfaces.Services;
using LibraryManagement.Application.DTOs;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace LibraryManagement.Api.Endpoints;

public static class UserEndpoints
{
    public static RouteGroupBuilder MapUserEndpoints(this WebApplication app)
    {
        var userItems = app.MapGroup("/user");

        userItems.MapGet("/search", SearchUsersAsync)
            .WithSummary("Search users");
        userItems.MapGet("/members/{id:int}", GetMemberByIdAsync)
            .WithSummary("Get a member by ID");
        userItems.MapGet("/members", GetAllMembersAsync)
            .WithSummary("Get all members");
        userItems.MapPost("/members", AddMemberAsync)
            .WithSummary("Create a member");
        userItems.MapPut("/members/{id:int}", UpdateMemberAsync)
            .WithSummary("Update a member");
        userItems.MapDelete("/members/{id:int}", DeleteMemberAsync)
            .WithSummary("Delete a member");
        userItems.MapGet("/librarians/{id:int}", GetLibrarianByIdAsync)
            .WithSummary("Get a librarian by ID");
        userItems.MapGet("/librarians", GetAllLibrariansAsync)
            .WithSummary("Get all librarians");
        userItems.MapPost("/librarians", AddLibrarianAsync)
            .WithSummary("Create a librarian");
        userItems.MapPut("/librarians/{id:int}", UpdateLibrarianAsync)
            .WithSummary("Update a librarian");
        userItems.MapDelete("/librarians/{id:int}", DeleteLibrarianAsync)
            .WithSummary("Delete a librarian");
        userItems.MapGet("/admins/{id:int}", GetAdminByIdAsync)
            .WithSummary("Get an admin by ID");
        userItems.MapGet("/admins", GetAllAdminsAsync)
            .WithSummary("Get all admins");
        userItems.MapPost("/admins", AddAdminAsync)
            .WithSummary("Create an admin");
        userItems.MapPut("/admins/{id:int}", UpdateAdminAsync)
            .WithSummary("Update an admin");
        userItems.MapPatch("/{id:int}/password", ChangeUserPasswordHashAsync)
            .WithSummary("Change a user's password");
        userItems.MapPatch("/{id:int}/activate", ActivateUserAsync)
            .WithSummary("Activate a user");
        userItems.MapPatch("/{id:int}/deactivate", DeactivateUserAsync)
            .WithSummary("Deactivate a user");
        userItems.MapPatch("/members/renew", RenewMembershipAsync)
            .WithSummary("Renew a member's membership");

        return userItems;
    }

    private static async Task<IResult> SearchUsersAsync(
        [AsParameters] SearchUserDto dto,
        IUserService userService,
        CancellationToken cancellationToken
    ) => TypedResults.Ok(await userService.SearchUsersAsync(dto, cancellationToken));

    private static async Task<IResult> GetMemberByIdAsync(
        int id,
        IUserService userService,
        CancellationToken cancellationToken
    )
    {
        var member = await userService.GetMemberByIdAsync(id, cancellationToken);
        return member is null ? TypedResults.NotFound() : TypedResults.Ok(member);
    }

    private static async Task<IResult> GetAllMembersAsync(
        IUserService userService,
        CancellationToken cancellationToken
    ) => TypedResults.Ok(await userService.GetAllMembersAsync(cancellationToken));

    private static async Task<IResult> AddMemberAsync(
        CreateMemberDto dto,
        IUserService userService,
        CancellationToken cancellationToken
    )
    {
        var member = await userService.AddMemberAsync(dto, cancellationToken);
        return TypedResults.Created($"/user/members/{member.Id}", member);
    }

    private static async Task<IResult> UpdateMemberAsync(
        int id,
        UpdateMemberDto dto,
        IUserService userService,
        CancellationToken cancellationToken
    )
    {
        await userService.UpdateMemberAsync(id, dto, cancellationToken);
        return TypedResults.NoContent();
    }

    private static async Task<IResult> DeleteMemberAsync(
        int id,
        IUserService userService,
        CancellationToken cancellationToken
    )
    {
        await userService.DeleteMemberAsync(id, cancellationToken);
        return TypedResults.NoContent();
    }

    private static async Task<IResult> GetLibrarianByIdAsync(
        int id,
        IUserService userService,
        CancellationToken cancellationToken
    )
    {
        var librarian = await userService.GetLibrarianByIdAsync(id, cancellationToken);
        return librarian is null ? TypedResults.NotFound() : TypedResults.Ok(librarian);
    }

    private static async Task<IResult> GetAllLibrariansAsync(
        IUserService userService,
        CancellationToken cancellationToken
    ) => TypedResults.Ok(await userService.GetAllLibrariansAsync(cancellationToken));

    private static async Task<IResult> AddLibrarianAsync(
        CreateLibrarianDto dto,
        IUserService userService,
        CancellationToken cancellationToken
    )
    {
        var librarian = await userService.AddLibrarianAsync(dto, cancellationToken);
        return TypedResults.Created($"/user/librarians/{librarian.Id}", librarian);
    }

    private static async Task<IResult> UpdateLibrarianAsync(
        int id,
        UpdateLibrarianDto dto,
        IUserService userService,
        CancellationToken cancellationToken
    )
    {
        await userService.UpdateLibrarianAsync(id, dto, cancellationToken);
        return TypedResults.NoContent();
    }

    private static async Task<IResult> DeleteLibrarianAsync(
        int id,
        IUserService userService,
        CancellationToken cancellationToken
    )
    {
        await userService.DeleteLibrarianAsync(id, cancellationToken);
        return TypedResults.NoContent();
    }

    private static async Task<IResult> GetAdminByIdAsync(
        int id,
        IUserService userService,
        CancellationToken cancellationToken
    )
    {
        var admin = await userService.GetAdminByIdAsync(id, cancellationToken);
        return admin is null ? TypedResults.NotFound() : TypedResults.Ok(admin);
    }

    private static async Task<IResult> GetAllAdminsAsync(
        IUserService userService,
        CancellationToken cancellationToken
    ) => TypedResults.Ok(await userService.GetAllAdminsAsync(cancellationToken));

    private static async Task<IResult> AddAdminAsync(
        CreateAdminDto dto,
        IUserService userService,
        CancellationToken cancellationToken
    )
    {
        var admin = await userService.AddAdminAsync(dto, cancellationToken);
        return TypedResults.Created($"/user/admins/{admin.Id}", admin);
    }

    private static async Task<IResult> UpdateAdminAsync(
        int id,
        UpdateAdminDto dto,
        IUserService userService,
        CancellationToken cancellationToken
    )
    {
        await userService.UpdateAdminAsync(id, dto, cancellationToken);
        return TypedResults.NoContent();
    }

    private static async Task<IResult> ChangeUserPasswordHashAsync(
        int id,
        ChangeUserPasswordHashDto dto,
        IUserService userService,
        CancellationToken cancellationToken
    )
    {
        await userService.ChangeUserPasswordHashAsync(id, dto, cancellationToken);
        return TypedResults.NoContent();
    }

    private static async Task<IResult> ActivateUserAsync(
        int id,
        IUserService userService,
        CancellationToken cancellationToken
    )
    {
        await userService.ActivateUserAsync(id, cancellationToken);
        return TypedResults.NoContent();
    }

    private static async Task<IResult> DeactivateUserAsync(
        int id,
        IUserService userService,
        CancellationToken cancellationToken
    )
    {
        await userService.DeactivateUserAsync(id, cancellationToken);
        return TypedResults.NoContent();
    }

    private static async Task<IResult> RenewMembershipAsync(
        RenewMembershipDto dto,
        IUserService userService,
        CancellationToken cancellationToken
    )
    {
        await userService.RenewMembershipAsync(dto, cancellationToken);
        return TypedResults.NoContent();
    }
}
