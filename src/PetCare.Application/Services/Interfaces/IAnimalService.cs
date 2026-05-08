using PetCare.Application.DTOs.Animal;

namespace PetCare.Application.Services.Interfaces;

public interface IAnimalService
{
    Task<AnimalResponseDto> Cadastrar(AnimalCreateDto dto);
    Task<IEnumerable<AnimalResponseDto>> ListarTodos();
    Task<IEnumerable<AnimalResponseDto>> ObterPorTutor(long tutorId);
    Task<AnimalResponseDto?> ObterPorId(long id);
    Task<bool> Remover(long id);
}