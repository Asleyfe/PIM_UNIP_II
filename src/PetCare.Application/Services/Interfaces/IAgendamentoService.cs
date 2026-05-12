using PetCare.Application.DTOs.Agendamento;

namespace PetCare.Application.Services.Interfaces;

public interface IAgendamentoService
{
    Task<IEnumerable<AgendamentoResponseDto>> ListarTodos();
    Task<AgendamentoResponseDto?> ObterPorId(long id);
    Task<IEnumerable<AgendamentoResponseDto>> ListarAgendaDoDia(DateOnly data);
    Task<AgendamentoResponseDto> Agendar(AgendamentoCreateDto dto);
    Task<bool> Cancelar(long id);
    Task<bool> Concluir(long id, decimal preco);
    Task<bool> Reagendar(long id, DateTime novaDataHora);
}
