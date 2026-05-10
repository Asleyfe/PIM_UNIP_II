using Dapper;
using PetCare.Domain.Entities.Vendas;
using PetCare.Domain.Interfaces.Repositories;
using PetCare.Infrastructure.Data;

namespace PetCare.Infrastructure.Repositories;

public class VendaRepository : IVendaRepository
{
    private readonly IConnectionFactory _connectionFactory;

    public VendaRepository(IConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<Venda?> ObterPorId(long id)
    {
        const string sqlVenda = """
            SELECT id, tutor_id AS TutorId, data_venda AS DataVenda, 
                   valor_total AS ValorTotal, forma_pagamento AS FormaPagamento, 
                   observacao, created_at AS CreatedAt
            FROM venda
            WHERE id = @Id
            """;

        const string sqlItens = """
            SELECT id, venda_id AS VendaId, produto_id AS ProdutoId, 
                   quantidade, preco_unitario AS PrecoUnitario, 
                   created_at AS CreatedAt
            FROM item_venda
            WHERE venda_id = @Id
            """;

        using var connection = _connectionFactory.CreateConnection();
        var rowVenda = await connection.QuerySingleOrDefaultAsync(sqlVenda, new { Id = id });
        
        if (rowVenda is null) return null;

        var venda = MapearVenda(rowVenda);
        var rowsItens = await connection.QueryAsync(sqlItens, new { Id = id });
        
        foreach (var rowItem in rowsItens)
        {
            var item = MapearItemVenda(rowItem);
            // Usando reflexão para adicionar o item à lista privada _itens
            var fieldItens = typeof(Venda).GetField("_itens", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            var itensList = (List<ItemVenda>)fieldItens!.GetValue(venda)!;
            itensList.Add(item);
        }

        return venda;
    }

    public async Task<IEnumerable<Venda>> Listar()
    {
        const string sql = """
            SELECT id, tutor_id AS TutorId, data_venda AS DataVenda, 
                   valor_total AS ValorTotal, forma_pagamento AS FormaPagamento, 
                   observacao, created_at AS CreatedAt
            FROM venda
            ORDER BY data_venda DESC
            """;

        using var connection = _connectionFactory.CreateConnection();
        var rows = await connection.QueryAsync(sql);
        return rows.Select(MapearVenda);
    }

    public async Task<Venda> Inserir(Venda venda)
    {
        // De acordo com a interface, o método InserirComItens deve ser o preferencial
        // para garantir a integridade da venda. Este método base insere apenas a venda.
        
        const string sql = """
            INSERT INTO venda (tutor_id, data_venda, valor_total, forma_pagamento, observacao)
            VALUES (@TutorId, @DataVenda, @ValorTotal, @FormaPagamento, @Observacao)
            RETURNING id, created_at AS CreatedAt
            """;

        using var connection = _connectionFactory.CreateConnection();
        var resultado = await connection.QuerySingleAsync(sql, new
        {
            venda.TutorId,
            venda.DataVenda,
            venda.ValorTotal,
            venda.FormaPagamento,
            venda.Observacao
        });

        venda.SetId((long)resultado.id);
        venda.SetCreatedAt((DateTime)resultado.createdat);

        return venda;
    }

    public async Task<bool> Atualizar(Venda venda)
    {
        const string sql = """
            UPDATE venda
            SET tutor_id = @TutorId, data_venda = @DataVenda, 
                valor_total = @ValorTotal, forma_pagamento = @FormaPagamento, 
                observacao = @Observacao
            WHERE id = @Id
            """;

        using var connection = _connectionFactory.CreateConnection();
        var linhasAfetadas = await connection.ExecuteAsync(sql, new
        {
            venda.Id,
            venda.TutorId,
            venda.DataVenda,
            venda.ValorTotal,
            venda.FormaPagamento,
            venda.Observacao
        });

        return linhasAfetadas > 0;
    }

    public async Task<bool> Remover(long id)
    {
        // Ao remover uma venda, os itens são removidos por cascata no banco ou manualmente?
        // O schema mostra FOREIGN KEY (venda_id) REFERENCES public.venda(id) sem ON DELETE CASCADE explícito no .txt,
        // então vou remover os itens primeiro por segurança.
        
        const string sqlItens = "DELETE FROM item_venda WHERE venda_id = @VendaId";
        const string sqlVenda = "DELETE FROM venda WHERE id = @Id";

        using var connection = _connectionFactory.CreateConnection();
        connection.Open();
        using var transaction = connection.BeginTransaction();

        try
        {
            await connection.ExecuteAsync(sqlItens, new { VendaId = id }, transaction);
            var linhasAfetadas = await connection.ExecuteAsync(sqlVenda, new { Id = id }, transaction);
            transaction.Commit();
            return linhasAfetadas > 0;
        }
        catch
        {
            transaction.Rollback();
            throw;
        }
    }

    public async Task<Venda> InserirComItens(Venda venda, IEnumerable<ItemVenda> itens)
    {
        using var connection = _connectionFactory.CreateConnection();
        connection.Open();
        using var transaction = connection.BeginTransaction();

        try
        {
            // 1. Insere a Venda
            const string sqlVenda = """
                INSERT INTO venda (tutor_id, data_venda, valor_total, forma_pagamento, observacao)
                VALUES (@TutorId, @DataVenda, @ValorTotal, @FormaPagamento, @Observacao)
                RETURNING id, created_at AS CreatedAt
                """;

            var resultadoVenda = await connection.QuerySingleAsync(sqlVenda, new
            {
                venda.TutorId,
                venda.DataVenda,
                venda.ValorTotal,
                venda.FormaPagamento,
                venda.Observacao
            }, transaction);

            venda.SetId((long)resultadoVenda.id);
            venda.SetCreatedAt((DateTime)resultadoVenda.createdat);

            // 2. Insere os Itens
            const string sqlItem = """
                INSERT INTO item_venda (venda_id, produto_id, quantidade, preco_unitario)
                VALUES (@VendaId, @ProdutoId, @Quantidade, @PrecoUnitario)
                RETURNING id, created_at AS CreatedAt
                """;

            foreach (var item in itens)
            {
                // Garante que o item está vinculado à venda recém criada
                item.GetType().GetProperty("VendaId")!.SetValue(item, venda.Id);

                var resultadoItem = await connection.QuerySingleAsync(sqlItem, new
                {
                    VendaId = venda.Id,
                    item.ProdutoId,
                    item.Quantidade,
                    item.PrecoUnitario
                }, transaction);

                item.SetId((long)resultadoItem.id);
                item.SetCreatedAt((DateTime)resultadoItem.createdat);
            }

            transaction.Commit();
            return venda;
        }
        catch
        {
            transaction.Rollback();
            throw;
        }
    }

    public async Task<IEnumerable<Venda>> ListarPorPeriodo(DateTime inicio, DateTime fim)
    {
        const string sql = """
            SELECT id, tutor_id AS TutorId, data_venda AS DataVenda, 
                   valor_total AS ValorTotal, forma_pagamento AS FormaPagamento, 
                   observacao, created_at AS CreatedAt
            FROM venda
            WHERE data_venda BETWEEN @Inicio AND @Fim
            ORDER BY data_venda DESC
            """;

        using var connection = _connectionFactory.CreateConnection();
        var rows = await connection.QueryAsync(sql, new { Inicio = inicio, Fim = fim });
        return rows.Select(MapearVenda);
    }

    private static Venda MapearVenda(dynamic row)
    {
        var venda = (Venda)Activator.CreateInstance(typeof(Venda), true)!;
        
        venda.SetId((long)row.id);
        venda.SetCreatedAt((DateTime)row.createdat);
        
        venda.GetType().GetProperty("TutorId")!.SetValue(venda, (long)row.tutorid);
        venda.GetType().GetProperty("DataVenda")!.SetValue(venda, (DateTime)row.datavenda);
        venda.GetType().GetProperty("ValorTotal")!.SetValue(venda, (decimal)row.valortotal);
        venda.GetType().GetProperty("FormaPagamento")!.SetValue(venda, (string)row.formapagamento);
        venda.GetType().GetProperty("Observacao")!.SetValue(venda, (string?)row.observacao);

        return venda;
    }

    private static ItemVenda MapearItemVenda(dynamic row)
    {
        var item = (ItemVenda)Activator.CreateInstance(typeof(ItemVenda), true)!;
        
        item.SetId((long)row.id);
        item.SetCreatedAt((DateTime)row.createdat);
        
        item.GetType().GetProperty("VendaId")!.SetValue(item, (long)row.vendaid);
        item.GetType().GetProperty("ProdutoId")!.SetValue(item, (long)row.produtoid);
        item.GetType().GetProperty("Quantidade")!.SetValue(item, (int)row.quantidade);
        item.GetType().GetProperty("PrecoUnitario")!.SetValue(item, (decimal)row.precounitario);

        return item;
    }
}
