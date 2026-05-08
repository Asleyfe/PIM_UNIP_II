using PetCare.Application.DTOs.Tutor;

namespace PetCare.Application.Services.Interfaces;

public interface ITutorService
{
    Task<IEnumerable<TutorResponseDto>> ListarTodos();
    Task<TutorResponseDto?> ObterPorId(long id);
    Task<IEnumerable<TutorResponseDto>> Buscar(string termo);
}