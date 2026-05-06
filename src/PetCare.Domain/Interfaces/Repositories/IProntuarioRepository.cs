using PetCare.Domain.Entities.Atendimento;

namespace PetCare.Domain.Interfaces.Repositories;

public interface IProntuarioRepository : IRepositorioBase<Prontuario>
{
    /// <summary>
    /// Busca o prontuário vinculado a um agendamento específico.
    /// Relacionamento 1:1 com agendamento.
    /// </summary>
    Task<Prontuario?> ObterPorAgendamento(long agendamentoId);
}