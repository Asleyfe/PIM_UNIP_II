using PetCare.Domain.Entities.Animais;

namespace PetCare.Domain.Interfaces.Repositories;

/// <summary>
/// Interface específica para o repositório de Animais.
/// </summary>
public interface IAnimalRepository : IRepositorioBase<Animal>
{
    /// <summary>
    /// Lista todos os animais vinculados a um tutor específico.
    /// </summary>
    Task<IEnumerable<Animal>> ObterPorTutorId(long tutorId);
}