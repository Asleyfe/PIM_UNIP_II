using Dapper;
using PetCare.Domain.Entities.Estoque;
using PetCare.Domain.Interfaces.Repositories;
using PetCare.Infrastructure.Data;

namespace PetCare.Infrastructure.Repositories;

public class ProdutoRepository : IProdutoRepository
{
    private readonly IConnectionFactory _connectionFactory;

    public ProdutoRepository(IConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<Produto?> ObterPorId(long id)
    {
        const string sql = """
            SELECT id, nome, preco, descricao, categoria, 
                   quantidade_estoque AS QuantidadeEstoque, 
                   estoque_minimo AS EstoqueMinimo, 
                   validade, created_at AS CreatedAt
            FROM produto
            WHERE id = @Id
            """;

        using var connection = _connectionFactory.CreateConnection();
        var row = await connection.QuerySingleOrDefaultAsync(sql, new { Id = id });
        return row is null ? null : MapearProduto(row);
    }

    public async Task<IEnumerable<Produto>> Listar()
    {
        const string sql = """
            SELECT id, nome, preco, descricao, categoria, 
                   quantidade_estoque AS QuantidadeEstoque, 
                   estoque_minimo AS EstoqueMinimo, 
                   validade, created_at AS CreatedAt
            FROM produto
            ORDER BY nome
            """;

        using var connection = _connectionFactory.CreateConnection();
        var rows = await connection.QueryAsync(sql);
        return rows.Select(MapearProduto);
    }

    public async Task<Produto> Inserir(Produto produto)
    {
        const string sql = """
            INSERT INTO produto (nome, preco, descricao, categoria, quantidade_estoque, estoque_minimo, validade)
            VALUES (@Nome, @Preco, @Descricao, @Categoria, @QuantidadeEstoque, @EstoqueMinimo, @Validade)
            RETURNING id, created_at AS CreatedAt
            """;

        using var connection = _connectionFactory.CreateConnection();
        var resultado = await connection.QuerySingleAsync(sql, new
        {
            produto.Nome,
            produto.Preco,
            produto.Descricao,
            produto.Categoria,
            produto.QuantidadeEstoque,
            produto.EstoqueMinimo,
            produto.Validade
        });

        produto.SetId((long)resultado.id);
        produto.SetCreatedAt((DateTime)resultado.createdat);

        return produto;
    }

    public async Task<bool> Atualizar(Produto produto)
    {
        const string sql = """
            UPDATE produto
            SET nome = @Nome, preco = @Preco, descricao = @Descricao, 
                categoria = @Categoria, quantidade_estoque = @QuantidadeEstoque, 
                estoque_minimo = @EstoqueMinimo, validade = @Validade
            WHERE id = @Id
            """;

        using var connection = _connectionFactory.CreateConnection();
        var linhasAfetadas = await connection.ExecuteAsync(sql, new
        {
            produto.Id,
            produto.Nome,
            produto.Preco,
            produto.Descricao,
            produto.Categoria,
            produto.QuantidadeEstoque,
            produto.EstoqueMinimo,
            produto.Validade
        });

        return linhasAfetadas > 0;
    }

    public async Task<bool> Remover(long id)
    {
        const string sql = "DELETE FROM produto WHERE id = @Id";

        using var connection = _connectionFactory.CreateConnection();
        var linhasAfetadas = await connection.ExecuteAsync(sql, new { Id = id });
        return linhasAfetadas > 0;
    }

    public async Task<IEnumerable<Produto>> ListarComEstoqueBaixo()
    {
        const string sql = """
            SELECT id, nome, preco, descricao, categoria, 
                   quantidade_estoque AS QuantidadeEstoque, 
                   estoque_minimo AS EstoqueMinimo, 
                   validade, created_at AS CreatedAt
            FROM produto
            WHERE quantidade_estoque <= estoque_minimo
            ORDER BY nome
            """;

        using var connection = _connectionFactory.CreateConnection();
        var rows = await connection.QueryAsync(sql);
        return rows.Select(MapearProduto);
    }

    public async Task<IEnumerable<Produto>> ListarProximosDoVencimento()
    {
        // Conforme sugerido na interface, poderíamos usar a view, 
        // mas vou implementar a lógica direta para garantir funcionamento.
        const string sql = """
            SELECT id, nome, preco, descricao, categoria, 
                   quantidade_estoque AS QuantidadeEstoque, 
                   estoque_minimo AS EstoqueMinimo, 
                   validade, created_at AS CreatedAt
            FROM produto
            WHERE validade IS NOT NULL 
              AND validade >= CURRENT_DATE 
              AND validade <= (CURRENT_DATE + INTERVAL '30 days')
            ORDER BY validade ASC
            """;

        using var connection = _connectionFactory.CreateConnection();
        var rows = await connection.QueryAsync(sql);
        return rows.Select(MapearProduto);
    }

    private static Produto MapearProduto(dynamic row)
    {
        var produto = new Produto(
            (string)row.nome,
            (decimal)row.preco,
            (string)row.categoria,
            (int)row.estoqueminimo,
            (string?)row.descricao,
            (DateTime?)row.validade
        );

        produto.SetId((long)row.id);
        produto.SetCreatedAt((DateTime)row.createdat);
        
        // QuantidadeEstoque é inicializada como 0 no construtor, 
        // precisamos atualizar com o valor do banco.
        produto.AtualizarEstoque((int)row.quantidadeestoque);

        return produto;
    }
}
