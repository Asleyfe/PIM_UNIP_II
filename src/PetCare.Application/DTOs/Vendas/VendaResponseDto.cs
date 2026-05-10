namespace PetCare.Application.DTOs.Vendas;

public class VendaResponseDto
{
    public long Id { get; set; }
    public long TutorId { get; set; }
    public string TutorNome { get; set; } = string.Empty;
    public DateTime DataVenda { get; set; }
    public decimal ValorTotal { get; set; }
    public string FormaPagamento { get; set; } = string.Empty;
    public string? Observacao { get; set; }
    public List<ItemVendaResponseDto> Itens { get; set; } = new();
    public DateTime CreatedAt { get; set; }
}

public class ItemVendaResponseDto
{
    public long Id { get; set; }
    public long ProdutoId { get; set; }
    public string ProdutoNome { get; set; } = string.Empty;
    public int Quantidade { get; set; }
    public decimal PrecoUnitario { get; set; }
    public decimal Subtotal => Quantidade * PrecoUnitario;
}
