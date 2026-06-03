using PetCare.Application.DTOs.Atendimento;

namespace PetCare.Application.Services.Interfaces;

public interface IProntuarioService
{
    Task<IEnumerable<ProntuarioResponseDto>> ListarTodos();
    Task<ProntuarioResponseDto?> ObterPorId(long id);
    Task<ProntuarioResponseDto?> ObterPorAgendamento(long agendamentoId);
    Task<ProntuarioResponseDto> Registrar(ProntuarioCreateDto dto);
    Task<bool> Atualizar(long id, ProntuarioCreateDto dto);
}
