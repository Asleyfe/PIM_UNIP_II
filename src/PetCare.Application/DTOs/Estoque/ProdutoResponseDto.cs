namespace PetCare.Application.DTOs.Estoque;

public class ProdutoResponseDto
{
    public long Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public decimal Preco { get; set; }
    public string? Descricao { get; set; }
    public string Categoria { get; set; } = string.Empty;
    public int QuantidadeEstoque { get; set; }
    public int EstoqueMinimo { get; set; }
    public DateTime? Validade { get; set; }
    public bool PrecisaReposicao { get; set; }
    public bool EstaVencido { get; set; }
    public bool ProximoAoVencimento { get; set; }
}
