using FluentAssertions;
using PetCare.Domain.Entities.Animais;
using PetCare.Domain.Exceptions;
using Xunit;

namespace PetCare.Tests.Domain.Entities.Animais;

public class EspecieTests
{
    // =========================================================================
    // CRIAÇÃO
    // =========================================================================

    [Fact]
    public void DeveCriarEspecieValida()
    {
        var especie = new Especie("Cachorro");

        especie.Nome.Should().Be("Cachorro");
    }

    [Theory]
    [InlineData("cachorro", "Cachorro")]
    [InlineData("CACHORRO", "Cachorro")]
    [InlineData("CaChOrRo", "Cachorro")]
    [InlineData("  gato  ", "Gato")]
    public void DeveNormalizarNome(string entrada, string esperado)
    {
        var especie = new Especie(entrada);

        especie.Nome.Should().Be(esperado);
    }

    // =========================================================================
    // VALIDAÇÕES
    // =========================================================================

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("A")]      // muito curto (1 char)
    public void NaoDeveCriarEspecieComNomeInvalido(string nomeInvalido)
    {
        var act = () => new Especie(nomeInvalido);

        act.Should().Throw<DominioException>();
    }

    // =========================================================================
    // COMPORTAMENTO
    // =========================================================================

    [Fact]
    public void DeveRenomearEspecie()
    {
        // Arrange
        var especie = new Especie("Cachorro");

        // Act
        especie.Renomear("Cão");

        // Assert
        especie.Nome.Should().Be("Cão");
    }

    [Fact]
    public void NaoDeveRenomearComNomeInvalido()
    {
        var especie = new Especie("Cachorro");

        var act = () => especie.Renomear("");

        act.Should().Throw<DominioException>();
    }

    [Fact]
    public void DeveNormalizarNomeAoRenomear()
    {
        var especie = new Especie("Cachorro");

        especie.Renomear("gato");

        especie.Nome.Should().Be("Gato");
    }
}