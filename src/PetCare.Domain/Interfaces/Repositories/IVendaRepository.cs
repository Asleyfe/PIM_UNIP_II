using PetCare.Domain.Entities.Vendas;

namespace PetCare.Domain.Interfaces.Repositories;

public interface IVendaRepository : IRepositorioBase<Venda>
{
    /// <summary>
    /// Insere venda + itens em transação única.
    /// As triggers do banco cuidam da baixa automática no estoque.
    /// </summary>
    Task<Venda> InserirComItens(Venda venda, IEnumerable<ItemVenda> itens);

    /// <summary>
    /// Lista vendas de um período específico.
    /// </summary>
    Task<IEnumerable<Venda>> ListarPorPeriodo(DateTime inicio, DateTime fim);
}