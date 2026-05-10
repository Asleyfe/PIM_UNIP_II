using PetCare.Application.DTOs.Veterinario;

namespace PetCare.Application.Services.Interfaces;

public interface IVeterinarioService
{
    Task<IEnumerable<VeterinarioResponseDto>> ListarTodos();
    Task<VeterinarioResponseDto?> ObterPorId(long id);
    Task<VeterinarioResponseDto> Cadastrar(VeterinarioCreateDto dto);
    Task<bool> Atualizar(long id, VeterinarioCreateDto dto);
    Task<bool> Remover(long id);
}
