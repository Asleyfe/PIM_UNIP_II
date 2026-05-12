using Microsoft.AspNetCore.Mvc;
using PetCare.Application.Services.Interfaces;

namespace PetCare.Web.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RelatoriosController : ControllerBase
{
    private readonly IRelatorioService _relatorioService;

    public RelatoriosController(IRelatorioService relatorioService)
    {
        _relatorioService = relatorioService;
    }

    [HttpGet("faturamento")]
    public async Task<IActionResult> ObterFaturamento([FromQuery] DateTime inicio, [FromQuery] DateTime fim)
    {
        if (inicio == default) inicio = DateTime.UtcNow.AddMonths(-1);
        if (fim == default) fim = DateTime.UtcNow;

        var relatorio = await _relatorioService.ObterFaturamentoPeriodo(inicio, fim);
        return Ok(relatorio);
    }

    [HttpGet("desempenho-profissionais")]
    public async Task<IActionResult> ObterDesempenhoProfissionais([FromQuery] DateTime inicio, [FromQuery] DateTime fim)
    {
        if (inicio == default) inicio = DateTime.UtcNow.AddMonths(-1);
        if (fim == default) fim = DateTime.UtcNow;

        var relatorio = await _relatorioService.ObterDesempenhoProfissionais(inicio, fim);
        return Ok(relatorio);
    }

    [HttpGet("produtos-mais-vendidos")]
    public async Task<IActionResult> ObterProdutosMaisVendidos([FromQuery] DateTime inicio, [FromQuery] DateTime fim, [FromQuery] int top = 10)
    {
        if (inicio == default) inicio = DateTime.UtcNow.AddMonths(-1);
        if (fim == default) fim = DateTime.UtcNow;

        var relatorio = await _relatorioService.ObterProdutosMaisVendidos(inicio, fim, top);
        return Ok(relatorio);
    }
}
