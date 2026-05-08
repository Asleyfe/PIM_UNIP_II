using PetCare.Domain.Entities.Base;
using PetCare.Domain.Exceptions;

namespace PetCare.Domain.Entities.Atendimento;

/// <summary>
/// Representa uma entrada no histórico clínico consolidado de um animal.
/// Pode ser gerado a partir de um prontuário ou de um registro avulso.
/// </summary>
public class HistoricoClinico : EntidadeBase
{
    public long AnimalId { get; private set; }
    public long VeterinarioId { get; private set; }
    public string Descricao { get; private set; } = string.Empty;
    public DateTime DataRegistro { get; private set; }

    // Construtor para o Dapper
    protected HistoricoClinico() { }

    public HistoricoClinico(long animalId, long veterinarioId, string descricao)
    {
        if (animalId <= 0)
            throw new DominioException("O ID do animal é obrigatório.");

        if (veterinarioId <= 0)
            throw new DominioException("O ID do veterinário é obrigatório.");

        if (string.IsNullOrWhiteSpace(descricao))
            throw new DominioException("A descrição do histórico é obrigatória.");

        AnimalId = animalId;
        VeterinarioId = veterinarioId;
        Descricao = descricao.Trim();
        DataRegistro = DateTime.UtcNow;
    }
}