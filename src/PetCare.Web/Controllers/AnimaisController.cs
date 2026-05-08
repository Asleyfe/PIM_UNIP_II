using Microsoft.AspNetCore.Mvc;
using PetCare.Application.DTOs.Animal;
using PetCare.Application.Services.Interfaces;

namespace PetCare.Web.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AnimaisController : ControllerBase
{
    private readonly IAnimalService _animalService;

    public AnimaisController(IAnimalService animalService)
    {
        _animalService = animalService;
    }

    [HttpGet]
    public async Task<IActionResult> Listar()
    {
        var animais = await _animalService.ListarTodos();
        return Ok(animais);
    }

    [HttpGet("{id:long}")]
    public async Task<IActionResult> ObterPorId(long id)
    {
        var animal = await _animalService.ObterPorId(id);
        if (animal is null)
            return NotFound(new { mensagem = $"Animal com id {id} não encontrado." });

        return Ok(animal);
    }

    [HttpGet("tutor/{tutorId:long}")]
    public async Task<IActionResult> ListarPorTutor(long tutorId)
    {
        var animais = await _animalService.ObterPorTutor(tutorId);
        return Ok(animais);
    }

    [HttpPost]
    public async Task<IActionResult> Cadastrar([FromBody] AnimalCreateDto dto)
    {
        try
        {
            var animal = await _animalService.Cadastrar(dto);
            return CreatedAtAction(nameof(ObterPorId), new { id = animal.Id }, animal);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { mensagem = ex.Message });
        }
    }

    [HttpDelete("{id:long}")]
    public async Task<IActionResult> Remover(long id)
    {
        var removido = await _animalService.Remover(id);
        if (!removido)
            return NotFound(new { mensagem = $"Animal com id {id} não encontrado." });

        return NoContent();
    }
}