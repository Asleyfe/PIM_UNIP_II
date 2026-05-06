using Dapper;

namespace PetCare.Infrastructure.Data;

/// <summary>
/// Configuração global do Dapper. Chamado uma única vez no Program.cs.
/// </summary>
public static class DapperSetup
{
    /// <summary>
    /// Habilita o mapeamento automático de snake_case (banco) para PascalCase (C#).
    /// Exemplo: coluna data_consulta vira propriedade DataConsulta.
    /// </summary>
    public static void Configure()
    {
        DefaultTypeMap.MatchNamesWithUnderscores = true;
    }
}