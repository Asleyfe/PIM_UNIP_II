using Microsoft.AspNetCore.Mvc;
using PetCare.Domain.Exceptions;
using PetCare.Domain.Interfaces.Repositories;

namespace PetCare.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TutoresController : ControllerBase
{
    private readonly ITutorRepository _tutorRepository;

    public TutoresController(ITutorRepository tutorRepository)
    {
        _tutorRepository = tutorRepository;
    }

    [HttpGet]
    public async Task<IActionResult> Listar()
    {
        var tutores = await _tutorRepository.Listar();
        return Ok(tutores);
    }

    [HttpGet("{id:long}")]
    public async Task<IActionResult> ObterPorId(long id)
    {
        var tutor = await _tutorRepository.ObterPorId(id);
        if (tutor is null)
            return NotFound(new { mensagem = $"Tutor com id {id} não encontrado." });

        return Ok(tutor);
    }

    [HttpGet("buscar")]
    public async Task<IActionResult> Buscar([FromQuery] string termo)
    {
        if (string.IsNullOrWhiteSpace(termo))
            return BadRequest(new { mensagem = "Informe um termo para busca." });

        var tutores = await _tutorRepository.BuscarPorTermo(termo);
        return Ok(tutores);
    }
}
