using Microsoft.AspNetCore.Mvc;
using PetCare.Application.Services.Interfaces;

namespace PetCare.Web.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TutoresController : ControllerBase
{
    private readonly ITutorService _tutorService;

    public TutoresController(ITutorService tutorService)
    {
        _tutorService = tutorService;
    }

    [HttpGet]
    public async Task<IActionResult> Listar()
    {
        var tutores = await _tutorService.ListarTodos();
        return Ok(tutores);
    }

    [HttpGet("{id:long}")]
    public async Task<IActionResult> ObterPorId(long id)
    {
        var tutor = await _tutorService.ObterPorId(id);
        if (tutor is null)
            return NotFound(new { mensagem = $"Tutor com id {id} não encontrado." });

        return Ok(tutor);
    }

    [HttpGet("buscar")]
    public async Task<IActionResult> Buscar([FromQuery] string termo)
    {
        if (string.IsNullOrWhiteSpace(termo))
            return BadRequest(new { mensagem = "Informe um termo para busca." });

        var tutores = await _tutorService.Buscar(termo);
        return Ok(tutores);
    }
}