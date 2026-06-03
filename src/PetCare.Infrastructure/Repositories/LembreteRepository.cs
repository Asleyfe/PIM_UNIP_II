using Dapper;
using PetCare.Domain.Entities.Comunicacao;
using PetCare.Domain.Enums;
using PetCare.Domain.Interfaces.Repositories;
using PetCare.Infrastructure.Data;

namespace PetCare.Infrastructure.Repositories;

public class LembreteRepository : ILembreteRepository
{
    private readonly IConnectionFactory _connectionFactory;

    public LembreteRepository(IConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<LembreteEnviado?> ObterPorId(long id)
    {
        const string sql = """
            SELECT id, tutor_id as TutorId, animal_id as AnimalId, agendamento_id as AgendamentoId,
                   tipo, meio_envio as MeioEnvio, status_envio as StatusEnvio, data_envio as DataEnvio,
                   mensagem, data_envio as CreatedAt
            FROM lembrete_enviado
            WHERE id = @Id
            """;

        using var connection = _connectionFactory.CreateConnection();
        return await connection.QuerySingleOrDefaultAsync<LembreteEnviado>(sql, new { Id = id });
    }

    public async Task<IEnumerable<LembreteEnviado>> Listar()
    {
        const string sql = """
            SELECT id, tutor_id as TutorId, animal_id as AnimalId, agendamento_id as AgendamentoId,
                   tipo, meio_envio as MeioEnvio, status_envio as StatusEnvio, data_envio as DataEnvio,
                   mensagem, data_envio as CreatedAt
            FROM lembrete_enviado
            ORDER BY data_envio DESC
            """;

        using var connection = _connectionFactory.CreateConnection();
        return await connection.QueryAsync<LembreteEnviado>(sql);
    }

    public async Task<LembreteEnviado> Inserir(LembreteEnviado lembrete)
    {
        const string sql = """
            INSERT INTO lembrete_enviado (tutor_id, animal_id, agendamento_id, tipo, meio_envio, status_envio, data_envio, mensagem)
            VALUES (@TutorId, @AnimalId, @AgendamentoId, @Tipo, @MeioEnvio, @StatusEnvio, @DataEnvio, @Mensagem)
            RETURNING id
            """;

        using var connection = _connectionFactory.CreateConnection();
        var resultado = await connection.QuerySingleAsync(sql, new
        {
            lembrete.TutorId,
            lembrete.AnimalId,
            lembrete.AgendamentoId,
            lembrete.Tipo,
            lembrete.MeioEnvio,
            lembrete.StatusEnvio,
            lembrete.DataEnvio,
            lembrete.Mensagem
        });

        lembrete.SetId((long)resultado.id);
        lembrete.SetCreatedAt(lembrete.DataEnvio);

        return lembrete;
    }

    public async Task<bool> Atualizar(LembreteEnviado lembrete)
    {
        const string sql = """
            UPDATE lembrete_enviado
            SET status_envio = @StatusEnvio, mensagem = @Mensagem
            WHERE id = @Id
            """;

        using var connection = _connectionFactory.CreateConnection();
        var rows = await connection.ExecuteAsync(sql, new
        {
            lembrete.StatusEnvio,
            lembrete.Mensagem,
            lembrete.Id
        });

        return rows > 0;
    }

    public async Task<bool> Remover(long id)
    {
        const string sql = "DELETE FROM lembrete_enviado WHERE id = @Id";

        using var connection = _connectionFactory.CreateConnection();
        var rows = await connection.ExecuteAsync(sql, new { Id = id });
        return rows > 0;
    }

    public async Task<IEnumerable<LembreteEnviado>> ListarPorTutor(long tutorId)
    {
        const string sql = """
            SELECT id, tutor_id as TutorId, animal_id as AnimalId, agendamento_id as AgendamentoId,
                   tipo, meio_envio as MeioEnvio, status_envio as StatusEnvio, data_envio as DataEnvio,
                   mensagem, data_envio as CreatedAt
            FROM lembrete_enviado
            WHERE tutor_id = @TutorId
            ORDER BY data_envio DESC
            """;

        using var connection = _connectionFactory.CreateConnection();
        return await connection.QueryAsync<LembreteEnviado>(sql, new { TutorId = tutorId });
    }

    public async Task<IEnumerable<LembreteEnviado>> ListarPorAnimal(long animalId)
    {
        const string sql = """
            SELECT id, tutor_id as TutorId, animal_id as AnimalId, agendamento_id as AgendamentoId,
                   tipo, meio_envio as MeioEnvio, status_envio as StatusEnvio, data_envio as DataEnvio,
                   mensagem, data_envio as CreatedAt
            FROM lembrete_enviado
            WHERE animal_id = @AnimalId
            ORDER BY data_envio DESC
            """;

        using var connection = _connectionFactory.CreateConnection();
        return await connection.QueryAsync<LembreteEnviado>(sql, new { AnimalId = animalId });
    }
}
