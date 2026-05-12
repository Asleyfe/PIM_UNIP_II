using PetCare.Application.DTOs.Comunicacao;
using PetCare.Application.Services.Interfaces;
using PetCare.Domain.Entities.Comunicacao;
using PetCare.Domain.Enums;
using PetCare.Domain.Interfaces.Repositories;

namespace PetCare.Application.Services;

public class LembreteService : ILembreteService
{
    private readonly ILembreteRepository _lembreteRepository;

    public LembreteService(ILembreteRepository lembreteRepository)
    {
        _lembreteRepository = lembreteRepository;
    }

    public async Task<IEnumerable<LembreteResponseDto>> ListarTodos()
    {
        var lembretes = await _lembreteRepository.Listar();
        return lembretes.Select(MapToDto);
    }

    public async Task<IEnumerable<LembreteResponseDto>> ListarPorTutor(long tutorId)
    {
        var lembretes = await _lembreteRepository.ListarPorTutor(tutorId);
        return lembretes.Select(MapToDto);
    }

    public async Task<LembreteResponseDto> RegistrarEnvio(LembreteCreateDto dto)
    {
        if (!Enum.TryParse<TipoLembrete>(dto.Tipo, true, out var tipo))
            throw new ArgumentException("Tipo de lembrete inválido.");

        if (!Enum.TryParse<MeioEnvio>(dto.MeioEnvio, true, out var meio))
            throw new ArgumentException("Meio de envio inválido.");

        var lembrete = new LembreteEnviado(
            dto.TutorId,
            dto.AnimalId,
            tipo,
            meio,
            StatusEnvio.PENDENTE, // Inicia como pendente
            dto.Mensagem,
            dto.AgendamentoId
        );

        // Aqui em uma implementação real, chamaria o serviço de disparo (WhatsApp/Email)
        // Por enquanto, apenas registramos no banco.
        
        var inserido = await _lembreteRepository.Inserir(lembrete);
        return MapToDto(inserido);
    }

    public async Task<bool> AtualizarStatus(long id, string status)
    {
        var lembrete = await _lembreteRepository.ObterPorId(id);
        if (lembrete == null) return false;

        if (!Enum.TryParse<StatusEnvio>(status, true, out var statusEnum))
            throw new ArgumentException("Status de envio inválido.");

        lembrete.AtualizarStatus(statusEnum);
        
        return await _lembreteRepository.Atualizar(lembrete);
    }

    private static LembreteResponseDto MapToDto(LembreteEnviado l)
    {
        return new LembreteResponseDto
        {
            Id = l.Id,
            TutorId = l.TutorId,
            AnimalId = l.AnimalId,
            AgendamentoId = l.AgendamentoId,
            Tipo = l.Tipo.ToString(),
            MeioEnvio = l.MeioEnvio.ToString(),
            StatusEnvio = l.StatusEnvio.ToString(),
            DataEnvio = l.DataEnvio,
            Mensagem = l.Mensagem,
            CreatedAt = l.CreatedAt
        };
    }
}
