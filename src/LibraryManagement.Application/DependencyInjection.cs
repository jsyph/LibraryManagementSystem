using LibraryManagement.Application.Common.Interfaces.Services;
using LibraryManagement.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace LibraryManagement.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IBookService, BookService>();
        services.AddScoped<IBookCopyService, BookCopyService>();
        services.AddScoped<IAuthorService, AuthorService>();
        services.AddScoped<ICategoryService, CategoryService>();
        services.AddScoped<IBorrowingService, BorrowingService>();
        services.AddScoped<IPaymentService, PaymentService>();
        services.AddScoped<IUserService, UserService>();

        return services;
    }
}
