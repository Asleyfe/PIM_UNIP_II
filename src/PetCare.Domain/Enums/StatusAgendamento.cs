namespace PetCare.Domain.Enums;

/// <summary>
/// Estados possíveis de um agendamento.
/// Espelha a constraint chk_status_agendamento da tabela agendamento.
/// </summary>
public enum StatusAgendamento
{
    Agendado,
    Cancelado,
    Concluido
}