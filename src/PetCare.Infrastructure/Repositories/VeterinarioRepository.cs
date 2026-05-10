using Dapper;
using PetCare.Domain.Entities.Pessoas;
using PetCare.Domain.Interfaces.Repositories;
using PetCare.Infrastructure.Data;

namespace PetCare.Infrastructure.Repositories;

public class VeterinarioRepository : IVeterinarioRepository
{
    private readonly IConnectionFactory _connectionFactory;

    public VeterinarioRepository(IConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<Veterinario?> ObterPorId(long id)
    {
        const string sql = """
            SELECT id, nome, crmv, telefone, email,
                   created_at AS CreatedAt
            FROM veterinario
            WHERE id = @Id
            """;

        using var connection = _connectionFactory.CreateConnection();
        var row = await connection.QuerySingleOrDefaultAsync(sql, new { Id = id });
        return row is null ? null : MapearVeterinario(row);
    }

    public async Task<IEnumerable<Veterinario>> Listar()
    {
        const string sql = """
            SELECT id, nome, crmv, telefone, email,
                   created_at AS CreatedAt
            FROM veterinario
            ORDER BY nome
            """;

        using var connection = _connectionFactory.CreateConnection();
        var rows = await connection.QueryAsync(sql);
        return rows.Select(MapearVeterinario);
    }

    public async Task<Veterinario> Inserir(Veterinario veterinario)
    {
        const string sql = """
            INSERT INTO veterinario (nome, crmv, telefone, email)
            VALUES (@Nome, @Crmv, @Telefone, @Email)
            RETURNING id, created_at AS CreatedAt
            """;

        using var connection = _connectionFactory.CreateConnection();
        var resultado = await connection.QuerySingleAsync(sql, new
        {
            veterinario.Nome,
            veterinario.Crmv,
            veterinario.Telefone,
            veterinario.Email
        });

        veterinario.SetId((long)resultado.id);
        veterinario.SetCreatedAt((DateTime)resultado.createdat);

        return veterinario;
    }

    public async Task<bool> Atualizar(Veterinario veterinario)
    {
        const string sql = """
            UPDATE veterinario
            SET nome = @Nome, telefone = @Telefone, email = @Email
            WHERE id = @Id
            """;

        using var connection = _connectionFactory.CreateConnection();
        var linhasAfetadas = await connection.ExecuteAsync(sql, new
        {
            veterinario.Id,
            veterinario.Nome,
            veterinario.Telefone,
            veterinario.Email
        });

        return linhasAfetadas > 0;
    }

    public async Task<bool> Remover(long id)
    {
        const string sql = "DELETE FROM veterinario WHERE id = @Id";

        using var connection = _connectionFactory.CreateConnection();
        var linhasAfetadas = await connection.ExecuteAsync(sql, new { Id = id });
        return linhasAfetadas > 0;
    }

    public async Task<bool> ExistePorCrmv(string crmv)
    {
        const string sql = "SELECT COUNT(1) FROM veterinario WHERE crmv = @Crmv";

        using var connection = _connectionFactory.CreateConnection();
        var count = await connection.ExecuteScalarAsync<int>(sql, new { Crmv = crmv.ToUpper() });
        return count > 0;
    }

    private static Veterinario MapearVeterinario(dynamic row)
    {
        var veterinario = new Veterinario(
            (string)row.nome,
            (string)row.crmv,
            (string)row.telefone,
            (string)row.email
        );

        veterinario.SetId((long)row.id);
        veterinario.SetCreatedAt((DateTime)row.createdat);

        return veterinario;
    }
}
