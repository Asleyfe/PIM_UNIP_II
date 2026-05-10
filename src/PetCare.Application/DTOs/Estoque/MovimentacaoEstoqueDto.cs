namespace PetCare.Application.DTOs.Estoque;

public class MovimentacaoEstoqueDto
{
    public long ProdutoId { get; set; }
    public int Quantidade { get; set; }
    public string Tipo { get; set; } = string.Empty; // "ENTRADA" ou "SAIDA"
    public DateTime DataMovimentacao { get; set; }
}
