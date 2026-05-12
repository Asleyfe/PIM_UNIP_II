namespace PetCare.Application.DTOs.Dashboard;

public class DashboardResponseDto
{
    public int AtendimentosHoje { get; set; }
    public decimal FaturamentoMensal { get; set; }
    public int ProdutosEstoqueBaixo { get; set; }
    public int ProdutosVencendo { get; set; }
    
    // Podemos adicionar uma lista simplificada de alertas se necessário
    public List<string> Alertas { get; set; } = new();
}
