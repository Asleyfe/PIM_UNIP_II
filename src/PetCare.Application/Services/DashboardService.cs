using PetCare.Application.DTOs.Dashboard;
using PetCare.Application.Services.Interfaces;
using PetCare.Domain.Interfaces.Repositories;

namespace PetCare.Application.Services;

public class DashboardService : IDashboardService
{
    private readonly IAgendamentoRepository _agendamentoRepository;
    private readonly IVendaRepository _vendaRepository;
    private readonly IProdutoRepository _produtoRepository;

    public DashboardService(
        IAgendamentoRepository agendamentoRepository,
        IVendaRepository vendaRepository,
        IProdutoRepository produtoRepository)
    {
        _agendamentoRepository = agendamentoRepository;
        _vendaRepository = vendaRepository;
        _produtoRepository = produtoRepository;
    }

    public async Task<DashboardResponseDto> ObterIndicadores()
    {
        var hoje = DateOnly.FromDateTime(DateTime.Now); // Considerar fuso local se necessário
        var primeiroDiaMes = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
        var ultimoDiaMes = primeiroDiaMes.AddMonths(1).AddDays(-1);

        var atendimentosHoje = await _agendamentoRepository.ListarAgendaDoDia(hoje);
        var vendasMes = await _vendaRepository.ListarPorPeriodo(primeiroDiaMes, ultimoDiaMes);
        var produtosBaixoEstoque = await _produtoRepository.ListarComEstoqueBaixo();
        var produtosVencendo = await _produtoRepository.ListarProximosDoVencimento();

        var dto = new DashboardResponseDto
        {
            AtendimentosHoje = atendimentosHoje.Count(),
            FaturamentoMensal = vendasMes.Sum(v => v.ValorTotal),
            ProdutosEstoqueBaixo = produtosBaixoEstoque.Count(),
            ProdutosVencendo = produtosVencendo.Count()
        };

        if (dto.ProdutosEstoqueBaixo > 0)
            dto.Alertas.Add($"{dto.ProdutosEstoqueBaixo} produtos com estoque baixo.");

        if (dto.ProdutosVencendo > 0)
            dto.Alertas.Add($"{dto.ProdutosVencendo} produtos próximos ao vencimento.");

        return dto;
    }
}
