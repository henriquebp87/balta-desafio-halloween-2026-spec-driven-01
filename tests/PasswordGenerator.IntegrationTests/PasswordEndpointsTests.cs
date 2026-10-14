using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PasswordGenerator.Api;
using PasswordGenerator.Api.Contracts;
using PasswordGenerator.Infrastructure.Data;

namespace PasswordGenerator.IntegrationTests;

public sealed class PasswordEndpointsTests
{
    private const string SpecialCharacters = "!@#$%^&*()-_=+[]{};:,.?";

    [Fact]
    public async Task PostPassword_WithValidLength_ReturnsAndStoresStrongPassword()
    {
        await using var api = await TestApi.StartAsync();

        var response = await api.Client.PostAsJsonAsync(
            "/api/v1/password",
            new CreatePasswordRequest(16));
        var created = await response.Content.ReadFromJsonAsync<PasswordResponse>();

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(created);
        Assert.NotEqual(Guid.Empty, created.Guid);
        Assert.Equal(16, created.Password.Length);
        Assert.Contains(created.Password, char.IsLetter);
        Assert.Contains(created.Password, char.IsDigit);
        Assert.Contains(created.Password, SpecialCharacters.Contains);
        Assert.DoesNotContain(created.Password, char.IsWhiteSpace);

        await using var scope = api.Application.Services.CreateAsyncScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<PasswordDbContext>();
        var persisted = await dbContext.Passwords.SingleAsync(
            password => password.Guid == created.Guid);

        Assert.Equal(created.Password, persisted.Value);
    }

    [Fact]
    public async Task GetPassword_WhenGuidExists_ReturnsGuidAndPassword()
    {
        await using var api = await TestApi.StartAsync();
        var createResponse = await api.Client.PostAsJsonAsync(
            "/api/v1/password",
            new CreatePasswordRequest(16));
        var created = await createResponse.Content.ReadFromJsonAsync<PasswordResponse>();
        Assert.NotNull(created);

        var response = await api.Client.GetAsync($"/api/v1/password/{created.Guid}");
        var found = await response.Content.ReadFromJsonAsync<PasswordResponse>();

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal(created, found);
    }

    [Theory]
    [InlineData(15)]
    [InlineData(65)]
    public async Task PostPassword_WithInvalidLength_ReturnsProblemDetails(int length)
    {
        await using var api = await TestApi.StartAsync();

        var response = await api.Client.PostAsJsonAsync(
            "/api/v1/password",
            new CreatePasswordRequest(length));
        var problem = await response.Content.ReadFromJsonAsync<ProblemDetails>();

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.Equal("application/problem+json", response.Content.Headers.ContentType?.MediaType);
        Assert.NotNull(problem);
        Assert.Contains("entre 16 e 64 caracteres", problem.Detail);
    }

    [Fact]
    public async Task PostPassword_WithoutBody_ReturnsProblemDetails()
    {
        await using var api = await TestApi.StartAsync();

        var response = await api.Client.PostAsync("/api/v1/password", content: null);
        var problem = await response.Content.ReadFromJsonAsync<ProblemDetails>();

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.Equal("application/problem+json", response.Content.Headers.ContentType?.MediaType);
        Assert.NotNull(problem);
    }

    [Fact]
    public async Task GetPassword_WhenGuidDoesNotExist_ReturnsProblemDetails()
    {
        await using var api = await TestApi.StartAsync();

        var response = await api.Client.GetAsync($"/api/v1/password/{Guid.NewGuid()}");
        var problem = await response.Content.ReadFromJsonAsync<ProblemDetails>();

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        Assert.Equal("application/problem+json", response.Content.Headers.ContentType?.MediaType);
        Assert.Equal("Nenhuma senha foi encontrada para o GUID informado.", problem?.Detail);
    }

    private sealed class TestApi : IAsyncDisposable
    {
        private readonly string _databasePath;

        private TestApi(WebApplication application, HttpClient client, string databasePath)
        {
            Application = application;
            Client = client;
            _databasePath = databasePath;
        }

        public WebApplication Application { get; }

        public HttpClient Client { get; }

        public static async Task<TestApi> StartAsync()
        {
            var databasePath = Path.Combine(
                Path.GetTempPath(),
                $"password-generator-{Guid.NewGuid():N}.db");
            var application = ApiApplication.Build(
            [
                "--urls", "http://127.0.0.1:0",
                "--ConnectionStrings:PasswordDatabase", $"Data Source={databasePath}"
            ]);

            await using (var scope = application.Services.CreateAsyncScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<PasswordDbContext>();
                await dbContext.Database.MigrateAsync();
            }

            await application.StartAsync();

            var server = application.Services.GetRequiredService<IServer>();
            var address = server.Features.Get<IServerAddressesFeature>()!.Addresses.Single();
            var client = new HttpClient { BaseAddress = new Uri(address) };

            return new TestApi(application, client, databasePath);
        }

        public async ValueTask DisposeAsync()
        {
            Client.Dispose();
            await Application.StopAsync();
            await Application.DisposeAsync();
            SqliteConnection.ClearAllPools();

            DeleteDatabaseFile(_databasePath);
            DeleteDatabaseFile($"{_databasePath}-shm");
            DeleteDatabaseFile($"{_databasePath}-wal");
        }

        private static void DeleteDatabaseFile(string path)
        {
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }
    }
}
