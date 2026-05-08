using PetCare.Domain.Entities.Pessoas;

namespace PetCare.Domain.Interfaces.Repositories;

public interface ITutorRepository : IRepositorioBase<Tutor>
{
    /// <summary>
    /// Verifica se já existe um tutor cadastrado com o CPF informado.
    /// Usado na validação de unicidade antes de inserir.
    /// </summary>
    Task<bool> ExistePorCpf(string cpf);

    /// <summary>
    /// Busca tutores por nome (busca parcial), CPF (exato) ou telefone.
    /// Usado pelo autocomplete na tela de agendamento.
    /// </summary>
    Task<IEnumerable<Tutor>> BuscarPorTermo(string termo);
}