using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetCare.Application.Services.Interfaces;

namespace PetCare.Web.Controllers;

[Route("Relatorios")]
[Authorize]
public class RelatoriosController : Controller
{
    private readonly IRelatorioService _relatorioService;

    public RelatoriosController(IRelatorioService relatorioService)
    {
        _relatorioService = relatorioService;
    }

    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }

    [HttpGet("/api/Relatorios/faturamento")]
    public async Task<IActionResult> ObterFaturamento([FromQuery] DateTime inicio, [FromQuery] DateTime fim)
    {
        if (inicio == default) inicio = DateTime.UtcNow.AddMonths(-1);
        if (fim == default) fim = DateTime.UtcNow;

        var relatorio = await _relatorioService.ObterFaturamentoPeriodo(inicio, fim);
        return Ok(relatorio);
    }

    [HttpGet("/api/Relatorios/desempenho-profissionais")]
    public async Task<IActionResult> ObterDesempenhoProfissionais([FromQuery] DateTime inicio, [FromQuery] DateTime fim)
    {
        if (inicio == default) inicio = DateTime.UtcNow.AddMonths(-1);
        if (fim == default) fim = DateTime.UtcNow;

        var relatorio = await _relatorioService.ObterDesempenhoProfissionais(inicio, fim);
        return Ok(relatorio);
    }

    [HttpGet("/api/Relatorios/produtos-mais-vendidos")]
    public async Task<IActionResult> ObterProdutosMaisVendidos([FromQuery] DateTime inicio, [FromQuery] DateTime fim, [FromQuery] int top = 10)
    {
        if (inicio == default) inicio = DateTime.UtcNow.AddMonths(-1);
        if (fim == default) fim = DateTime.UtcNow;

        var relatorio = await _relatorioService.ObterProdutosMaisVendidos(inicio, fim, top);
        return Ok(relatorio);
    }
}
