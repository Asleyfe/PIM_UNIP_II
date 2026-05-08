using FluentAssertions;
using PetCare.Domain.Entities.Pessoas;
using PetCare.Domain.Exceptions;
using Xunit;

namespace PetCare.Tests.Domain.Entities.Pessoas;

public class VeterinarioTests
{
    // =========================================================================
    // TESTES DE CRIAÇÃO
    // =========================================================================

    [Fact]
    public void DeveCriarVeterinarioComDadosObrigatorios()
    {
        // Act
        var vet = new Veterinario(
            nome: "Ana Souza",
            crmv: "12345/GO");

        // Assert
        vet.Nome.Should().Be("Ana Souza");
        vet.Crmv.Should().Be("12345/GO");
        vet.Telefone.Should().BeNull();
        vet.Email.Should().BeNull();
    }

    [Fact]
    public void DeveCriarVeterinarioComContatosOpcionais()
    {
        // Act
        var vet = new Veterinario(
            nome: "Ana Souza",
            crmv: "12345/GO",
            telefone: "(62) 99999-1234",
            email: "ana@petcare.com");

        // Assert
        vet.Telefone.Should().Be("(62) 99999-1234");
        vet.Email.Should().Be("ana@petcare.com");
    }

    [Fact]
    public void DeveNormalizarCrmvParaMaiusculas()
    {
        // Act
        var vet = new Veterinario(
            nome: "Ana Souza",
            crmv: "12345/go");

        // Assert
        vet.Crmv.Should().Be("12345/GO");
    }

    [Fact]
    public void DeveNormalizarEmailParaMinusculas()
    {
        // Act
        var vet = new Veterinario(
            nome: "Ana Souza",
            crmv: "12345/GO",
            email: "  ANA@PetCare.COM  ");

        // Assert
        vet.Email.Should().Be("ana@petcare.com");
    }

    [Fact]
    public void DeveTratarTelefoneVazioComoNulo()
    {
        // Act
        var vet = new Veterinario(
            nome: "Ana Souza",
            crmv: "12345/GO",
            telefone: "   ",
            email: "");

        // Assert
        vet.Telefone.Should().BeNull();
        vet.Email.Should().BeNull();
    }

    // =========================================================================
    // VALIDAÇÃO DE NOME
    // =========================================================================

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("Ab")]
    public void NaoDeveCriarVeterinarioComNomeInvalido(string nomeInvalido)
    {
        var act = () => new Veterinario(nome: nomeInvalido, crmv: "12345/GO");

        act.Should().Throw<DominioException>();
    }

    // =========================================================================
    // VALIDAÇÃO DE CRMV
    // =========================================================================

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("12345")]              // sem UF
    [InlineData("12345/")]             // UF vazia
    [InlineData("/GO")]                // número vazio
    [InlineData("ABC/GO")]             // letras antes da barra
    [InlineData("12345/GOI")]          // UF com 3 letras
    [InlineData("12345/G")]            // UF com 1 letra
    [InlineData("12345/12")]           // UF com números
    [InlineData("12345-GO")]           // separador errado
    public void NaoDeveCriarVeterinarioComCrmvInvalido(string crmvInvalido)
    {
        var act = () => new Veterinario(nome: "Ana Souza", crmv: crmvInvalido);

        act.Should().Throw<DominioException>();
    }

    [Theory]
    [InlineData("12345/GO")]
    [InlineData("123456/SP")]
    [InlineData("9876/MG")]
    public void DeveAceitarCrmvComFormatoValido(string crmvValido)
    {
        var act = () => new Veterinario(nome: "Ana Souza", crmv: crmvValido);

        act.Should().NotThrow();
    }

    // =========================================================================
    // VALIDAÇÃO DE EMAIL
    // =========================================================================

    [Theory]
    [InlineData("semarroba.com")]
    [InlineData("semponto@com")]
    public void NaoDeveCriarVeterinarioComEmailMalFormatado(string emailInvalido)
    {
        var act = () => new Veterinario(
            nome: "Ana Souza",
            crmv: "12345/GO",
            email: emailInvalido);

        act.Should().Throw<DominioException>();
    }

    // =========================================================================
    // MÉTODOS DE COMPORTAMENTO
    // =========================================================================

    [Fact]
    public void DeveAtualizarContato()
    {
        // Arrange
        var vet = new Veterinario(
            nome: "Ana Souza",
            crmv: "12345/GO",
            telefone: "(62) 99999-1234",
            email: "ana@petcare.com");

        // Act
        vet.AtualizarContato("(62) 88888-5678", "ana.nova@petcare.com");

        // Assert
        vet.Telefone.Should().Be("(62) 88888-5678");
        vet.Email.Should().Be("ana.nova@petcare.com");
    }

    [Fact]
    public void DevePermitirAtualizarContatoParaNulo()
    {
        // Arrange
        var vet = new Veterinario(
            nome: "Ana Souza",
            crmv: "12345/GO",
            telefone: "(62) 99999-1234",
            email: "ana@petcare.com");

        // Act
        vet.AtualizarContato(null, null);

        // Assert
        vet.Telefone.Should().BeNull();
        vet.Email.Should().BeNull();
    }
}