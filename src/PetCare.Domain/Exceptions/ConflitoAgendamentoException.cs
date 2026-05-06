namespace PetCare.Domain.Exceptions;

/// <summary>
/// Lançada quando há tentativa de agendar consulta em horário já ocupado para o mesmo veterinário.
/// Convertida em HTTP 409 (Conflict) pelo middleware.
/// </summary>
public class ConflitoAgendamentoException : DominioException
{
    public ConflitoAgendamentoException(DateTime dataHora, long veterinarioId)
        : base($"Já existe agendamento em {dataHora:dd/MM/yyyy HH:mm} para o veterinário {veterinarioId}.") { }
}