using Microsoft.AspNetCore.Mvc;
using PetCare.Application.DTOs.Agendamento;
using PetCare.Application.Services.Interfaces;

namespace PetCare.Web.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AgendamentosController : ControllerBase
{
    private readonly IAgendamentoService _agendamentoService;

    public AgendamentosController(IAgendamentoService agendamentoService)
    {
        _agendamentoService = agendamentoService;
    }

    [HttpGet]
    public async Task<IActionResult> Listar()
    {
        var agendamentos = await _agendamentoService.ListarTodos();
        return Ok(agendamentos);
    }

    [HttpGet("{id:long}")]
    public async Task<IActionResult> ObterPorId(long id)
    {
        var agendamento = await _agendamentoService.ObterPorId(id);
        if (agendamento is null)
            return NotFound(new { mensagem = $"Agendamento com id {id} não encontrado." });

        return Ok(agendamento);
    }

    [HttpGet("agenda-dia")]
    public async Task<IActionResult> ListarAgendaDoDia([FromQuery] string data)
    {
        if (!DateOnly.TryParse(data, out var dateOnly))
            return BadRequest(new { mensagem = "Data inválida. Use o formato AAAA-MM-DD." });

        var agenda = await _agendamentoService.ListarAgendaDoDia(dateOnly);
        return Ok(agenda);
    }

    [HttpPost]
    public async Task<IActionResult> Agendar([FromBody] AgendamentoCreateDto dto)
    {
        try
        {
            var agendamento = await _agendamentoService.Agendar(dto);
            return CreatedAtAction(nameof(ObterPorId), new { id = agendamento.Id }, agendamento);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { mensagem = ex.Message });
        }
    }

    [HttpPatch("{id:long}/cancelar")]
    public async Task<IActionResult> Cancelar(long id)
    {
        var cancelado = await _agendamentoService.Cancelar(id);
        if (!cancelado)
            return NotFound(new { mensagem = $"Agendamento com id {id} não encontrado." });

        return NoContent();
    }

    [HttpPatch("{id:long}/concluir")]
    public async Task<IActionResult> Concluir(long id)
    {
        var concluido = await _agendamentoService.Concluir(id);
        if (!concluido)
            return NotFound(new { mensagem = $"Agendamento com id {id} não encontrado." });

        return NoContent();
    }

    [HttpPatch("{id:long}/reagendar")]
    public async Task<IActionResult> Reagendar(long id, [FromBody] DateTime novaDataHora)
    {
        try
        {
            var reagendado = await _agendamentoService.Reagendar(id, novaDataHora);
            if (!reagendado)
                return NotFound(new { mensagem = $"Agendamento com id {id} não encontrado." });

            return NoContent();
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { mensagem = ex.Message });
        }
    }
}
