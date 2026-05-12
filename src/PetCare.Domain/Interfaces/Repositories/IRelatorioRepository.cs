using PetCare.Domain.DTOs.Relatorios;

namespace PetCare.Domain.Interfaces.Repositories;

public interface IRelatorioRepository
{
    Task<decimal> ObterFaturamentoTotal(DateTime inicio, DateTime fim);
    Task<IEnumerable<FaturamentoPorCategoriaDto>> ObterFaturamentoPorCategoria(DateTime inicio, DateTime fim);
    Task<IEnumerable<FaturamentoMensalDto>> ObterFaturamentoMensal(DateTime inicio, DateTime fim);
    Task<IEnumerable<RelatorioDesempenhoProfissionalDto>> ObterDesempenhoProfissionais(DateTime inicio, DateTime fim);
    Task<IEnumerable<RelatorioProdutosMaisVendidosDto>> ObterProdutosMaisVendidos(DateTime inicio, DateTime fim, int top);
}
