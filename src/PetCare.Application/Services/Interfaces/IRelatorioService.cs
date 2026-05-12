using PetCare.Application.DTOs.Relatorios;
using PetCare.Domain.DTOs.Relatorios;

namespace PetCare.Application.Services.Interfaces;

public interface IRelatorioService
{
    Task<RelatorioFaturamentoDto> ObterFaturamentoPeriodo(DateTime inicio, DateTime fim);
    Task<IEnumerable<RelatorioDesempenhoProfissionalDto>> ObterDesempenhoProfissionais(DateTime inicio, DateTime fim);
    Task<IEnumerable<RelatorioProdutosMaisVendidosDto>> ObterProdutosMaisVendidos(DateTime inicio, DateTime fim, int top = 10);
}
