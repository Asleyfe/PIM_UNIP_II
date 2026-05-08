using PetCare.Domain.Entities.Base;

namespace PetCare.Domain.Entities.Estoque;

/// <summary>
/// Representa um produto no catálogo do pet shop.
/// </summary>
public class Produto : EntidadeBase
{
    public string Nome { get; private set; } = string.Empty;
    public decimal Preco { get; private set; }
    public string? Descricao { get; private set; }
    public string Categoria { get; private set; } = string.Empty;
    public int QuantidadeEstoque { get; private set; }
    public int EstoqueMinimo { get; private set; }
    public DateTime? Validade { get; private set; }

    // Construtor para o Dapper/ORM
    protected Produto() { }

    public Produto(string nome, decimal preco, string categoria, int estoqueMinimo, string? descricao = null, DateTime? validade = null)
    {
        Validar(nome, preco, categoria, estoqueMinimo, validade);

        Nome = nome;
        Preco = preco;
        Categoria = categoria;
        EstoqueMinimo = estoqueMinimo;
        Descricao = descricao;
        Validade = validade;
        QuantidadeEstoque = 0;
        CreatedAt = DateTime.UtcNow;
    }

    public void AtualizarEstoque(int quantidade)
    {
        // Regra do RF005: Estoque pode ser negativo em caso de não trabalhar mais com o produto
        QuantidadeEstoque += quantidade;
    }

    public bool PrecisaReposição() => QuantidadeEstoque <= EstoqueMinimo;

    public bool EstaVencido() => Validade.HasValue && Validade.Value < DateTime.UtcNow;

    public bool ProximoAoVencimento(int dias = 30) 
        => Validade.HasValue && !EstaVencido() && (Validade.Value - DateTime.UtcNow).TotalDays <= dias;

    private void Validar(string nome, decimal preco, string categoria, int estoqueMinimo, DateTime? validade)
    {
        if (string.IsNullOrWhiteSpace(nome))
            throw new ArgumentException("O nome do produto é obrigatório.");

        if (preco <= 0)
            throw new ArgumentException("O preço deve ser maior que zero.");

        if (string.IsNullOrWhiteSpace(categoria))
            throw new ArgumentException("A categoria é obrigatória.");

        if (estoqueMinimo < 0)
            throw new ArgumentException("O estoque mínimo não pode ser negativo.");

        // Regra do RF005: Validade deve ser obrigatória para medicamentos e rações
        var categoriasPereciveis = new[] { "Rações", "Medicamentos", "Ração", "Medicamento" };
        if (categoriasPereciveis.Contains(categoria) && !validade.HasValue)
        {
            throw new ArgumentException($"A validade é obrigatória para a categoria {categoria}.");
        }
    }
}