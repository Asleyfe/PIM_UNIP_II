using Dapper;
using Microsoft.AspNetCore.Mvc;
using PetCare.Infrastructure.Data;

namespace PetCare.API.Controllers;

/// <summary>
/// Endpoint de verificação de saúde da aplicação.
/// Útil para monitoramento e validação inicial da configuração.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class HealthController : ControllerBase
{
    private readonly IConnectionFactory _connectionFactory;
    private readonly ILogger<HealthController> _logger;

    public HealthController(
        IConnectionFactory connectionFactory,
        ILogger<HealthController> logger)
    {
        _connectionFactory = connectionFactory;
        _logger = logger;
    }

    /// <summary>
    /// Verifica se a API está respondendo e se a conexão com o banco está funcional.
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        try
        {
            using var conn = _connectionFactory.CreateConnection();
            var resultado = await conn.QuerySingleAsync<int>("SELECT 1");

            return Ok(new
            {
                status = "ok",
                database = resultado == 1 ? "connected" : "unexpected_response",
                timestamp = DateTime.UtcNow
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Falha ao verificar saúde do banco");

            return StatusCode(503, new
            {
                status = "degraded",
                database = "unreachable",
                erro = ex.Message,
                timestamp = DateTime.UtcNow
            });
        }
    }
}