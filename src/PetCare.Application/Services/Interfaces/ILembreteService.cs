using PetCare.Application.DTOs.Comunicacao;

namespace PetCare.Application.Services.Interfaces;

public interface ILembreteService
{
    Task<IEnumerable<LembreteResponseDto>> ListarTodos();
    Task<IEnumerable<LembreteResponseDto>> ListarPorTutor(long tutorId);
    Task<LembreteResponseDto> RegistrarEnvio(LembreteCreateDto dto);
    Task<bool> AtualizarStatus(long id, string status);
}
