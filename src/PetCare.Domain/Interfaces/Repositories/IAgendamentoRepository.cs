using PetCare.Domain.Entities.Atendimento;

namespace PetCare.Domain.Interfaces.Repositories;

public interface IAgendamentoRepository : IRepositorioBase<Agendamento>
{
    /// <summary>
    /// Verifica se já existe agendamento ativo (status diferente de Cancelado)
    /// para o veterinário no instante informado. Usado na validação de conflito (RF04).
    /// </summary>
    /// <param name="dataHoraUtc">Instante a verificar (em UTC).</param>
    Task<bool> ExisteConflito(long veterinarioId, DateTime dataHoraUtc);

    /// <summary>
    /// Lista agendamentos do dia (no fuso local) para visualização da agenda.
    /// </summary>
    /// <param name="dataLocal">Data alvo no fuso local de Goiânia (UTC-3).</param>
    Task<IEnumerable<Agendamento>> ListarAgendaDoDia(DateOnly dataLocal);
}