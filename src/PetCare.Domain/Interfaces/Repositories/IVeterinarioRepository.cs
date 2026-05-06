using PetCare.Domain.Entities.Pessoas;

namespace PetCare.Domain.Interfaces.Repositories;

public interface IVeterinarioRepository : IRepositorioBase<Veterinario>
{
    Task<bool> ExistePorCrmv(string crmv);
}