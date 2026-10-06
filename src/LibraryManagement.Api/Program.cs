using LibraryManagement.Api.Endpoints;
using LibraryManagement.Application;
using LibraryManagement.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.MapUserEndpoints();
app.MapBookEndpoints();
app.MapBorrowEndpoints();
app.MapCategoryEndpoints();
app.MapAuthorEndpoints();
app.MapBookCopyEndpoints();
app.MapPaymentEndpoints();

app.Run();
