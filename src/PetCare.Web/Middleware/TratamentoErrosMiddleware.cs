using System.Text.Json;
using PetCare.Domain.Exceptions;

namespace PetCare.Web.Middleware;

/// <summary>
/// Middleware global para captura de exceções.
/// Converte exceções de domínio em respostas HTTP apropriadas e gera logs estruturados.
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
            _logger.LogWarning("Entidade não encontrada: {Message}", ex.Message);
            await EscreverErro(context, 404, ex.Message);
        }
        catch (ConflitoAgendamentoException ex)
        {
            _logger.LogWarning("Conflito de agendamento: {Message}", ex.Message);
            await EscreverErro(context, 409, ex.Message);
        }
        catch (EstoqueInsuficienteException ex)
        {
            _logger.LogWarning("Estoque insuficiente: {Message}", ex.Message);
            await EscreverErro(context, 400, ex.Message);
        }
        catch (DominioException ex)
        {
            _logger.LogWarning("Regra de negócio violada: {Message}", ex.Message);
            await EscreverErro(context, 400, ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro crítico não tratado na rota {Path}. Detalhes: {Message}", 
                context.Request.Path, ex.Message);
            
            await EscreverErro(context, 500, "Ocorreu um erro inesperado no servidor. Por favor, tente novamente mais tarde.");
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
