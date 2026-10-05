using LibraryManagement.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace LibraryManagement.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<BookService>();
        services.AddScoped<BookCopyService>();
        services.AddScoped<AuthorService>();
        services.AddScoped<CategoryService>();
        services.AddScoped<BorrowingService>();
        services.AddScoped<PaymentService>();
        services.AddScoped<UserService>();

        return services;
    }
}
