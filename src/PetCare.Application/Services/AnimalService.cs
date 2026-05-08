using PetCare.Application.DTOs.Animal;
using PetCare.Application.Services.Interfaces;
using PetCare.Domain.Entities.Animais;
using PetCare.Domain.Enums;
using PetCare.Domain.Interfaces.Repositories;

namespace PetCare.Application.Services;

public class AnimalService : IAnimalService
{
    private readonly IAnimalRepository _animalRepository;

    public AnimalService(IAnimalRepository animalRepository)
    {
        _animalRepository = animalRepository;
    }

    public async Task<AnimalResponseDto> Cadastrar(AnimalCreateDto dto)
    {
        if (!Enum.TryParse<Sexo>(dto.Sexo, true, out var sexo))
            throw new ArgumentException("Sexo inválido. Use 'M' ou 'F'.");

        var animal = new Animal(
            dto.Nome,
            dto.DataNascimento,
            dto.Peso,
            sexo,
            dto.TutorId,
            dto.RacaId
        );

        var animalInserido = await _animalRepository.Inserir(animal);

        return MapToDto(animalInserido);
    }

    public async Task<IEnumerable<AnimalResponseDto>> ListarTodos()
    {
        var animais = await _animalRepository.Listar();
        return animais.Select(MapToDto);
    }

    public async Task<IEnumerable<AnimalResponseDto>> ObterPorTutor(long tutorId)
    {
        var animais = await _animalRepository.ObterPorTutorId(tutorId);
        return animais.Select(MapToDto);
    }

    public async Task<AnimalResponseDto?> ObterPorId(long id)
    {
        var animal = await _animalRepository.ObterPorId(id);
        return animal == null ? null : MapToDto(animal);
    }

    public async Task<bool> Remover(long id)
    {
        return await _animalRepository.Remover(id);
    }

    private static AnimalResponseDto MapToDto(Animal animal)
    {
        return new AnimalResponseDto
        {
            Id = animal.Id,
            Nome = animal.Nome,
            DataNascimento = animal.DataNascimento,
            Peso = animal.Peso,
            Sexo = animal.Sexo.ToString(),
            TutorId = animal.TutorId,
            RacaId = animal.RacaId,
            IdadeAnos = animal.IdadeAnos,
            EhFilhote = animal.EhFilhote
        };
    }
}