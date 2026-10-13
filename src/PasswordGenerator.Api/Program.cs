using Microsoft.EntityFrameworkCore;
using PasswordGenerator.Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("PasswordDatabase")
    ?? throw new InvalidOperationException("A string de conexão 'PasswordDatabase' não foi configurada.");

builder.Services.AddDbContext<PasswordDbContext>(options =>
    options.UseSqlite(connectionString));

var app = builder.Build();

app.Run();
