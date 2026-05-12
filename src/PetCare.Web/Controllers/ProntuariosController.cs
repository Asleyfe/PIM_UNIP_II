using Microsoft.AspNetCore.Mvc;
using PetCare.Application.DTOs.Atendimento;
using PetCare.Application.Services.Interfaces;

namespace PetCare.Web.Controllers;

[Route("Prontuarios")]
public class ProntuariosController : Controller
{
    private readonly IProntuarioService _prontuarioService;

    public ProntuariosController(IProntuarioService prontuarioService)
    {
        _prontuarioService = prontuarioService;
    }

    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }

    [HttpGet("Registro")]
    public IActionResult Registro()
    {
        return View();
    }

    [HttpGet("/api/Prontuarios/{id:long}")]
    public async Task<IActionResult> ObterPorId(long id)
    {
        var prontuario = await _prontuarioService.ObterPorId(id);
        if (prontuario is null)
            return NotFound(new { mensagem = $"Prontuário com id {id} não encontrado." });

        return Ok(prontuario);
    }

    [HttpGet("/api/Prontuarios/agendamento/{agendamentoId:long}")]
    public async Task<IActionResult> ObterPorAgendamento(long agendamentoId)
    {
        var prontuario = await _prontuarioService.ObterPorAgendamento(agendamentoId);
        if (prontuario is null)
            return NotFound(new { mensagem = $"Prontuário para o agendamento {agendamentoId} não encontrado." });

        return Ok(prontuario);
    }

    [HttpPost("/api/Prontuarios")]
    public async Task<IActionResult> Registrar([FromBody] ProntuarioCreateDto dto)
    {
        try
        {
            var prontuario = await _prontuarioService.Registrar(dto);
            return CreatedAtAction(nameof(ObterPorId), new { id = prontuario.Id }, prontuario);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { mensagem = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { mensagem = ex.Message });
        }
    }

    [HttpPut("/api/Prontuarios/{id:long}")]
    public async Task<IActionResult> Atualizar(long id, [FromBody] ProntuarioCreateDto dto)
    {
        var atualizado = await _prontuarioService.Atualizar(id, dto);
        if (!atualizado)
            return NotFound(new { mensagem = $"Prontuário com id {id} não encontrado." });

        return NoContent();
    }
}
