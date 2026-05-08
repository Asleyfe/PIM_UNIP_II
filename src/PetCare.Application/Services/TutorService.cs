using PetCare.Application.DTOs.Tutor;
using PetCare.Application.Services.Interfaces;
using PetCare.Domain.Interfaces.Repositories;

namespace PetCare.Application.Services;

public class TutorService : ITutorService
{
    private readonly ITutorRepository _tutorRepository;

    public TutorService(ITutorRepository tutorRepository)
    {
        _tutorRepository = tutorRepository;
    }

    public async Task<IEnumerable<TutorResponseDto>> ListarTodos()
    {
        var tutores = await _tutorRepository.Listar();
        return tutores.Select(t => new TutorResponseDto
        {
            Id = t.Id,
            Nome = t.Nome,
            Cpf = t.Cpf,
            Telefone = t.Telefone,
            Email = t.Email,
            CreatedAt = t.CreatedAt
        });
    }

    public async Task<TutorResponseDto?> ObterPorId(long id)
    {
        var t = await _tutorRepository.ObterPorId(id);
        if (t == null) return null;

        return new TutorResponseDto
        {
            Id = t.Id,
            Nome = t.Nome,
            Cpf = t.Cpf,
            Telefone = t.Telefone,
            Email = t.Email,
            CreatedAt = t.CreatedAt
        };
    }

    public async Task<IEnumerable<TutorResponseDto>> Buscar(string termo)
    {
        var tutores = await _tutorRepository.BuscarPorTermo(termo);
        return tutores.Select(t => new TutorResponseDto
        {
            Id = t.Id,
            Nome = t.Nome,
            Cpf = t.Cpf,
            Telefone = t.Telefone,
            Email = t.Email,
            CreatedAt = t.CreatedAt
        });
    }
}