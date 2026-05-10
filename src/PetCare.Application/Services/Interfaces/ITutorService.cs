using PetCare.Application.DTOs.Tutor;

namespace PetCare.Application.Services.Interfaces;

public interface ITutorService
{
    Task<IEnumerable<TutorResponseDto>> ListarTodos();
    Task<TutorResponseDto?> ObterPorId(long id);
    Task<IEnumerable<TutorResponseDto>> Buscar(string termo);
    Task<TutorResponseDto> Cadastrar(TutorCreateDto dto);
    Task<bool> Atualizar(long id, TutorCreateDto dto);
    Task<bool> Remover(long id);
}