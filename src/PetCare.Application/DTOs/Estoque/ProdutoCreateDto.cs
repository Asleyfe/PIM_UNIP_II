namespace PetCare.Application.DTOs.Estoque;

public class ProdutoCreateDto
{
    public string Nome { get; set; } = string.Empty;
    public decimal Preco { get; set; }
    public string? Descricao { get; set; }
    public string Categoria { get; set; } = string.Empty;
    public int EstoqueMinimo { get; set; }
    public DateTime? Validade { get; set; }
}
