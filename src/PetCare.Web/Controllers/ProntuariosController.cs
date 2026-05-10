using Microsoft.AspNetCore.Mvc;
using PetCare.Application.DTOs.Atendimento;
using PetCare.Application.Services.Interfaces;

namespace PetCare.Web.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProntuariosController : ControllerBase
{
    private readonly IProntuarioService _prontuarioService;

    public ProntuariosController(IProntuarioService prontuarioService)
    {
        _prontuarioService = prontuarioService;
    }

    [HttpGet("{id:long}")]
    public async Task<IActionResult> ObterPorId(long id)
    {
        var prontuario = await _prontuarioService.ObterPorId(id);
        if (prontuario is null)
            return NotFound(new { mensagem = $"Prontuário com id {id} não encontrado." });

        return Ok(prontuario);
    }

    [HttpGet("agendamento/{agendamentoId:long}")]
    public async Task<IActionResult> ObterPorAgendamento(long agendamentoId)
    {
        var prontuario = await _prontuarioService.ObterPorAgendamento(agendamentoId);
        if (prontuario is null)
            return NotFound(new { mensagem = $"Prontuário para o agendamento {agendamentoId} não encontrado." });

        return Ok(prontuario);
    }

    [HttpPost]
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

    [HttpPut("{id:long}")]
    public async Task<IActionResult> Atualizar(long id, [FromBody] ProntuarioCreateDto dto)
    {
        var atualizado = await _prontuarioService.Atualizar(id, dto);
        if (!atualizado)
            return NotFound(new { mensagem = $"Prontuário com id {id} não encontrado." });

        return NoContent();
    }
}
