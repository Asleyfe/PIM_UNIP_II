using PetCare.Domain.DTOs.Relatorios;

namespace PetCare.Application.DTOs.Relatorios;

public class RelatorioFaturamentoDto
{
    public DateTime Inicio { get; set; }
    public DateTime Fim { get; set; }
    public decimal FaturamentoTotal { get; set; }
    public List<FaturamentoPorCategoriaDto> PorCategoria { get; set; } = new();
    public List<FaturamentoMensalDto> EvolucaoMensal { get; set; } = new();
}
