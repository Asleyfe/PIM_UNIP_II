using FluentAssertions;
using PetCare.Domain.Entities.Atendimento;
using PetCare.Domain.Exceptions;
using Xunit;

namespace PetCare.Tests.Domain.Entities.Atendimento;

public class ProntuarioTests
{
    [Fact(DisplayName = "Deve criar um prontuário válido")]
    public void DeveCriarProntuarioValido()
    {
        // Arrange & Act
        var prontuario = new Prontuario(1, "Vômitos e letargia", "Comeu lixo", "Desidratação leve", "Gastrite", "Soro e antiemético");

        // Assert
        prontuario.AgendamentoId.Should().Be(1);
        prontuario.QueixaPrincipal.Should().Be("Vômitos e letargia");
        prontuario.Diagnostico.Should().Be("Gastrite");
        prontuario.DataRegistro.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
    }

    [Fact(DisplayName = "Deve lançar exceção ao criar prontuário sem queixa principal")]
    public void DeveLancarExcecaoSemQueixa()
    {
        var act = () => new Prontuario(1, "");
        act.Should().Throw<DominioException>().WithMessage("A queixa principal é obrigatória.");
    }

    [Fact(DisplayName = "Deve permitir atualizar prontuário")]
    public void DevePermitirAtualizar()
    {
        // Arrange
        var prontuario = new Prontuario(1, "Queixa inicial");

        // Act
        prontuario.Atualizar("Queixa atualizada", "Relato", "Exame", "Diagnostico", "Prescricao", "Obs");

        // Assert
        prontuario.QueixaPrincipal.Should().Be("Queixa atualizada");
    }
}

public class HistoricoClinicoTests
{
    [Fact(DisplayName = "Deve criar histórico clínico válido")]
    public void DeveCriarHistoricoValido()
    {
        // Arrange & Act
        var historico = new HistoricoClinico(1, 2, "Vacinação V10 aplicada.");

        // Assert
        historico.AnimalId.Should().Be(1);
        historico.VeterinarioId.Should().Be(2);
        historico.Descricao.Should().Be("Vacinação V10 aplicada.");
    }

    [Fact(DisplayName = "Deve lançar exceção ao criar histórico sem descrição")]
    public void DeveLancarExcecaoSemDescricao()
    {
        var act = () => new HistoricoClinico(1, 2, "");
        act.Should().Throw<DominioException>().WithMessage("A descrição do histórico é obrigatória.");
    }
}