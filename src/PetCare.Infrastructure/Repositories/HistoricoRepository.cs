using Dapper;
using PetCare.Domain.Entities.Atendimento;
using PetCare.Domain.Interfaces.Repositories;
using PetCare.Infrastructure.Data;

namespace PetCare.Infrastructure.Repositories;

public class HistoricoRepository : IHistoricoRepository
{
    private readonly IConnectionFactory _connectionFactory;

    public HistoricoRepository(IConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<HistoricoClinico?> ObterPorId(long id)
    {
        const string sql = """
            SELECT id, animal_id as AnimalId, veterinario_id as VeterinarioId, 
                   descricao, data_registro as DataRegistro, created_at as CreatedAt
            FROM historico_clinico
            WHERE id = @Id
            """;

        using var connection = _connectionFactory.CreateConnection();
        return await connection.QuerySingleOrDefaultAsync<HistoricoClinico>(sql, new { Id = id });
    }

    public async Task<IEnumerable<HistoricoClinico>> Listar()
    {
        const string sql = """
            SELECT id, animal_id as AnimalId, veterinario_id as VeterinarioId, 
                   descricao, data_registro as DataRegistro, created_at as CreatedAt
            FROM historico_clinico
            ORDER BY data_registro DESC
            """;

        using var connection = _connectionFactory.CreateConnection();
        return await connection.QueryAsync<HistoricoClinico>(sql);
    }

    public async Task<HistoricoClinico> Inserir(HistoricoClinico historico)
    {
        const string sql = """
            INSERT INTO historico_clinico (animal_id, veterinario_id, descricao, data_registro)
            VALUES (@AnimalId, @VeterinarioId, @Descricao, @DataRegistro)
            RETURNING id, created_at as CreatedAt
            """;

        using var connection = _connectionFactory.CreateConnection();
        var resultado = await connection.QuerySingleAsync(sql, new
        {
            historico.AnimalId,
            historico.VeterinarioId,
            historico.Descricao,
            historico.DataRegistro
        });

        historico.SetId((long)resultado.id);
        historico.SetCreatedAt((DateTime)resultado.createdat);

        return historico;
    }

    public async Task<bool> Atualizar(HistoricoClinico historico)
    {
        const string sql = """
            UPDATE historico_clinico
            SET descricao = @Descricao
            WHERE id = @Id
            """;

        using var connection = _connectionFactory.CreateConnection();
        var rows = await connection.ExecuteAsync(sql, new
        {
            historico.Descricao,
            historico.Id
        });

        return rows > 0;
    }

    public async Task<bool> Remover(long id)
    {
        const string sql = "DELETE FROM historico_clinico WHERE id = @Id";

        using var connection = _connectionFactory.CreateConnection();
        var rows = await connection.ExecuteAsync(sql, new { Id = id });
        return rows > 0;
    }

    public async Task<IEnumerable<HistoricoClinico>> ListarPorAnimal(long animalId)
    {
        const string sql = """
            SELECT id, animal_id as AnimalId, veterinario_id as VeterinarioId, 
                   descricao, data_registro as DataRegistro, created_at as CreatedAt
            FROM historico_clinico
            WHERE animal_id = @AnimalId
            ORDER BY data_registro DESC
            """;

        using var connection = _connectionFactory.CreateConnection();
        return await connection.QueryAsync<HistoricoClinico>(sql, new { AnimalId = animalId });
    }
}
