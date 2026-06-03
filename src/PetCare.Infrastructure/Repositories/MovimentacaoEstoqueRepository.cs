using Dapper;
using PetCare.Domain.Entities.Estoque;
using PetCare.Domain.Enums;
using PetCare.Domain.Interfaces.Repositories;
using PetCare.Infrastructure.Data;

namespace PetCare.Infrastructure.Repositories;

public class MovimentacaoEstoqueRepository : IMovimentacaoEstoqueRepository
{
    private readonly IConnectionFactory _connectionFactory;

    public MovimentacaoEstoqueRepository(IConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<MovimentacaoEstoque?> ObterPorId(long id)
    {
        const string sql = """
            SELECT id, produto_id AS ProdutoId, quantidade, tipo, 
                   data_movimentacao AS DataMovimentacao, data_movimentacao AS CreatedAt
            FROM movimentacao_estoque
            WHERE id = @Id
            """;

        using var connection = _connectionFactory.CreateConnection();
        var row = await connection.QuerySingleOrDefaultAsync(sql, new { Id = id });
        return row is null ? null : MapearMovimentacao(row);
    }

    public async Task<IEnumerable<MovimentacaoEstoque>> Listar()
    {
        const string sql = """
            SELECT id, produto_id AS ProdutoId, quantidade, tipo, 
                   data_movimentacao AS DataMovimentacao, data_movimentacao AS CreatedAt
            FROM movimentacao_estoque
            ORDER BY data_movimentacao DESC
            """;

        using var connection = _connectionFactory.CreateConnection();
        var rows = await connection.QueryAsync(sql);
        return rows.Select(MapearMovimentacao);
    }

    public async Task<MovimentacaoEstoque> Inserir(MovimentacaoEstoque movimentacao)
    {
        const string sql = """
            INSERT INTO movimentacao_estoque (produto_id, quantidade, tipo, data_movimentacao)
            VALUES (@ProdutoId, @Quantidade, @Tipo, @DataMovimentacao)
            RETURNING id
            """;

        using var connection = _connectionFactory.CreateConnection();
        var resultado = await connection.QuerySingleAsync(sql, new
        {
            movimentacao.ProdutoId,
            movimentacao.Quantidade,
            Tipo = movimentacao.Tipo.ToString(),
            movimentacao.DataMovimentacao
        });

        movimentacao.SetId((long)resultado.id);
        movimentacao.SetCreatedAt(movimentacao.DataMovimentacao);

        return movimentacao;
    }

    public async Task<bool> Atualizar(MovimentacaoEstoque movimentacao)
    {
        // Geralmente movimentações de estoque não são atualizadas, mas para manter a interface IRepositorioBase:
        const string sql = """
            UPDATE movimentacao_estoque
            SET produto_id = @ProdutoId, quantidade = @Quantidade, 
                tipo = @Tipo, data_movimentacao = @DataMovimentacao
            WHERE id = @Id
            """;

        using var connection = _connectionFactory.CreateConnection();
        var linhasAfetadas = await connection.ExecuteAsync(sql, new
        {
            movimentacao.Id,
            movimentacao.ProdutoId,
            movimentacao.Quantidade,
            Tipo = movimentacao.Tipo.ToString(),
            movimentacao.DataMovimentacao
        });

        return linhasAfetadas > 0;
    }

    public async Task<bool> Remover(long id)
    {
        const string sql = "DELETE FROM movimentacao_estoque WHERE id = @Id";

        using var connection = _connectionFactory.CreateConnection();
        var linhasAfetadas = await connection.ExecuteAsync(sql, new { Id = id });
        return linhasAfetadas > 0;
    }

    public async Task<IEnumerable<MovimentacaoEstoque>> ListarPorProduto(long produtoId)
    {
        const string sql = """
            SELECT id, produto_id AS ProdutoId, quantidade, tipo, 
                   data_movimentacao AS DataMovimentacao, data_movimentacao AS CreatedAt
            FROM movimentacao_estoque
            WHERE produto_id = @ProdutoId
            ORDER BY data_movimentacao DESC
            """;

        using var connection = _connectionFactory.CreateConnection();
        var rows = await connection.QueryAsync(sql, new { ProdutoId = produtoId });
        return rows.Select(MapearMovimentacao);
    }

    private static MovimentacaoEstoque MapearMovimentacao(dynamic row)
    {
        if (!Enum.TryParse<TipoMovimentacao>((string)row.tipo, true, out var tipo))
            throw new ArgumentException($"Tipo de movimentação inválido: {row.tipo}");

        // Como MovimentacaoEstoque tem construtor que seta DataMovimentacao como UtcNow,
        // mas o banco retorna a data original, usamos reflexão para o mapeamento completo.
        
        var movimentacao = (MovimentacaoEstoque)Activator.CreateInstance(typeof(MovimentacaoEstoque), true)!;
        
        movimentacao.SetId((long)row.id);
        movimentacao.SetCreatedAt((DateTime)row.createdat);
        
        movimentacao.GetType().GetProperty("ProdutoId")!.SetValue(movimentacao, (long)row.produtoid);
        movimentacao.GetType().GetProperty("Quantidade")!.SetValue(movimentacao, (int)row.quantidade);
        movimentacao.GetType().GetProperty("Tipo")!.SetValue(movimentacao, tipo);
        movimentacao.GetType().GetProperty("DataMovimentacao")!.SetValue(movimentacao, (DateTime)row.datamovimentacao);

        return movimentacao;
    }
}
