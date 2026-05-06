using FluentAssertions;
using PetCare.Domain.Exceptions;
using PetCare.Domain.ValueObjects;
using Xunit;

namespace PetCare.Tests.Domain.ValueObjects;

public class EnderecoTests
{
    [Fact]
    public void DeveCriarEnderecoValido()
    {
        // Arrange & Act
        var endereco = new Endereco(
            rua: "Rua T-30",
            numero: "1234",
            bairro: "Setor Bueno",
            cidade: "Goiânia",
            estado: "GO");

        // Assert
        endereco.Rua.Should().Be("Rua T-30");
        endereco.Cidade.Should().Be("Goiânia");
        endereco.ToString().Should().Be("Rua T-30, 1234 — Setor Bueno, Goiânia/GO");
    }

    [Theory]
    [InlineData("", "1234", "Bueno", "Goiânia", "GO")]      // rua vazia
    [InlineData("Rua T-30", "", "Bueno", "Goiânia", "GO")]  // numero vazio
    [InlineData("Rua T-30", "1234", "", "Goiânia", "GO")]   // bairro vazio
    [InlineData("Rua T-30", "1234", "Bueno", "", "GO")]     // cidade vazia
    [InlineData("Rua T-30", "1234", "Bueno", "Goiânia", "")] // estado vazio
    public void DeveLancarExcecaoComCampoVazio(
        string rua, string numero, string bairro, string cidade, string estado)
    {
        // Act
        var act = () => new Endereco(rua, numero, bairro, cidade, estado);

        // Assert
        act.Should().Throw<DominioException>();
    }
}