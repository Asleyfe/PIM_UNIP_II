using Microsoft.AspNetCore.Mvc;
using PetCare.Application.DTOs.Veterinario;
using PetCare.Application.Services.Interfaces;

namespace PetCare.Web.Controllers;

[ApiController]
[Route("api/[controller]")]
public class VeterinariosController : ControllerBase
{
    private readonly IVeterinarioService _veterinarioService;

    public VeterinariosController(IVeterinarioService veterinarioService)
    {
        _veterinarioService = veterinarioService;
    }

    [HttpGet]
    public async Task<IActionResult> Listar()
    {
        var veterinarios = await _veterinarioService.ListarTodos();
        return Ok(veterinarios);
    }

    [HttpGet("{id:long}")]
    public async Task<IActionResult> ObterPorId(long id)
    {
        var veterinario = await _veterinarioService.ObterPorId(id);
        if (veterinario is null)
            return NotFound(new { mensagem = $"Veterinário com id {id} não encontrado." });

        return Ok(veterinario);
    }

    [HttpPost]
    public async Task<IActionResult> Cadastrar([FromBody] VeterinarioCreateDto dto)
    {
        try
        {
            var veterinario = await _veterinarioService.Cadastrar(dto);
            return CreatedAtAction(nameof(ObterPorId), new { id = veterinario.Id }, veterinario);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { mensagem = ex.Message });
        }
    }

    [HttpPut("{id:long}")]
    public async Task<IActionResult> Atualizar(long id, [FromBody] VeterinarioCreateDto dto)
    {
        var atualizado = await _veterinarioService.Atualizar(id, dto);
        if (!atualizado)
            return NotFound(new { mensagem = $"Veterinário com id {id} não encontrado." });

        return NoContent();
    }

    [HttpDelete("{id:long}")]
    public async Task<IActionResult> Remover(long id)
    {
        var removido = await _veterinarioService.Remover(id);
        if (!removido)
            return NotFound(new { mensagem = $"Veterinário com id {id} não encontrado." });

        return NoContent();
    }
}
