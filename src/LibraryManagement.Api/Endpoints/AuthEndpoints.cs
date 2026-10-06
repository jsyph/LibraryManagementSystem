using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using LibraryManagement.Application.Common.Interfaces.Persistence;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace LibraryManagement.Api.Endpoints;

public static class AuthEndpoints
{
    public static RouteGroupBuilder MapAuthEndpoints(this WebApplication app)
    {
        var authItems = app.MapGroup("/auth");

        authItems
            .MapPost("/login", LoginAsync)
            .AllowAnonymous()
            .WithSummary("Authenticate a user and issue a JWT");

        return authItems;
    }

    private static async Task<IResult> LoginAsync(
        LoginRequest request,
        IUnitOfWork unitOfWork,
        IConfiguration configuration,
        CancellationToken cancellationToken
    )
    {
        var users = await unitOfWork.Users.SearchAsync(
            FirstName: null,
            LastName: null,
            Email: request.Email,
            PhoneNumber: null,
            IsActive: true,
            DateAdded: null,
            Role: null,
            cancellationToken
        );

        var user = users.FirstOrDefault(candidate =>
            string.Equals(candidate.Email, request.Email, System.StringComparison.OrdinalIgnoreCase)
        );

        if (user is null || !PasswordMatches(user.PasswordHash, request.PasswordHash))
        {
            return TypedResults.Unauthorized();
        }

        var issuer =
            configuration["Jwt:Issuer"]
            ?? throw new InvalidOperationException("JWT issuer is not configured.");
        var audience =
            configuration["Jwt:Audience"]
            ?? throw new InvalidOperationException("JWT audience is not configured.");
        var key =
            configuration["Jwt:Key"]
            ?? throw new InvalidOperationException("JWT key is not configured.");

        var credentials = new SigningCredentials(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
            SecurityAlgorithms.HmacSha256
        );
        var expiresAt = DateTime.UtcNow.AddHours(1);
        var token = new JwtSecurityToken(
            issuer,
            audience,
            [
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role.ToString()),
            ],
            expires: expiresAt,
            signingCredentials: credentials
        );

        return TypedResults.Ok(
            new LoginResponse(
                new JwtSecurityTokenHandler().WriteToken(token),
                expiresAt,
                user.Id,
                user.Role.ToString()
            )
        );
    }

    private static bool PasswordMatches(string storedHash, string submittedHash) =>
        CryptographicOperations.FixedTimeEquals(
            Encoding.UTF8.GetBytes(storedHash),
            Encoding.UTF8.GetBytes(submittedHash)
        );
}

public sealed record LoginRequest(string Email, string PasswordHash);

public sealed record LoginResponse(string AccessToken, DateTime ExpiresAt, int UserId, string Role);
