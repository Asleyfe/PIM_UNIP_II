using Dapper;
using PetCare.Domain.Entities.Atendimento;
using PetCare.Domain.Interfaces.Repositories;
using PetCare.Infrastructure.Data;

namespace PetCare.Infrastructure.Repositories;

public class ProntuarioRepository : IProntuarioRepository
{
    private readonly IConnectionFactory _connectionFactory;

    public ProntuarioRepository(IConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<Prontuario?> ObterPorId(long id)
    {
        const string sql = """
            SELECT id, agendamento_id AS AgendamentoId, queixa_principal AS QueixaPrincipal, 
                   relato_tutor AS RelatoTutor, exame_fisico AS ExameFisico, 
                   diagnostico, prescricao, observacoes, 
                   data_registro AS DataRegistro, data_registro AS CreatedAt
            FROM prontuario
            WHERE id = @Id
            """;

        using var connection = _connectionFactory.CreateConnection();
        var row = await connection.QuerySingleOrDefaultAsync(sql, new { Id = id });
        return row is null ? null : MapearProntuario(row);
    }

    public async Task<IEnumerable<Prontuario>> Listar()
    {
        const string sql = """
            SELECT id, agendamento_id AS AgendamentoId, queixa_principal AS QueixaPrincipal, 
                   relato_tutor AS RelatoTutor, exame_fisico AS ExameFisico, 
                   diagnostico, prescricao, observacoes, 
                   data_registro AS DataRegistro, data_registro AS CreatedAt
            FROM prontuario
            ORDER BY data_registro DESC
            """;

        using var connection = _connectionFactory.CreateConnection();
        var rows = await connection.QueryAsync(sql);
        return rows.Select(MapearProntuario);
    }

    public async Task<Prontuario> Inserir(Prontuario prontuario)
    {
        const string sql = """
            INSERT INTO prontuario (agendamento_id, queixa_principal, relato_tutor, exame_fisico, diagnostico, prescricao, observacoes, data_registro)
            VALUES (@AgendamentoId, @QueixaPrincipal, @RelatoTutor, @ExameFisico, @Diagnostico, @Prescricao, @Observacoes, @DataRegistro)
            RETURNING id
            """;

        using var connection = _connectionFactory.CreateConnection();
        var resultado = await connection.QuerySingleAsync(sql, new
        {
            prontuario.AgendamentoId,
            prontuario.QueixaPrincipal,
            prontuario.RelatoTutor,
            prontuario.ExameFisico,
            prontuario.Diagnostico,
            prontuario.Prescricao,
            prontuario.Observacoes,
            prontuario.DataRegistro
        });

        prontuario.SetId((long)resultado.id);
        prontuario.SetCreatedAt(prontuario.DataRegistro);

        return prontuario;
    }

    public async Task<bool> Atualizar(Prontuario prontuario)
    {
        const string sql = """
            UPDATE prontuario
            SET queixa_principal = @QueixaPrincipal, relato_tutor = @RelatoTutor, 
                exame_fisico = @ExameFisico, diagnostico = @Diagnostico, 
                prescricao = @Prescricao, observacoes = @Observacoes
            WHERE id = @Id
            """;

        using var connection = _connectionFactory.CreateConnection();
        var linhasAfetadas = await connection.ExecuteAsync(sql, new
        {
            prontuario.Id,
            prontuario.QueixaPrincipal,
            prontuario.RelatoTutor,
            prontuario.ExameFisico,
            prontuario.Diagnostico,
            prontuario.Prescricao,
            prontuario.Observacoes
        });

        return linhasAfetadas > 0;
    }

    public async Task<bool> Remover(long id)
    {
        const string sql = "DELETE FROM prontuario WHERE id = @Id";

        using var connection = _connectionFactory.CreateConnection();
        var linhasAfetadas = await connection.ExecuteAsync(sql, new { Id = id });
        return linhasAfetadas > 0;
    }

    public async Task<Prontuario?> ObterPorAgendamento(long agendamentoId)
    {
        const string sql = """
            SELECT id, agendamento_id AS AgendamentoId, queixa_principal AS QueixaPrincipal, 
                   relato_tutor AS RelatoTutor, exame_fisico AS ExameFisico, 
                   diagnostico, prescricao, observacoes, 
                   data_registro AS DataRegistro, data_registro AS CreatedAt
            FROM prontuario
            WHERE agendamento_id = @AgendamentoId
            """;

        using var connection = _connectionFactory.CreateConnection();
        var row = await connection.QuerySingleOrDefaultAsync(sql, new { AgendamentoId = agendamentoId });
        return row is null ? null : MapearProntuario(row);
    }

    private static Prontuario MapearProntuario(dynamic row)
    {
        // Usando reflexão para o mapeamento completo, incluindo campos privados e datas
        var prontuario = (Prontuario)Activator.CreateInstance(typeof(Prontuario), true)!;
        
        prontuario.SetId((long)row.id);
        prontuario.SetCreatedAt((DateTime)row.createdat);
        
        prontuario.GetType().GetProperty("AgendamentoId")!.SetValue(prontuario, (long)row.agendamentoid);
        prontuario.GetType().GetProperty("QueixaPrincipal")!.SetValue(prontuario, (string)row.queixaprincipal);
        prontuario.GetType().GetProperty("RelatoTutor")!.SetValue(prontuario, (string?)row.relatotutor);
        prontuario.GetType().GetProperty("ExameFisico")!.SetValue(prontuario, (string?)row.examefisico);
        prontuario.GetType().GetProperty("Diagnostico")!.SetValue(prontuario, (string?)row.diagnostico);
        prontuario.GetType().GetProperty("Prescricao")!.SetValue(prontuario, (string?)row.prescricao);
        prontuario.GetType().GetProperty("Observacoes")!.SetValue(prontuario, (string?)row.observacoes);
        prontuario.GetType().GetProperty("DataRegistro")!.SetValue(prontuario, (DateTime)row.dataregistro);

        return prontuario;
    }
}
