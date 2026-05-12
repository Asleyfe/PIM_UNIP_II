using Microsoft.AspNetCore.Mvc;
using PetCare.Application.DTOs.Atendimento;
using PetCare.Application.Services.Interfaces;

namespace PetCare.Web.Controllers;
[Route("Historico")]
public class HistoricosController : Controller
{
    private readonly IHistoricoService _historicoService;

    public HistoricosController(IHistoricoService historicoService)
    {
        _historicoService = historicoService;
    }

    [HttpGet("Animal/{animalId:long}")]
    public IActionResult Index(long animalId)
    {
        ViewBag.AnimalId = animalId;
        return View();
    }

    [HttpGet("/api/Historico/animal/{animalId:long}")]
    public async Task<IActionResult> ListarPorAnimal(long animalId)
    {
        var historicos = await _historicoService.ListarPorAnimal(animalId);
        return Ok(historicos);
    }

    [HttpPost("/api/Historico")]
    public async Task<IActionResult> Registrar([FromBody] HistoricoClinicoCreateDto dto)
    {
        try
        {
            var historico = await _historicoService.Registrar(dto);
            return Ok(historico);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { mensagem = ex.Message });
        }
    }

    [HttpDelete("/api/Historico/{id:long}")]
    public async Task<IActionResult> Remover(long id)
    {
        var removido = await _historicoService.Remover(id);
        if (!removido)
            return NotFound(new { mensagem = $"Registro de histórico com id {id} não encontrado." });

        return NoContent();
    }
}
