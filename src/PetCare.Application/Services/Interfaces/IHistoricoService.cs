using PetCare.Application.DTOs.Atendimento;

namespace PetCare.Application.Services.Interfaces;

public interface IHistoricoService
{
    Task<IEnumerable<HistoricoClinicoResponseDto>> ListarPorAnimal(long animalId);
    Task<HistoricoClinicoResponseDto> Registrar(HistoricoClinicoCreateDto dto);
    Task<bool> Remover(long id);
}
