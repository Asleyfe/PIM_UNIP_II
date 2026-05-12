using Dapper;
using PetCare.Domain.DTOs.Relatorios;
using PetCare.Domain.Interfaces.Repositories;
using PetCare.Infrastructure.Data;

namespace PetCare.Infrastructure.Repositories;

public class RelatorioRepository : IRelatorioRepository
{
    private readonly IConnectionFactory _connectionFactory;

    public RelatorioRepository(IConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<decimal> ObterFaturamentoTotal(DateTime inicio, DateTime fim)
    {
        const string sql = "SELECT COALESCE(SUM(valor_total), 0) FROM venda WHERE data_venda BETWEEN @Inicio AND @Fim";
        using var connection = _connectionFactory.CreateConnection();
        return await connection.ExecuteScalarAsync<decimal>(sql, new { Inicio = inicio, Fim = fim });
    }

    public async Task<IEnumerable<FaturamentoPorCategoriaDto>> ObterFaturamentoPorCategoria(DateTime inicio, DateTime fim)
    {
        const string sql = """
            SELECT p.categoria, SUM(iv.quantidade * iv.preco_unitario) as Valor
            FROM item_venda iv
            JOIN venda v ON v.id = iv.venda_id
            JOIN produto p ON p.id = iv.produto_id
            WHERE v.data_venda BETWEEN @Inicio AND @Fim
            GROUP BY p.categoria
            ORDER BY Valor DESC
            """;
        using var connection = _connectionFactory.CreateConnection();
        return await connection.QueryAsync<FaturamentoPorCategoriaDto>(sql, new { Inicio = inicio, Fim = fim });
    }

    public async Task<IEnumerable<FaturamentoMensalDto>> ObterFaturamentoMensal(DateTime inicio, DateTime fim)
    {
        const string sql = """
            SELECT TO_CHAR(data_venda, 'MM/YYYY') as MesAno, SUM(valor_total) as Valor
            FROM venda
            WHERE data_venda BETWEEN @Inicio AND @Fim
            GROUP BY TO_CHAR(data_venda, 'MM/YYYY'), DATE_TRUNC('month', data_venda)
            ORDER BY DATE_TRUNC('month', data_venda)
            """;
        using var connection = _connectionFactory.CreateConnection();
        return await connection.QueryAsync<FaturamentoMensalDto>(sql, new { Inicio = inicio, Fim = fim });
    }

    public async Task<IEnumerable<RelatorioDesempenhoProfissionalDto>> ObterDesempenhoProfissionais(DateTime inicio, DateTime fim)
    {
        const string sql = """
            SELECT v.id as VeterinarioId, v.nome as NomeVeterinario, COUNT(a.id) as TotalAtendimentos, 0 as ValorGerado
            FROM agendamento a
            JOIN veterinario v ON v.id = a.veterinario_id
            WHERE a.datahora_consulta BETWEEN @Inicio AND @Fim AND a.status = 'CONCLUIDO'
            GROUP BY v.id, v.nome
            ORDER BY TotalAtendimentos DESC
            """;
        using var connection = _connectionFactory.CreateConnection();
        return await connection.QueryAsync<RelatorioDesempenhoProfissionalDto>(sql, new { Inicio = inicio, Fim = fim });
    }

    public async Task<IEnumerable<RelatorioProdutosMaisVendidosDto>> ObterProdutosMaisVendidos(DateTime inicio, DateTime fim, int top)
    {
        const string sql = """
            SELECT p.id as ProdutoId, p.nome as NomeProduto, SUM(iv.quantidade) as QuantidadeVendida, SUM(iv.quantidade * iv.preco_unitario) as TotalArrecadado
            FROM item_venda iv
            JOIN venda v ON v.id = iv.venda_id
            JOIN produto p ON p.id = iv.produto_id
            WHERE v.data_venda BETWEEN @Inicio AND @Fim
            GROUP BY p.id, p.nome
            ORDER BY QuantidadeVendida DESC
            LIMIT @Top
            """;
        using var connection = _connectionFactory.CreateConnection();
        return await connection.QueryAsync<RelatorioProdutosMaisVendidosDto>(sql, new { Inicio = inicio, Fim = fim, Top = top });
    }
}
