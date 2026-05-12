using PetCare.Domain.Entities.Base;
using PetCare.Domain.Enums;
using PetCare.Domain.Exceptions;

namespace PetCare.Domain.Entities.Comunicacao;

/// <summary>
/// Registro de lembretes enviados aos tutores.
/// </summary>
public class LembreteEnviado : EntidadeBase
{
    public long TutorId { get; private set; }
    public long AnimalId { get; private set; }
    public long? AgendamentoId { get; private set; }
    public TipoLembrete Tipo { get; private set; }
    public MeioEnvio MeioEnvio { get; private set; }
    public StatusEnvio StatusEnvio { get; private set; }
    public DateTime DataEnvio { get; private set; }
    public string? Mensagem { get; private set; }

    // Construtor para o Dapper
    protected LembreteEnviado() { }

    public LembreteEnviado(
        long tutorId, 
        long animalId, 
        TipoLembrete tipo, 
        MeioEnvio meioEnvio, 
        StatusEnvio statusEnvio, 
        string? mensagem = null,
        long? agendamentoId = null)
    {
        if (tutorId <= 0) throw new DominioException("O ID do tutor é obrigatório.");
        if (animalId <= 0) throw new DominioException("O ID do animal é obrigatório.");

        TutorId = tutorId;
        AnimalId = animalId;
        Tipo = tipo;
        MeioEnvio = meioEnvio;
        StatusEnvio = statusEnvio;
        Mensagem = mensagem;
        AgendamentoId = agendamentoId;
        DataEnvio = DateTime.UtcNow;
    }

    public void AtualizarStatus(StatusEnvio novoStatus)
    {
        StatusEnvio = novoStatus;
    }
}