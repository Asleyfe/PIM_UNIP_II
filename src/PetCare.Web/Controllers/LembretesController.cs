using Microsoft.AspNetCore.Mvc;
using PetCare.Application.DTOs.Comunicacao;
using PetCare.Application.Services.Interfaces;

namespace PetCare.Web.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LembretesController : ControllerBase
{
    private readonly ILembreteService _lembreteService;

    public LembretesController(ILembreteService lembreteService)
    {
        _lembreteService = lembreteService;
    }

    [HttpGet]
    public async Task<IActionResult> Listar()
    {
        var lembretes = await _lembreteService.ListarTodos();
        return Ok(lembretes);
    }

    [HttpGet("tutor/{tutorId:long}")]
    public async Task<IActionResult> ListarPorTutor(long tutorId)
    {
        var lembretes = await _lembreteService.ListarPorTutor(tutorId);
        return Ok(lembretes);
    }

    [HttpPost]
    public async Task<IActionResult> RegistrarEnvio([FromBody] LembreteCreateDto dto)
    {
        try
        {
            var lembrete = await _lembreteService.RegistrarEnvio(dto);
            return Ok(lembrete);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { mensagem = ex.Message });
        }
    }

    [HttpPatch("{id:long}/status")]
    public async Task<IActionResult> AtualizarStatus(long id, [FromQuery] string status)
    {
        try
        {
            var atualizado = await _lembreteService.AtualizarStatus(id, status);
            if (!atualizado)
                return NotFound(new { mensagem = $"Lembrete com id {id} não encontrado." });

            return NoContent();
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { mensagem = ex.Message });
        }
    }
}
