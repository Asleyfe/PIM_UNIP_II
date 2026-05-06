using PetCare.Domain.Entities.Estoque;

namespace PetCare.Domain.Interfaces.Repositories;

public interface IMovimentacaoEstoqueRepository : IRepositorioBase<MovimentacaoEstoque>
{
    /// <summary>
    /// Lista todas as movimentações de um produto específico, ordenadas por data desc.
    /// </summary>
    Task<IEnumerable<MovimentacaoEstoque>> ListarPorProduto(long produtoId);
}