using PasswordGenerator.Api.Contracts;
using PasswordGenerator.Application.Exceptions;
using PasswordGenerator.Application.Services;

namespace PasswordGenerator.Api.Endpoints;

public static class PasswordEndpoints
{
    public static IEndpointRouteBuilder MapPasswordEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapGroup("/api/v1/password");

        group.MapPost("", CreateAsync);
        group.MapGet("/{guid:guid}", GetByGuidAsync);

        return endpoints;
    }

    private static async Task<IResult> CreateAsync(
        CreatePasswordRequest? request,
        IPasswordService passwordService,
        CancellationToken cancellationToken)
    {
        if (request is null)
        {
            return Results.Problem(
                statusCode: StatusCodes.Status400BadRequest,
                title: "Requisição inválida",
                detail: "Informe o tamanho da senha.");
        }

        try
        {
            var result = await passwordService.CreateAsync(request.Length, cancellationToken);
            return Results.Ok(PasswordResponse.FromResult(result));
        }
        catch (PasswordLengthOutOfRangeException exception)
        {
            return Results.Problem(
                statusCode: StatusCodes.Status400BadRequest,
                title: "Tamanho de senha inválido",
                detail: exception.Message);
        }
        catch
        {
            return Results.Problem(
                statusCode: StatusCodes.Status500InternalServerError,
                title: "Não foi possível gerar a senha",
                detail: "Ocorreu um erro ao gerar ou armazenar a senha.");
        }
    }

    private static async Task<IResult> GetByGuidAsync(
        Guid guid,
        IPasswordService passwordService,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await passwordService.GetByGuidAsync(guid, cancellationToken);

            return result is null
                ? Results.Problem(
                    statusCode: StatusCodes.Status404NotFound,
                    title: "Senha não encontrada",
                    detail: "Nenhuma senha foi encontrada para o GUID informado.")
                : Results.Ok(PasswordResponse.FromResult(result));
        }
        catch
        {
            return Results.Problem(
                statusCode: StatusCodes.Status500InternalServerError,
                title: "Não foi possível recuperar a senha",
                detail: "Ocorreu um erro ao consultar a senha.");
        }
    }
}
