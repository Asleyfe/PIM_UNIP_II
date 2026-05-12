using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetCare.Application.DTOs.Tutor;
using PetCare.Application.Services.Interfaces;

namespace PetCare.Web.Controllers;

[Route("Tutores")]
[Authorize]
public class TutoresController : Controller
{
    private readonly ITutorService _tutorService;

    public TutoresController(ITutorService tutorService)
    {
        _tutorService = tutorService;
    }

    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }

    [HttpGet("Cadastro")]
    public IActionResult Cadastro()
    {
        return View();
    }

    [HttpGet("/api/Tutores")]
    public async Task<IActionResult> Listar()
    {
        var tutores = await _tutorService.ListarTodos();
        return Ok(tutores);
    }

    [HttpGet("/api/Tutores/{id:long}")]
    public async Task<IActionResult> ObterPorId(long id)
    {
        var tutor = await _tutorService.ObterPorId(id);
        if (tutor is null)
            return NotFound(new { mensagem = $"Tutor com id {id} não encontrado." });

        return Ok(tutor);
    }

    [HttpGet("/api/Tutores/buscar")]
    public async Task<IActionResult> Buscar([FromQuery] string termo)
    {
        if (string.IsNullOrWhiteSpace(termo))
            return BadRequest(new { mensagem = "Informe um termo para busca." });

        var tutores = await _tutorService.Buscar(termo);
        return Ok(tutores);
    }

    [HttpPost("/api/Tutores")]
    public async Task<IActionResult> Cadastrar([FromBody] TutorCreateDto dto)
    {
        try
        {
            var tutor = await _tutorService.Cadastrar(dto);
            return CreatedAtAction(nameof(ObterPorId), new { id = tutor.Id }, tutor);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { mensagem = ex.Message });
        }
    }

    [HttpPut("/api/Tutores/{id:long}")]
    public async Task<IActionResult> Atualizar(long id, [FromBody] TutorCreateDto dto)
    {
        var atualizado = await _tutorService.Atualizar(id, dto);
        if (!atualizado)
            return NotFound(new { mensagem = $"Tutor com id {id} não encontrado." });

        return NoContent();
    }

    [HttpDelete("/api/Tutores/{id:long}")]
    public async Task<IActionResult> Remover(long id)
    {
        var removido = await _tutorService.Remover(id);
        if (!removido)
            return NotFound(new { mensagem = $"Tutor com id {id} não encontrado." });

        return NoContent();
    }
}
