using Dapper;
using Microsoft.AspNetCore.Mvc;
using PetCare.Infrastructure.Data;

namespace PetCare.Web.Controllers;

public class CatalogosController : Controller
{
    private readonly IConnectionFactory _connectionFactory;

    public CatalogosController(IConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    [HttpGet("/api/Racas")]
    public async Task<IActionResult> ListarRacas()
    {
        const string sql = """
            SELECT
                r.id,
                r.nome,
                r.especie_id AS EspecieId,
                e.nome AS EspecieNome
            FROM raca r
            JOIN especie e ON e.id = r.especie_id
            ORDER BY e.nome, r.nome
            """;

        using var connection = _connectionFactory.CreateConnection();
        var racas = await connection.QueryAsync<RacaOptionDto>(sql);
        return Ok(racas);
    }

    private sealed record RacaOptionDto(long Id, string Nome, long EspecieId, string EspecieNome);
}
