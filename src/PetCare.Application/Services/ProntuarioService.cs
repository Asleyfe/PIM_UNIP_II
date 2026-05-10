using PetCare.Application.DTOs.Atendimento;
using PetCare.Application.Services.Interfaces;
using PetCare.Domain.Entities.Atendimento;
using PetCare.Domain.Enums;
using PetCare.Domain.Interfaces.Repositories;

namespace PetCare.Application.Services;

public class ProntuarioService : IProntuarioService
{
    private readonly IProntuarioRepository _prontuarioRepository;
    private readonly IAgendamentoRepository _agendamentoRepository;

    public ProntuarioService(
        IProntuarioRepository prontuarioRepository,
        IAgendamentoRepository agendamentoRepository)
    {
        _prontuarioRepository = prontuarioRepository;
        _agendamentoRepository = agendamentoRepository;
    }

    public async Task<ProntuarioResponseDto?> ObterPorId(long id)
    {
        var p = await _prontuarioRepository.ObterPorId(id);
        return p == null ? null : MapToDto(p);
    }

    public async Task<ProntuarioResponseDto?> ObterPorAgendamento(long agendamentoId)
    {
        var p = await _prontuarioRepository.ObterPorAgendamento(agendamentoId);
        return p == null ? null : MapToDto(p);
    }

    public async Task<ProntuarioResponseDto> Registrar(ProntuarioCreateDto dto)
    {
        var agendamento = await _agendamentoRepository.ObterPorId(dto.AgendamentoId);
        if (agendamento == null)
            throw new ArgumentException($"Agendamento com id {dto.AgendamentoId} não encontrado.");

        if (agendamento.Status != StatusAgendamento.CONCLUIDO)
            throw new InvalidOperationException("Só é possível registrar prontuário para agendamentos concluídos.");

        var existente = await _prontuarioRepository.ObterPorAgendamento(dto.AgendamentoId);
        if (existente != null)
            throw new InvalidOperationException("Já existe um prontuário para este agendamento.");

        var prontuario = new Prontuario(
            dto.AgendamentoId,
            dto.QueixaPrincipal,
            dto.RelatoTutor,
            dto.ExameFisico,
            dto.Diagnostico,
            dto.Prescricao,
            dto.Observacoes
        );

        var inserido = await _prontuarioRepository.Inserir(prontuario);
        return MapToDto(inserido);
    }

    public async Task<bool> Atualizar(long id, ProntuarioCreateDto dto)
    {
        var existente = await _prontuarioRepository.ObterPorId(id);
        if (existente == null) return false;

        existente.Atualizar(
            dto.QueixaPrincipal,
            dto.RelatoTutor,
            dto.ExameFisico,
            dto.Diagnostico,
            dto.Prescricao,
            dto.Observacoes
        );

        return await _prontuarioRepository.Atualizar(existente);
    }

    private static ProntuarioResponseDto MapToDto(Prontuario p)
    {
        return new ProntuarioResponseDto
        {
            Id = p.Id,
            AgendamentoId = p.AgendamentoId,
            QueixaPrincipal = p.QueixaPrincipal,
            RelatoTutor = p.RelatoTutor,
            ExameFisico = p.ExameFisico,
            Diagnostico = p.Diagnostico,
            Prescricao = p.Prescricao,
            Observacoes = p.Observacoes,
            DataRegistro = p.DataRegistro,
            CreatedAt = p.CreatedAt
        };
    }
}
