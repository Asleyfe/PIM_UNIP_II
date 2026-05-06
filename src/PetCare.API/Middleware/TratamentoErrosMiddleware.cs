using System.Text.Json;
using PetCare.Domain.Exceptions;

namespace PetCare.API.Middleware;

/// <summary>
/// Middleware global para captura de exceções.
/// Converte exceções de domínio em respostas HTTP apropriadas.
/// </summary>
public class TratamentoErrosMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<TratamentoErrosMiddleware> _logger;

    public TratamentoErrosMiddleware(
        RequestDelegate next,
        ILogger<TratamentoErrosMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (EntidadeNaoEncontradaException ex)
        {
            await EscreverErro(context, 404, ex.Message);
        }
        catch (ConflitoAgendamentoException ex)
        {
            await EscreverErro(context, 409, ex.Message);
        }
        catch (EstoqueInsuficienteException ex)
        {
            await EscreverErro(context, 400, ex.Message);
        }
        catch (DominioException ex)
        {
            // Captura DominioException genérica e quaisquer subclasses não tratadas acima
            await EscreverErro(context, 400, ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro não tratado em {Path}", context.Request.Path);
            await EscreverErro(context, 500, "Erro interno do servidor.");
        }
    }

    private static async Task EscreverErro(HttpContext context, int statusCode, string mensagem)
    {
        context.Response.StatusCode = statusCode;
        context.Response.ContentType = "application/json";

        var resposta = JsonSerializer.Serialize(new
        {
            erro = mensagem,
            statusCode,
            timestamp = DateTime.UtcNow
        });

        await context.Response.WriteAsync(resposta);
    }
}