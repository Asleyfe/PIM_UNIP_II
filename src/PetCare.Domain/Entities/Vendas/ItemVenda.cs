using PetCare.Domain.Entities.Base;
using PetCare.Domain.Exceptions;

namespace PetCare.Domain.Entities.Vendas;

/// <summary>
/// Representa um item individual dentro de uma venda.
/// </summary>
public class ItemVenda : EntidadeBase
{
    public long VendaId { get; private set; }
    public long ProdutoId { get; private set; }
    public int Quantidade { get; private set; }
    public decimal PrecoUnitario { get; private set; }

    // Construtor para o Dapper
    protected ItemVenda() { }

    public ItemVenda(long vendaId, long produtoId, int quantidade, decimal precoUnitario)
    {
        if (produtoId <= 0)
            throw new DominioException("O ID do produto é obrigatório.");

        if (quantidade <= 0)
            throw new DominioException("A quantidade deve ser maior que zero.");

        if (precoUnitario <= 0)
            throw new DominioException("O preço unitário deve ser maior que zero.");

        VendaId = vendaId;
        ProdutoId = produtoId;
        Quantidade = quantidade;
        PrecoUnitario = precoUnitario;
    }
}