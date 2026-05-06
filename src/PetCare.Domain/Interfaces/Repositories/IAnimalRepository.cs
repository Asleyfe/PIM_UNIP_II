using PetCare.Domain.Entities.Animais;

namespace PetCare.Domain.Interfaces.Repositories;

public interface IAnimalRepository : IRepositorioBase<Animal>
{
    /// <summary>
    /// Lista todos os animais vinculados a um tutor específico.
    /// </summary>
    Task<IEnumerable<Animal>> ListarPorTutor(long tutorId);
}