using Microsoft.AspNetCore.Mvc;
using PetCare.Application.DTOs.Vendas;
using PetCare.Application.Services.Interfaces;

namespace PetCare.Web.Controllers;

[Route("Vendas")]
public class VendasController : Controller
{
    private readonly IVendaService _vendaService;

    public VendasController(IVendaService vendaService)
    {
        _vendaService = vendaService;
    }

    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }

    [HttpGet("/api/Vendas")]
    public async Task<IActionResult> Listar()
    {
        var vendas = await _vendaService.ListarTodas();
        return Ok(vendas);
    }

    [HttpGet("/api/Vendas/{id:long}")]
    public async Task<IActionResult> ObterPorId(long id)
    {
        var venda = await _vendaService.ObterPorId(id);
        if (venda is null)
            return NotFound(new { mensagem = $"Venda com id {id} não encontrada." });

        return Ok(venda);
    }

    [HttpGet("/api/Vendas/periodo")]
    public async Task<IActionResult> ListarPorPeriodo([FromQuery] DateTime inicio, [FromQuery] DateTime fim)
    {
        var vendas = await _vendaService.ListarPorPeriodo(inicio, fim);
        return Ok(vendas);
    }

    [HttpPost("/api/Vendas")]
    public async Task<IActionResult> RealizarVenda([FromBody] VendaCreateDto dto)
    {
        try
        {
            var venda = await _vendaService.RealizarVenda(dto);
            return Created($"/api/Vendas/{venda.Id}", venda);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { mensagem = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return UnprocessableEntity(new { mensagem = ex.Message });
        }
    }
}
