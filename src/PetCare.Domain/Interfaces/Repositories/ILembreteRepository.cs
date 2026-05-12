using PetCare.Domain.Entities.Comunicacao;

namespace PetCare.Domain.Interfaces.Repositories;

public interface ILembreteRepository : IRepositorioBase<LembreteEnviado>
{
    Task<IEnumerable<LembreteEnviado>> ListarPorTutor(long tutorId);
    Task<IEnumerable<LembreteEnviado>> ListarPorAnimal(long animalId);
}
