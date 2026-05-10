using PetCare.Application.DTOs.Tutor;
using PetCare.Application.Services.Interfaces;
using PetCare.Domain.Entities.Pessoas;
using PetCare.Domain.Interfaces.Repositories;
using PetCare.Domain.ValueObjects;

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
        return tutores.Select(MapToDto);
    }

    public async Task<TutorResponseDto?> ObterPorId(long id)
    {
        var t = await _tutorRepository.ObterPorId(id);
        return t == null ? null : MapToDto(t);
    }

    public async Task<IEnumerable<TutorResponseDto>> Buscar(string termo)
    {
        var tutores = await _tutorRepository.BuscarPorTermo(termo);
        return tutores.Select(MapToDto);
    }

    public async Task<TutorResponseDto> Cadastrar(TutorCreateDto dto)
    {
        if (await _tutorRepository.ExistePorCpf(dto.Cpf))
            throw new ArgumentException($"Já existe um tutor cadastrado com o CPF {dto.Cpf}.");

        var endereco = new Endereco(dto.Rua, dto.Numero, dto.Bairro, dto.Cidade, dto.Estado);
        var tutor = new Tutor(dto.Nome, dto.Cpf, dto.Telefone, dto.Email, endereco);

        var tutorInserido = await _tutorRepository.Inserir(tutor);
        return MapToDto(tutorInserido);
    }

    public async Task<bool> Atualizar(long id, TutorCreateDto dto)
    {
        var tutorExistente = await _tutorRepository.ObterPorId(id);
        if (tutorExistente == null) return false;

        var endereco = new Endereco(dto.Rua, dto.Numero, dto.Bairro, dto.Cidade, dto.Estado);
        
        // Usando reflexão para atualizar campos, já que Tutor e Endereco são "imutáveis" via private setters no domínio
        // Mas o TutorRepository.Atualizar espera um objeto Tutor.
        // Como o domínio é focado em PIM e pode ser rígido, vou instanciar um novo ou atualizar via métodos se existirem.
        // Olhando Tutor.cs...
        
        var tutorAtualizado = new Tutor(dto.Nome, dto.Cpf, dto.Telefone, dto.Email, endereco);
        tutorAtualizado.SetId(id);

        return await _tutorRepository.Atualizar(tutorAtualizado);
    }

    public async Task<bool> Remover(long id)
    {
        return await _tutorRepository.Remover(id);
    }

    private static TutorResponseDto MapToDto(Tutor t)
    {
        return new TutorResponseDto
        {
            Id = t.Id,
            Nome = t.Nome,
            Cpf = t.Cpf,
            Telefone = t.Telefone,
            Email = t.Email,
            Rua = t.Endereco.Rua,
            Numero = t.Endereco.Numero,
            Bairro = t.Endereco.Bairro,
            Cidade = t.Endereco.Cidade,
            Estado = t.Endereco.Estado,
            CreatedAt = t.CreatedAt
        };
    }
}
