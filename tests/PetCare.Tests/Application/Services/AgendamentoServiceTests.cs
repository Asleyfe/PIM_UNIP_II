using FluentAssertions;
using Moq;
using PetCare.Application.DTOs.Agendamento;
using PetCare.Application.Services;
using PetCare.Domain.Entities.Animais;
using PetCare.Domain.Entities.Pessoas;
using PetCare.Domain.Interfaces.Repositories;
using PetCare.Domain.Enums;
using PetCare.Domain.ValueObjects;

namespace PetCare.Tests.Application.Services;

public class AgendamentoServiceTests
{
    private readonly Mock<IAgendamentoRepository> _agendamentoRepoMock;
    private readonly Mock<ITutorRepository> _tutorRepoMock;
    private readonly Mock<IAnimalRepository> _animalRepoMock;
    private readonly Mock<IVeterinarioRepository> _veterinarioRepoMock;
    private readonly AgendamentoService _service;

    public AgendamentoServiceTests()
    {
        _agendamentoRepoMock = new Mock<IAgendamentoRepository>();
        _tutorRepoMock = new Mock<ITutorRepository>();
        _animalRepoMock = new Mock<IAnimalRepository>();
        _veterinarioRepoMock = new Mock<IVeterinarioRepository>();

        _service = new AgendamentoService(
            _agendamentoRepoMock.Object,
            _tutorRepoMock.Object,
            _animalRepoMock.Object,
            _veterinarioRepoMock.Object
        );
    }

    [Fact]
    public async Task Agendar_DeveFalhar_QuandoTutorNaoExiste()
    {
        // Arrange
        var dto = new AgendamentoCreateDto { TutorId = 99 };
        _tutorRepoMock.Setup(r => r.ObterPorId(dto.TutorId)).ReturnsAsync((Tutor?)null);

        // Act
        Func<Task> act = async () => await _service.Agendar(dto);

        // Assert
        await act.Should().ThrowAsync<ArgumentException>()
            .WithMessage("*Tutor*não encontrado*");
    }

    [Fact]
    public async Task Agendar_DeveFalhar_QuandoAnimalNaoPertenceAoTutor()
    {
        // Arrange
        var dto = new AgendamentoCreateDto { TutorId = 1, AnimalId = 2 };
        var endereco = new Endereco("Rua", "123", "Bairro", "Cidade", "GO");
        var tutor = new Tutor("Tutor", "12345678901", "123", "a@a.com", endereco);
        var animal = new Animal("Pet", DateOnly.FromDateTime(DateTime.UtcNow.AddYears(-1)), 5, Sexo.Macho, 99, 1); // TutorId 99

        _tutorRepoMock.Setup(r => r.ObterPorId(dto.TutorId)).ReturnsAsync(tutor);
        _animalRepoMock.Setup(r => r.ObterPorId(dto.AnimalId)).ReturnsAsync(animal);

        // Act
        Func<Task> act = async () => await _service.Agendar(dto);

        // Assert
        await act.Should().ThrowAsync<ArgumentException>()
            .WithMessage("*não pertence ao tutor*");
    }

    [Fact]
    public async Task Agendar_DeveFalhar_QuandoHaConflitoDeHorario()
    {
        // Arrange
        var dataHora = DateTime.UtcNow.AddDays(1);
        var dto = new AgendamentoCreateDto 
        { 
            TutorId = 1, 
            AnimalId = 2, 
            VeterinarioId = 3, 
            DataHoraConsulta = dataHora 
        };

        var endereco = new Endereco("Rua", "123", "Bairro", "Cidade", "GO");
        var tutor = new Tutor("Tutor", "12345678901", "123", "a@a.com", endereco);
        var animal = new Animal("Pet", DateOnly.FromDateTime(DateTime.UtcNow.AddYears(-1)), 5, Sexo.Macho, 1, 1);
        var vet = new Veterinario("Vet", "12345/GO");

        _tutorRepoMock.Setup(r => r.ObterPorId(dto.TutorId)).ReturnsAsync(tutor);
        _animalRepoMock.Setup(r => r.ObterPorId(dto.AnimalId)).ReturnsAsync(animal);
        _veterinarioRepoMock.Setup(r => r.ObterPorId(dto.VeterinarioId)).ReturnsAsync(vet);
        
        // Simula conflito
        _agendamentoRepoMock.Setup(r => r.ExisteConflito(dto.VeterinarioId, dto.DataHoraConsulta))
            .ReturnsAsync(true);

        // Act
        Func<Task> act = async () => await _service.Agendar(dto);

        // Assert
        await act.Should().ThrowAsync<ArgumentException>()
            .WithMessage("*Já existe um agendamento*");
    }
}
