using PetCare.Domain.Entities.Atendimento;

namespace PetCare.Domain.Interfaces.Repositories;

public interface IHistoricoRepository : IRepositorioBase<HistoricoClinico>
{
    Task<IEnumerable<HistoricoClinico>> ListarPorAnimal(long animalId);
}
