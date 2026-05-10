namespace PetCare.Application.DTOs.Vendas;

public class VendaCreateDto
{
    public long TutorId { get; set; }
    public string FormaPagamento { get; set; } = string.Empty;
    public string? Observacao { get; set; }
    public List<ItemVendaCreateDto> Itens { get; set; } = new();
}

public class ItemVendaCreateDto
{
    public long ProdutoId { get; set; }
    public int Quantidade { get; set; }
    public decimal PrecoUnitario { get; set; }
}
