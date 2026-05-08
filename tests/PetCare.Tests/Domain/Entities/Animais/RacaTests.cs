using FluentAssertions;
using PetCare.Domain.Entities.Animais;
using PetCare.Domain.Exceptions;
using Xunit;

namespace PetCare.Tests.Domain.Entities.Animais;

public class RacaTests
{
    // =========================================================================
    // CRIAÇÃO
    // =========================================================================

    [Fact]
    public void DeveCriarRacaValida()
    {
        var raca = new Raca("Labrador", especieId: 1);

        raca.Nome.Should().Be("Labrador");
        raca.EspecieId.Should().Be(1);
    }

    [Theory]
    [InlineData("labrador", "Labrador")]
    [InlineData("LABRADOR", "Labrador")]
    [InlineData("LaBrAdOr", "Labrador")]
    [InlineData("pastor alemão", "Pastor Alemão")]
    [InlineData("PASTOR ALEMÃO", "Pastor Alemão")]
    [InlineData("  golden  retriever  ", "Golden Retriever")]
    public void DeveNormalizarNomeEmTitleCase(string entrada, string esperado)
    {
        var raca = new Raca(entrada, especieId: 1);

        raca.Nome.Should().Be(esperado);
    }

    // =========================================================================
    // VALIDAÇÃO DE NOME
    // =========================================================================

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("A")]
    public void NaoDeveCriarRacaComNomeInvalido(string nomeInvalido)
    {
        var act = () => new Raca(nomeInvalido, especieId: 1);

        act.Should().Throw<DominioException>();
    }

    // =========================================================================
    // VALIDAÇÃO DE ESPÉCIE ID
    // =========================================================================

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-100)]
    public void NaoDeveCriarRacaComEspecieIdInvalido(long especieIdInvalido)
    {
        var act = () => new Raca("Labrador", especieIdInvalido);

        act.Should().Throw<DominioException>();
    }

    // =========================================================================
    // COMPORTAMENTO — Renomear
    // =========================================================================

    [Fact]
    public void DeveRenomearRaca()
    {
        var raca = new Raca("Labrador", especieId: 1);

        raca.Renomear("Golden Retriever");

        raca.Nome.Should().Be("Golden Retriever");
    }

    [Fact]
    public void DeveNormalizarAoRenomear()
    {
        var raca = new Raca("Labrador", especieId: 1);

        raca.Renomear("pastor alemão");

        raca.Nome.Should().Be("Pastor Alemão");
    }

    [Fact]
    public void NaoDeveRenomearComNomeInvalido()
    {
        var raca = new Raca("Labrador", especieId: 1);

        var act = () => raca.Renomear("");

        act.Should().Throw<DominioException>();
    }

    // =========================================================================
    // COMPORTAMENTO — Transferir Espécie
    // =========================================================================

    [Fact]
    public void DeveTransferirParaOutraEspecie()
    {
        var raca = new Raca("Labrador", especieId: 1);

        raca.TransferirParaEspecie(2);

        raca.EspecieId.Should().Be(2);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void NaoDeveTransferirParaEspecieIdInvalido(long idInvalido)
    {
        var raca = new Raca("Labrador", especieId: 1);

        var act = () => raca.TransferirParaEspecie(idInvalido);

        act.Should().Throw<DominioException>();
    }
}