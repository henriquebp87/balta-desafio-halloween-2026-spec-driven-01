using Microsoft.EntityFrameworkCore;
using PasswordGenerator.Api;
using PasswordGenerator.Infrastructure.Data;

var app = ApiApplication.Build(args);

await using (var scope = app.Services.CreateAsyncScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<PasswordDbContext>();
    await dbContext.Database.MigrateAsync();
}

await app.RunAsync();
