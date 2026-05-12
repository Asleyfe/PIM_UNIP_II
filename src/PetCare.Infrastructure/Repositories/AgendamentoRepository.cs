using Dapper;
using PetCare.Domain.Entities.Atendimento;
using PetCare.Domain.Enums;
using PetCare.Domain.Interfaces.Repositories;
using PetCare.Infrastructure.Data;

namespace PetCare.Infrastructure.Repositories;

public class AgendamentoRepository : IAgendamentoRepository
{
    private readonly IConnectionFactory _connectionFactory;

    public AgendamentoRepository(IConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<Agendamento?> ObterPorId(long id)
    {
        const string sql = """
            SELECT id, tutor_id AS TutorId, animal_id AS AnimalId, 
                   veterinario_id AS VeterinarioId, preco, datahora_consulta AS DataHoraConsulta,
                   status, observacao, created_at AS CreatedAt
            FROM agendamento
            WHERE id = @Id
            """;

        using var connection = _connectionFactory.CreateConnection();
        var row = await connection.QuerySingleOrDefaultAsync(sql, new { Id = id });
        return row is null ? null : MapearAgendamento(row);
    }

    public async Task<IEnumerable<Agendamento>> Listar()
    {
        const string sql = """
            SELECT id, tutor_id AS TutorId, animal_id AS AnimalId, 
                   veterinario_id AS VeterinarioId, preco, datahora_consulta AS DataHoraConsulta,
                   status, observacao, created_at AS CreatedAt
            FROM agendamento
            ORDER BY datahora_consulta DESC
            """;

        using var connection = _connectionFactory.CreateConnection();
        var rows = await connection.QueryAsync(sql);
        return rows.Select(MapearAgendamento);
    }

    public async Task<Agendamento> Inserir(Agendamento agendamento)
    {
        const string sql = """
            INSERT INTO agendamento (tutor_id, animal_id, veterinario_id, preco, datahora_consulta, status, observacao)
            VALUES (@TutorId, @AnimalId, @VeterinarioId, @Preco, @DataHoraConsulta, @Status, @Observacao)
            RETURNING id, created_at AS CreatedAt
            """;

        using var connection = _connectionFactory.CreateConnection();
        var resultado = await connection.QuerySingleAsync(sql, new
        {
            agendamento.TutorId,
            agendamento.AnimalId,
            agendamento.VeterinarioId,
            agendamento.Preco,
            agendamento.DataHoraConsulta,
            Status = agendamento.Status.ToString(),
            agendamento.Observacao
        });

        agendamento.SetId((long)resultado.id);
        agendamento.SetCreatedAt((DateTime)resultado.createdat);

        return agendamento;
    }

    public async Task<bool> Atualizar(Agendamento agendamento)
    {
        const string sql = """
            UPDATE agendamento
            SET tutor_id = @TutorId, animal_id = @AnimalId, 
                veterinario_id = @VeterinarioId, preco = @Preco, 
                datahora_consulta = @DataHoraConsulta,
                status = @Status, observacao = @Observacao
            WHERE id = @Id
            """;

        using var connection = _connectionFactory.CreateConnection();
        var linhasAfetadas = await connection.ExecuteAsync(sql, new
        {
            agendamento.Id,
            agendamento.TutorId,
            agendamento.AnimalId,
            agendamento.VeterinarioId,
            agendamento.Preco,
            agendamento.DataHoraConsulta,
            Status = agendamento.Status.ToString(),
            agendamento.Observacao
        });

        return linhasAfetadas > 0;
    }

    public async Task<bool> Remover(long id)
    {
        const string sql = "DELETE FROM agendamento WHERE id = @Id";

        using var connection = _connectionFactory.CreateConnection();
        var linhasAfetadas = await connection.ExecuteAsync(sql, new { Id = id });
        return linhasAfetadas > 0;
    }

    public async Task<bool> ExisteConflito(long veterinarioId, DateTime dataHoraUtc)
    {
        const string sql = """
            SELECT COUNT(1) 
            FROM agendamento 
            WHERE veterinario_id = @VeterinarioId 
              AND datahora_consulta = @DataHoraUtc
              AND status != 'CANCELADO'
            """;

        using var connection = _connectionFactory.CreateConnection();
        var count = await connection.ExecuteScalarAsync<int>(sql, new
        {
            VeterinarioId = veterinarioId,
            DataHoraUtc = dataHoraUtc
        });

        return count > 0;
    }

    public async Task<IEnumerable<Agendamento>> ListarAgendaDoDia(DateOnly dataLocal)
    {
        // No PostgreSQL, comparamos a data convertida para o fuso de Goiânia (UTC-3)
        const string sql = """
            SELECT id, tutor_id AS TutorId, animal_id AS AnimalId, 
                   veterinario_id AS VeterinarioId, preco, datahora_consulta AS DataHoraConsulta,
                   status, observacao, created_at AS CreatedAt
            FROM agendamento
            WHERE (datahora_consulta AT TIME ZONE 'UTC' AT TIME ZONE 'America/Sao_Paulo')::date = @DataLocal
            ORDER BY datahora_consulta
            """;

        using var connection = _connectionFactory.CreateConnection();
        var rows = await connection.QueryAsync(sql, new { DataLocal = dataLocal.ToDateTime(TimeOnly.MinValue) });
        return rows.Select(MapearAgendamento);
    }

    private static Agendamento MapearAgendamento(dynamic row)
    {
        var agendamento = new Agendamento(
            (long)row.tutorid,
            (long)row.animalid,
            (long)row.veterinarioid,
            (DateTime)row.datahoraconsulta,
            (string?)row.observacao
        );

        agendamento.SetId((long)row.id);
        agendamento.SetCreatedAt((DateTime)row.createdat);

        // Preenche o preço que vem do banco
        agendamento.GetType().GetProperty("Preco")!.SetValue(agendamento, (decimal)row.preco);

        // Ajusta o status que vem do banco
        if (Enum.TryParse<StatusAgendamento>((string)row.status, true, out var status))
        {
            agendamento.GetType().GetProperty("Status")!.SetValue(agendamento, status);
        }

        return agendamento;
    }
}
