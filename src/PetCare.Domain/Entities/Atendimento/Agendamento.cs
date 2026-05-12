using PetCare.Domain.Entities.Base;
using PetCare.Domain.Enums;
using PetCare.Domain.Exceptions;

namespace PetCare.Domain.Entities.Atendimento;

/// <summary>
/// Representa um agendamento de consulta veterinária.
/// Vincula um tutor, um animal e um veterinário a um instante específico.
/// Ciclo de vida do status: Agendado → (Concluido | Cancelado).
/// </summary>
/// <remarks>
/// IMPORTANTE: A propriedade DataHoraConsulta é sempre armazenada em UTC.
/// O Repository é responsável por garantir Kind=Utc antes de persistir,
/// e a camada de apresentação converte para o fuso local ao exibir.
/// </remarks>
public class Agendamento : EntidadeBase
{
    public long TutorId { get; private set; }
    public long AnimalId { get; private set; }
    public long VeterinarioId { get; private set; }

    /// <summary>
    /// Valor da consulta ou procedimento agendado. 
    /// Preenchido apenas na conclusão do atendimento.
    /// </summary>
    public decimal Preco { get; private set; }

    /// <summary>
    /// Data e hora da consulta, sempre em UTC.
    /// Espelha datahora_consulta (timestamptz) no banco.
    /// </summary>
    public DateTime DataHoraConsulta { get; private set; }

    /// <summary>Estado atual do agendamento. Inicia como Agendado.</summary>
    public StatusAgendamento Status { get; private set; }

    /// <summary>Observações livres (opcional). Máximo 255 caracteres.</summary>
    public string? Observacao { get; private set; }

    /// <summary>
    /// Construtor sem parâmetros para Dapper. Não usar diretamente.
    /// </summary>
    protected Agendamento()
    {
        Status = StatusAgendamento.AGENDADO;
        Observacao = null;
        Preco = 0;
    }

    /// <summary>
    /// Cria um novo agendamento. Status inicial é sempre Agendado.
    /// </summary>
    /// <param name="dataHoraConsulta">
    /// Data/hora da consulta. Será normalizada para UTC se vier em outro Kind.
    /// </param>
    public Agendamento(
        long tutorId,
        long animalId,
        long veterinarioId,
        DateTime dataHoraConsulta,
        string? observacao = null)
    {
        ValidarId(tutorId, nameof(tutorId));
        ValidarId(animalId, nameof(animalId));
        ValidarId(veterinarioId, nameof(veterinarioId));

        var dataHoraUtc = NormalizarParaUtc(dataHoraConsulta);
        ValidarDataHoraNaoPassada(dataHoraUtc);
        ValidarObservacao(observacao);

        TutorId = tutorId;
        AnimalId = animalId;
        VeterinarioId = veterinarioId;
        DataHoraConsulta = dataHoraUtc;
        Preco = 0; // Inicia zerado
        Status = StatusAgendamento.AGENDADO;
        Observacao = observacao?.Trim();
    }

    // =========================================================================
    // MÉTODOS DE COMPORTAMENTO (transições de status com regras explícitas)
    // =========================================================================

    /// <summary>
    /// Marca o agendamento como concluído e define o valor final.
    /// </summary>
    public void Concluir(decimal precoFinal)
    {
        if (Status != StatusAgendamento.AGENDADO)
            throw new DominioException(
                $"Só é possível concluir agendamentos no status Agendado. Status atual: {Status}.");

        ValidarPreco(precoFinal);
        
        Preco = precoFinal;
        Status = StatusAgendamento.CONCLUIDO;
    }

    /// <summary>
    /// Cancela o agendamento. Só permitido se ainda estiver Agendado.
    /// </summary>
    public void Cancelar()
    {
        if (Status != StatusAgendamento.AGENDADO)
            throw new DominioException(
                $"Só é possível cancelar agendamentos no status Agendado. Status atual: {Status}.");

        Status = StatusAgendamento.CANCELADO;
    }

    /// <summary>
    /// Reagenda a consulta para uma nova data/hora. Só permitido se ainda estiver Agendado.
    /// </summary>
    public void Reagendar(DateTime novaDataHora)
    {
        if (Status != StatusAgendamento.AGENDADO)
            throw new DominioException(
                $"Só é possível reagendar consultas no status Agendado. Status atual: {Status}.");

        var novaDataHoraUtc = NormalizarParaUtc(novaDataHora);
        ValidarDataHoraNaoPassada(novaDataHoraUtc);

        DataHoraConsulta = novaDataHoraUtc;
    }

    /// <summary>
    /// Atualiza a observação. Permitido em qualquer status.
    /// </summary>
    public void AtualizarObservacao(string? novaObservacao)
    {
        ValidarObservacao(novaObservacao);
        Observacao = novaObservacao?.Trim();
    }

    // =========================================================================
    // VALIDAÇÕES E NORMALIZAÇÕES PRIVADAS
    // =========================================================================

    /// <summary>
    /// Garante que a DateTime esteja em UTC. Se vier como Local, converte.
    /// Se vier como Unspecified, assume Local e converte.
    /// </summary>
    private static DateTime NormalizarParaUtc(DateTime dataHora) => dataHora.Kind switch
    {
        DateTimeKind.Utc => dataHora,
        DateTimeKind.Local => dataHora.ToUniversalTime(),
        DateTimeKind.Unspecified => DateTime.SpecifyKind(dataHora, DateTimeKind.Local).ToUniversalTime(),
        _ => dataHora.ToUniversalTime()
    };

    private static void ValidarId(long id, string nomeCampo)
    {
        if (id <= 0)
            throw new DominioException($"{nomeCampo} deve ser um identificador válido (maior que zero).");
    }

    private static void ValidarDataHoraNaoPassada(DateTime dataHoraUtc)
    {
        if (dataHoraUtc < DateTime.UtcNow)
            throw new DominioException("Não é possível agendar consultas em data/hora no passado.");
    }

    private static void ValidarObservacao(string? observacao)
    {
        if (observacao is not null && observacao.Trim().Length > 255)
            throw new DominioException("Observação deve ter no máximo 255 caracteres.");
    }

    private static void ValidarPreco(decimal preco)
    {
        if (preco <= 0)
            throw new DominioException("O preço não pode ser negativo.");
    }
}
