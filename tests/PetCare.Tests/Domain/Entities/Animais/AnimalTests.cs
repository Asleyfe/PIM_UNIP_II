using FluentAssertions;
using PetCare.Domain.Entities.Animais;
using PetCare.Domain.Enums;
using PetCare.Domain.Exceptions;
using Xunit;

namespace PetCare.Tests.Domain.Entities.Animais;

public class AnimalTests
{
    // Helper: cria um Animal válido com defaults sensatos
    private static Animal AnimalValido(
        string nome = "Rex",
        DateOnly? dataNascimento = null,
        decimal peso = 15.5m,
        Sexo sexo = Sexo.Macho,
        long tutorId = 1,
        long racaId = 1)
    {
        return new Animal(
            nome,
            dataNascimento ?? new DateOnly(2020, 1, 1),
            peso,
            sexo,
            tutorId,
            racaId);
    }

    // =========================================================================
    // CRIAÇÃO
    // =========================================================================

    [Fact]
    public void DeveCriarAnimalValido()
    {
        // Act
        var animal = new Animal(
            nome: "Rex",
            dataNascimento: new DateOnly(2020, 5, 15),
            peso: 25.5m,
            sexo: Sexo.Macho,
            tutorId: 1,
            racaId: 1);

        // Assert
        animal.Nome.Should().Be("Rex");
        animal.DataNascimento.Should().Be(new DateOnly(2020, 5, 15));
        animal.Peso.Should().Be(25.5m);
        animal.Sexo.Should().Be(Sexo.Macho);
        animal.TutorId.Should().Be(1);
        animal.RacaId.Should().Be(1);
    }

    [Fact]
    public void DeveAparararEspacosDoNome()
    {
        var animal = AnimalValido(nome: "  Rex  ");

        animal.Nome.Should().Be("Rex");
    }

    // =========================================================================
    // VALIDAÇÃO DE NOME
    // =========================================================================

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("A")]
    public void NaoDeveCriarAnimalComNomeInvalido(string nomeInvalido)
    {
        var act = () => AnimalValido(nome: nomeInvalido);

        act.Should().Throw<DominioException>();
    }

    // =========================================================================
    // VALIDAÇÃO DE DATA DE NASCIMENTO
    // =========================================================================

    [Fact]
    public void NaoDeveCriarAnimalComDataNascimentoFutura()
    {
        var amanha = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(1));

        var act = () => AnimalValido(dataNascimento: amanha);

        act.Should().Throw<DominioException>()
            .WithMessage("*futuro*");
    }

    [Fact]
    public void NaoDeveCriarAnimalComDataNascimentoMuitoAntiga()
    {
        var dataAntiga = new DateOnly(1950, 1, 1);

        var act = () => AnimalValido(dataNascimento: dataAntiga);

        act.Should().Throw<DominioException>();
    }

    [Fact]
    public void DeveAceitarDataDeNascimentoHoje()
    {
        var hoje = DateOnly.FromDateTime(DateTime.UtcNow);

        var act = () => AnimalValido(dataNascimento: hoje);

        act.Should().NotThrow();
    }

    // =========================================================================
    // VALIDAÇÃO DE PESO
    // =========================================================================

    [Theory]
    [InlineData(0)]
    [InlineData(-0.1)]
    [InlineData(-100)]
    public void NaoDeveCriarAnimalComPesoInvalido(decimal pesoInvalido)
    {
        var act = () => AnimalValido(peso: pesoInvalido);

        act.Should().Throw<DominioException>();
    }

    [Fact]
    public void NaoDeveCriarAnimalComPesoExcessivo()
    {
        var act = () => AnimalValido(peso: 1000m);

        act.Should().Throw<DominioException>();
    }

    [Theory]
    [InlineData(0.01)]    // mínimo possível
    [InlineData(15.5)]    // gato/cachorro pequeno
    [InlineData(80.0)]    // cachorro grande
    [InlineData(999.99)]  // limite
    public void DeveAceitarPesosValidos(decimal pesoValido)
    {
        var act = () => AnimalValido(peso: pesoValido);

        act.Should().NotThrow();
    }

    // =========================================================================
    // VALIDAÇÃO DE IDs
    // =========================================================================

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void NaoDeveCriarAnimalComTutorIdInvalido(long tutorIdInvalido)
    {
        var act = () => AnimalValido(tutorId: tutorIdInvalido);

        act.Should().Throw<DominioException>();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void NaoDeveCriarAnimalComRacaIdInvalido(long racaIdInvalido)
    {
        var act = () => AnimalValido(racaId: racaIdInvalido);

        act.Should().Throw<DominioException>();
    }

    // =========================================================================
    // PROPRIEDADES CALCULADAS — IDADE
    // =========================================================================

    [Fact]
    public void DeveCalcularIdadeCorretamente()
    {
        // Animal nasceu há 5 anos exatos
        var hoje = DateOnly.FromDateTime(DateTime.UtcNow);
        var dataNasc = hoje.AddYears(-5);

        var animal = AnimalValido(dataNascimento: dataNasc);

        animal.IdadeAnos.Should().Be(5);
    }

    [Fact]
    public void DeveAjustarIdadeQuandoAniversarioNaoChegou()
    {
        // Animal nasceu há 5 anos menos 1 dia (aniversário ainda não chegou)
        var hoje = DateOnly.FromDateTime(DateTime.UtcNow);
        var dataNasc = hoje.AddYears(-5).AddDays(1);

        var animal = AnimalValido(dataNascimento: dataNasc);

        animal.IdadeAnos.Should().Be(4);
    }

    [Fact]
    public void DeveIdentificarFilhoteQuandoMenorQueUmAno()
    {
        // Nasceu há 6 meses
        var seisMesesAtras = DateOnly.FromDateTime(DateTime.UtcNow.AddMonths(-6));

        var animal = AnimalValido(dataNascimento: seisMesesAtras);

        animal.EhFilhote.Should().BeTrue();
    }

    [Fact]
    public void NaoDeveSerFilhoteSeMaiorOuIgualAUmAno()
    {
        var hoje = DateOnly.FromDateTime(DateTime.UtcNow);
        var umAnoAtras = hoje.AddYears(-1);

        var animal = AnimalValido(dataNascimento: umAnoAtras);

        animal.EhFilhote.Should().BeFalse();
    }

    // =========================================================================
    // COMPORTAMENTO — Atualizar peso
    // =========================================================================

    [Fact]
    public void DeveAtualizarPeso()
    {
        var animal = AnimalValido(peso: 15.0m);

        animal.AtualizarPeso(17.5m);

        animal.Peso.Should().Be(17.5m);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-5)]
    [InlineData(1000)]
    public void NaoDeveAtualizarParaPesoInvalido(decimal pesoInvalido)
    {
        var animal = AnimalValido(peso: 15.0m);

        var act = () => animal.AtualizarPeso(pesoInvalido);

        act.Should().Throw<DominioException>();
    }

    // =========================================================================
    // COMPORTAMENTO — Transferência de tutor
    // =========================================================================

    [Fact]
    public void DeveTransferirParaOutroTutor()
    {
        var animal = AnimalValido(tutorId: 1);

        animal.TransferirParaTutor(2);

        animal.TutorId.Should().Be(2);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void NaoDeveTransferirParaTutorIdInvalido(long idInvalido)
    {
        var animal = AnimalValido();

        var act = () => animal.TransferirParaTutor(idInvalido);

        act.Should().Throw<DominioException>();
    }

    // =========================================================================
    // COMPORTAMENTO — Corrigir raça
    // =========================================================================

    [Fact]
    public void DeveCorrigirRaca()
    {
        var animal = AnimalValido(racaId: 1);

        animal.CorrigirRaca(2);

        animal.RacaId.Should().Be(2);
    }
}