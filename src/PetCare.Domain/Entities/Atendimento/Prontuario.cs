using PetCare.Domain.Entities.Base;
using PetCare.Domain.Exceptions;

namespace PetCare.Domain.Entities.Atendimento;

/// <summary>
/// Representa o prontuário digital de um animal em uma consulta específica.
/// Vinculado 1:1 com um Agendamento.
/// </summary>
public class Prontuario : EntidadeBase
{
    public long AgendamentoId { get; private set; }
    public string QueixaPrincipal { get; private set; } = string.Empty;
    public string? RelatoTutor { get; private set; }
    public string? ExameFisico { get; private set; }
    public string? Diagnostico { get; private set; }
    public string? Prescricao { get; private set; }
    public string? Observacoes { get; private set; }
    public DateTime DataRegistro { get; private set; }

    // Construtor para o Dapper
    protected Prontuario() { }

    public Prontuario(
        long agendamentoId, 
        string queixaPrincipal, 
        string? relatoTutor = null, 
        string? exameFisico = null, 
        string? diagnostico = null, 
        string? prescricao = null, 
        string? observacoes = null)
    {
        if (agendamentoId <= 0)
            throw new DominioException("O ID do agendamento é obrigatório.");

        if (string.IsNullOrWhiteSpace(queixaPrincipal))
            throw new DominioException("A queixa principal é obrigatória.");

        AgendamentoId = agendamentoId;
        QueixaPrincipal = queixaPrincipal.Trim();
        RelatoTutor = relatoTutor?.Trim();
        ExameFisico = exameFisico?.Trim();
        Diagnostico = diagnostico?.Trim();
        Prescricao = prescricao?.Trim();
        Observacoes = observacoes?.Trim();
        DataRegistro = DateTime.UtcNow;
    }

    public void Atualizar(
        string queixaPrincipal, 
        string? relatoTutor, 
        string? exameFisico, 
        string? diagnostico, 
        string? prescricao, 
        string? observacoes)
    {
        if (string.IsNullOrWhiteSpace(queixaPrincipal))
            throw new DominioException("A queixa principal é obrigatória.");

        QueixaPrincipal = queixaPrincipal.Trim();
        RelatoTutor = relatoTutor?.Trim();
        ExameFisico = exameFisico?.Trim();
        Diagnostico = diagnostico?.Trim();
        Prescricao = prescricao?.Trim();
        Observacoes = observacoes?.Trim();
    }
}