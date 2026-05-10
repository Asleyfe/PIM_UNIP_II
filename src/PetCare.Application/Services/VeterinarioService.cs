using PetCare.Application.DTOs.Veterinario;
using PetCare.Application.Services.Interfaces;
using PetCare.Domain.Entities.Pessoas;
using PetCare.Domain.Interfaces.Repositories;

namespace PetCare.Application.Services;

public class VeterinarioService : IVeterinarioService
{
    private readonly IVeterinarioRepository _veterinarioRepository;

    public VeterinarioService(IVeterinarioRepository veterinarioRepository)
    {
        _veterinarioRepository = veterinarioRepository;
    }

    public async Task<IEnumerable<VeterinarioResponseDto>> ListarTodos()
    {
        var veterinarios = await _veterinarioRepository.Listar();
        return veterinarios.Select(MapToDto);
    }

    public async Task<VeterinarioResponseDto?> ObterPorId(long id)
    {
        var v = await _veterinarioRepository.ObterPorId(id);
        return v == null ? null : MapToDto(v);
    }

    public async Task<VeterinarioResponseDto> Cadastrar(VeterinarioCreateDto dto)
    {
        if (await _veterinarioRepository.ExistePorCrmv(dto.Crmv))
            throw new ArgumentException($"Já existe um veterinário cadastrado com o CRMV {dto.Crmv}.");

        var veterinario = new Veterinario(dto.Nome, dto.Crmv, dto.Telefone, dto.Email);
        var inserido = await _veterinarioRepository.Inserir(veterinario);
        
        return MapToDto(inserido);
    }

    public async Task<bool> Atualizar(long id, VeterinarioCreateDto dto)
    {
        var existente = await _veterinarioRepository.ObterPorId(id);
        if (existente == null) return false;

        // CRMV e Nome não podem ser alterados segundo o domínio do Veterinario.cs
        // Mas o repositório permite atualizar Nome, Telefone e Email.
        // Vamos seguir a lógica do domínio para consistência.
        existente.AtualizarContato(dto.Telefone, dto.Email);
        
        // Se precisar atualizar o nome (conforme permite o Repository.Atualizar):
        // Como o domínio não tem método SetNome, mas o Repository aceita,
        // vamos respeitar o domínio onde o nome é fixo após cadastro, ou usar reflexão se for necessário.
        // Vou manter a integridade do domínio.
        
        return await _veterinarioRepository.Atualizar(existente);
    }

    public async Task<bool> Remover(long id)
    {
        return await _veterinarioRepository.Remover(id);
    }

    private static VeterinarioResponseDto MapToDto(Veterinario v)
    {
        return new VeterinarioResponseDto
        {
            Id = v.Id,
            Nome = v.Nome,
            Crmv = v.Crmv,
            Telefone = v.Telefone,
            Email = v.Email,
            CreatedAt = v.CreatedAt
        };
    }
}
