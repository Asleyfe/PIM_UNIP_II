using PetCare.Application.DTOs.Agendamento;
using PetCare.Application.Services.Interfaces;
using PetCare.Domain.Entities.Atendimento;
using PetCare.Domain.Interfaces.Repositories;

namespace PetCare.Application.Services;

public class AgendamentoService : IAgendamentoService
{
    private readonly IAgendamentoRepository _agendamentoRepository;
    private readonly ITutorRepository _tutorRepository;
    private readonly IAnimalRepository _animalRepository;
    private readonly IVeterinarioRepository _veterinarioRepository;

    public AgendamentoService(
        IAgendamentoRepository agendamentoRepository,
        ITutorRepository tutorRepository,
        IAnimalRepository animalRepository,
        IVeterinarioRepository veterinarioRepository)
    {
        _agendamentoRepository = agendamentoRepository;
        _tutorRepository = tutorRepository;
        _animalRepository = animalRepository;
        _veterinarioRepository = veterinarioRepository;
    }

    public async Task<IEnumerable<AgendamentoResponseDto>> ListarTodos()
    {
        var agendamentos = await _agendamentoRepository.Listar();
        return await Task.WhenAll(agendamentos.Select(MapToDto));
    }

    public async Task<AgendamentoResponseDto?> ObterPorId(long id)
    {
        var agendamento = await _agendamentoRepository.ObterPorId(id);
        return agendamento == null ? null : await MapToDto(agendamento);
    }

    public async Task<IEnumerable<AgendamentoResponseDto>> ListarAgendaDoDia(DateOnly data)
    {
        var agendamentos = await _agendamentoRepository.ListarAgendaDoDia(data);
        return await Task.WhenAll(agendamentos.Select(MapToDto));
    }

    public async Task<AgendamentoResponseDto> Agendar(AgendamentoCreateDto dto)
    {
        // Valida se as entidades relacionadas existem
        if (await _tutorRepository.ObterPorId(dto.TutorId) == null)
            throw new ArgumentException($"Tutor com id {dto.TutorId} não encontrado.");

        var animal = await _animalRepository.ObterPorId(dto.AnimalId);
        if (animal == null)
            throw new ArgumentException($"Animal com id {dto.AnimalId} não encontrado.");

        if (animal.TutorId != dto.TutorId)
            throw new ArgumentException("O animal informado não pertence ao tutor informado.");

        if (await _veterinarioRepository.ObterPorId(dto.VeterinarioId) == null)
            throw new ArgumentException($"Veterinário com id {dto.VeterinarioId} não encontrado.");

        // Valida conflito de horário
        if (await _agendamentoRepository.ExisteConflito(dto.VeterinarioId, dto.DataHoraConsulta))
            throw new ArgumentException("Já existe um agendamento para este veterinário neste horário.");

        var agendamento = new Agendamento(
            dto.TutorId,
            dto.AnimalId,
            dto.VeterinarioId,
            dto.DataHoraConsulta,
            dto.Observacao
        );

        var inserido = await _agendamentoRepository.Inserir(agendamento);
        return await MapToDto(inserido);
    }

    public async Task<bool> Cancelar(long id)
    {
        var agendamento = await _agendamentoRepository.ObterPorId(id);
        if (agendamento == null) return false;

        agendamento.Cancelar();
        return await _agendamentoRepository.Atualizar(agendamento);
    }

    public async Task<bool> Concluir(long id, decimal preco)
    {
        var agendamento = await _agendamentoRepository.ObterPorId(id);
        if (agendamento == null) return false;

        agendamento.Concluir(preco);
        return await _agendamentoRepository.Atualizar(agendamento);
    }

    public async Task<bool> Reagendar(long id, DateTime novaDataHora)
    {
        var agendamento = await _agendamentoRepository.ObterPorId(id);
        if (agendamento == null) return false;

        // Valida conflito para o novo horário
        if (await _agendamentoRepository.ExisteConflito(agendamento.VeterinarioId, novaDataHora))
            throw new ArgumentException("Já existe um agendamento para este veterinário no novo horário informado.");

        agendamento.Reagendar(novaDataHora);
        return await _agendamentoRepository.Atualizar(agendamento);
    }

    private async Task<AgendamentoResponseDto> MapToDto(Agendamento a)
    {
        var tutor = await _tutorRepository.ObterPorId(a.TutorId);
        var animal = await _animalRepository.ObterPorId(a.AnimalId);
        var veterinario = await _veterinarioRepository.ObterPorId(a.VeterinarioId);

        return new AgendamentoResponseDto
        {
            Id = a.Id,
            TutorId = a.TutorId,
            TutorNome = tutor?.Nome ?? "N/A",
            AnimalId = a.AnimalId,
            AnimalNome = animal?.Nome ?? "N/A",
            VeterinarioId = a.VeterinarioId,
            VeterinarioNome = veterinario?.Nome ?? "N/A",
            DataHoraConsulta = a.DataHoraConsulta,
            Preco = a.Preco,
            Status = a.Status.ToString(),
            Observacao = a.Observacao,
            CreatedAt = a.CreatedAt
        };
    }
}
