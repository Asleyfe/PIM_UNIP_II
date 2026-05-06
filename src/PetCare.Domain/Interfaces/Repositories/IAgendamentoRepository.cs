using PetCare.Domain.Entities.Atendimento;

namespace PetCare.Domain.Interfaces.Repositories;

public interface IAgendamentoRepository : IRepositorioBase<Agendamento>
{
    /// <summary>
    /// Verifica se já existe agendamento ativo (não cancelado) para o veterinário
    /// na data e hora informadas. Usado na validação de conflito (RF04).
    /// </summary>
    Task<bool> ExisteConflito(long veterinarioId, DateTime dataHora);

    /// <summary>
    /// Lista agendamentos do dia agrupados por veterinário.
    /// Usa a view vw_agenda_dia.
    /// </summary>
    Task<IEnumerable<Agendamento>> ListarAgendaDoDia(DateTime data);
}