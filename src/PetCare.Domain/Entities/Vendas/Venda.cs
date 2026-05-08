using PetCare.Domain.Entities.Base;
using PetCare.Domain.Exceptions;

namespace PetCare.Domain.Entities.Vendas;

/// <summary>
/// Representa uma venda realizada na clínica ou pet shop.
/// </summary>
public class Venda : EntidadeBase
{
    public long TutorId { get; private set; }
    public DateTime DataVenda { get; private set; }
    public decimal ValorTotal { get; private set; }
    public string FormaPagamento { get; private set; } = string.Empty;
    public string? Observacao { get; private set; }

    private readonly List<ItemVenda> _itens = new();
    public IReadOnlyCollection<ItemVenda> Itens => _itens.AsReadOnly();

    // Construtor para o Dapper
    protected Venda() { }

    public Venda(long tutorId, string formaPagamento, string? observacao = null)
    {
        if (tutorId <= 0)
            throw new DominioException("O ID do tutor é obrigatório.");

        if (string.IsNullOrWhiteSpace(formaPagamento))
            throw new DominioException("A forma de pagamento é obrigatória.");

        TutorId = tutorId;
        FormaPagamento = formaPagamento;
        Observacao = observacao;
        DataVenda = DateTime.UtcNow;
        ValorTotal = 0;
    }

    public void AdicionarItem(long produtoId, int quantidade, decimal precoUnitario)
    {
        var item = new ItemVenda(Id, produtoId, quantidade, precoUnitario);
        _itens.Add(item);
        CalcularValorTotal();
    }

    private void CalcularValorTotal()
    {
        ValorTotal = _itens.Sum(i => i.Quantidade * i.PrecoUnitario);
    }
}