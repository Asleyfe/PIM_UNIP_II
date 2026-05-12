using PetCare.Application.DTOs.Atendimento;
using PetCare.Application.Services.Interfaces;
using PetCare.Domain.Entities.Atendimento;
using PetCare.Domain.Interfaces.Repositories;

namespace PetCare.Application.Services;

public class HistoricoService : IHistoricoService
{
    private readonly IHistoricoRepository _historicoRepository;

    public HistoricoService(IHistoricoRepository historicoRepository)
    {
        _historicoRepository = historicoRepository;
    }

    public async Task<IEnumerable<HistoricoClinicoResponseDto>> ListarPorAnimal(long animalId)
    {
        var historicos = await _historicoRepository.ListarPorAnimal(animalId);
        return historicos.Select(MapToDto);
    }

    public async Task<HistoricoClinicoResponseDto> Registrar(HistoricoClinicoCreateDto dto)
    {
        var historico = new HistoricoClinico(
            dto.AnimalId,
            dto.VeterinarioId,
            dto.Descricao
        );

        var inserido = await _historicoRepository.Inserir(historico);
        return MapToDto(inserido);
    }

    public async Task<bool> Remover(long id)
    {
        return await _historicoRepository.Remover(id);
    }

    private static HistoricoClinicoResponseDto MapToDto(HistoricoClinico h)
    {
        return new HistoricoClinicoResponseDto
        {
            Id = h.Id,
            AnimalId = h.AnimalId,
            VeterinarioId = h.VeterinarioId,
            Descricao = h.Descricao,
            DataRegistro = h.DataRegistro,
            CreatedAt = h.CreatedAt
        };
    }
}
