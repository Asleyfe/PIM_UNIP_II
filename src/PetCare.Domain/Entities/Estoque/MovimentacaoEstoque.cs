using PetCare.Domain.Entities.Base;
using PetCare.Domain.Enums;

namespace PetCare.Domain.Entities.Estoque;

/// <summary>
/// Registra uma movimentação (entrada ou saída) de estoque.
/// </summary>
public class MovimentacaoEstoque : EntidadeBase
{
    public long ProdutoId { get; private set; }
    public int Quantidade { get; private set; }
    public TipoMovimentacao Tipo { get; private set; }
    public DateTime DataMovimentacao { get; private set; }

    // Construtor para o Dapper
    protected MovimentacaoEstoque() { }

    public MovimentacaoEstoque(long produtoId, int quantidade, TipoMovimentacao tipo)
    {
        if (produtoId <= 0)
            throw new ArgumentException("ID do produto inválido.");

        if (quantidade <= 0)
            throw new ArgumentException("A quantidade da movimentação deve ser maior que zero.");

        ProdutoId = produtoId;
        Quantidade = quantidade;
        Tipo = tipo;
        DataMovimentacao = DateTime.UtcNow;
    }
}