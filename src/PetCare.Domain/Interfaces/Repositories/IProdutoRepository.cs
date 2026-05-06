using PetCare.Domain.Entities.Estoque;

namespace PetCare.Domain.Interfaces.Repositories;

public interface IProdutoRepository : IRepositorioBase<Produto>
{
    /// <summary>
    /// Lista produtos com estoque atual igual ou abaixo do estoque mínimo definido.
    /// Usado no alerta do dashboard (RF07).
    /// </summary>
    Task<IEnumerable<Produto>> ListarComEstoqueBaixo();

    /// <summary>
    /// Lista produtos com validade próxima (até 30 dias).
    /// Usa a view vw_produtos_a_vencer.
    /// </summary>
    Task<IEnumerable<Produto>> ListarProximosDoVencimento();
}