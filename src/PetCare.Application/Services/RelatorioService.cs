using PetCare.Application.DTOs.Relatorios;
using PetCare.Application.Services.Interfaces;
using PetCare.Domain.DTOs.Relatorios;
using PetCare.Domain.Interfaces.Repositories;

namespace PetCare.Application.Services;

public class RelatorioService : IRelatorioService
{
    private readonly IRelatorioRepository _relatorioRepository;

    public RelatorioService(IRelatorioRepository relatorioRepository)
    {
        _relatorioRepository = relatorioRepository;
    }

    public async Task<RelatorioFaturamentoDto> ObterFaturamentoPeriodo(DateTime inicio, DateTime fim)
    {
        var faturamentoTotal = await _relatorioRepository.ObterFaturamentoTotal(inicio, fim);
        var porCategoria = await _relatorioRepository.ObterFaturamentoPorCategoria(inicio, fim);
        var evolucaoMensal = await _relatorioRepository.ObterFaturamentoMensal(inicio, fim);

        return new RelatorioFaturamentoDto
        {
            Inicio = inicio,
            Fim = fim,
            FaturamentoTotal = faturamentoTotal,
            PorCategoria = porCategoria.ToList(),
            EvolucaoMensal = evolucaoMensal.ToList()
        };
    }

    public async Task<IEnumerable<RelatorioDesempenhoProfissionalDto>> ObterDesempenhoProfissionais(DateTime inicio, DateTime fim)
    {
        return await _relatorioRepository.ObterDesempenhoProfissionais(inicio, fim);
    }

    public async Task<IEnumerable<RelatorioProdutosMaisVendidosDto>> ObterProdutosMaisVendidos(DateTime inicio, DateTime fim, int top = 10)
    {
        return await _relatorioRepository.ObterProdutosMaisVendidos(inicio, fim, top);
    }
}
