using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Diagnostics;
using PasswordGenerator.Api.Endpoints;
using PasswordGenerator.Application.Abstractions;
using PasswordGenerator.Application.Services;
using PasswordGenerator.Infrastructure.Data;
using PasswordGenerator.Infrastructure.Repositories;

namespace PasswordGenerator.Api;

public static class ApiApplication
{
    public static WebApplication Build(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        var connectionString = builder.Configuration.GetConnectionString("PasswordDatabase")
            ?? throw new InvalidOperationException(
                "A string de conexão 'PasswordDatabase' não foi configurada.");

        builder.Services.AddProblemDetails();
        builder.Services.AddDbContext<PasswordDbContext>(options =>
            options.UseSqlite(connectionString));
        builder.Services.AddScoped<IPasswordRepository, PasswordRepository>();
        builder.Services.AddScoped<IPasswordService, PasswordService>();
        builder.Services.AddSingleton<IRandomByteGenerator, RandomByteGenerator>();
        builder.Services.AddSingleton<IPasswordGenerator, StrongPasswordGenerator>();

        var app = builder.Build();
        app.UseExceptionHandler(new ExceptionHandlerOptions
        {
            StatusCodeSelector = exception => exception is BadHttpRequestException badRequest
                ? badRequest.StatusCode
                : StatusCodes.Status500InternalServerError
        });
        app.MapPasswordEndpoints();

        return app;
    }
}
