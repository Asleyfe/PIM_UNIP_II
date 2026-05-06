using FluentAssertions;
using PetCare.Domain.Entities.Pessoas;
using PetCare.Domain.Exceptions;
using PetCare.Domain.ValueObjects;
using Xunit;

namespace PetCare.Tests.Domain.Entities.Pessoas;

public class TutorTests
{
    // Helper para criar um endereço válido reutilizável nos testes
    private static Endereco EnderecoValido() => new(
        rua: "Rua T-30",
        numero: "1234",
        bairro: "Setor Bueno",
        cidade: "Goiânia",
        estado: "GO");

    // =========================================================================
    // TESTES DE CRIAÇÃO
    // =========================================================================

    [Fact]
    public void DeveCriarTutorValido()
    {
        // Act
        var tutor = new Tutor(
            nome: "Carlos Silva",
            cpf: "12345678909",
            telefone: "(62) 99999-8888",
            email: "carlos@email.com",
            endereco: EnderecoValido());

        // Assert
        tutor.Nome.Should().Be("Carlos Silva");
        tutor.Cpf.Should().Be("12345678909");
        tutor.Email.Should().Be("carlos@email.com");
        tutor.Endereco.Cidade.Should().Be("Goiânia");
    }

    [Fact]
    public void DeveNormalizarEmailParaMinusculas()
    {
        // Act
        var tutor = new Tutor(
            nome: "Carlos Silva",
            cpf: "12345678909",
            telefone: "(62) 99999-8888",
            email: "  CARLOS@Email.COM  ",
            endereco: EnderecoValido());

        // Assert
        tutor.Email.Should().Be("carlos@email.com");
    }

    // =========================================================================
    // VALIDAÇÃO DE NOME
    // =========================================================================

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("Ab")]
    public void NaoDeveCriarTutorComNomeInvalido(string nomeInvalido)
    {
        var act = () => new Tutor(
            nome: nomeInvalido,
            cpf: "12345678909",
            telefone: "(62) 99999-8888",
            email: "carlos@email.com",
            endereco: EnderecoValido());

        act.Should().Throw<DominioException>();
    }

    // =========================================================================
    // VALIDAÇÃO DE CPF
    // =========================================================================

    [Theory]
    [InlineData("")]
    [InlineData("123")]              // muito curto
    [InlineData("123456789012")]     // muito longo
    [InlineData("123456789ab")]      // contém letras
    public void NaoDeveCriarTutorComCpfInvalido(string cpfInvalido)
    {
        var act = () => new Tutor(
            nome: "Carlos Silva",
            cpf: cpfInvalido,
            telefone: "(62) 99999-8888",
            email: "carlos@email.com",
            endereco: EnderecoValido());

        act.Should().Throw<DominioException>();
    }

    // =========================================================================
    // VALIDAÇÃO DE EMAIL
    // =========================================================================

    [Theory]
    [InlineData("")]
    [InlineData("semarroba.com")]
    [InlineData("semponto@com")]
    public void NaoDeveCriarTutorComEmailInvalido(string emailInvalido)
    {
        var act = () => new Tutor(
            nome: "Carlos Silva",
            cpf: "12345678909",
            telefone: "(62) 99999-8888",
            email: emailInvalido,
            endereco: EnderecoValido());

        act.Should().Throw<DominioException>();
    }

    // =========================================================================
    // MÉTODOS DE COMPORTAMENTO
    // =========================================================================

    [Fact]
    public void DeveAtualizarContato()
    {
        // Arrange
        var tutor = new Tutor(
            nome: "Carlos Silva",
            cpf: "12345678909",
            telefone: "(62) 99999-8888",
            email: "carlos@email.com",
            endereco: EnderecoValido());

        // Act
        tutor.AtualizarContato("(62) 88888-7777", "novo@email.com");

        // Assert
        tutor.Telefone.Should().Be("(62) 88888-7777");
        tutor.Email.Should().Be("novo@email.com");
    }

    [Fact]
    public void DeveAlterarEndereco()
    {
        // Arrange
        var tutor = new Tutor(
            nome: "Carlos Silva",
            cpf: "12345678909",
            telefone: "(62) 99999-8888",
            email: "carlos@email.com",
            endereco: EnderecoValido());

        var novoEndereco = new Endereco(
            rua: "Avenida Goiás",
            numero: "500",
            bairro: "Centro",
            cidade: "Goiânia",
            estado: "GO");

        // Act
        tutor.AlterarEndereco(novoEndereco);

        // Assert
        tutor.Endereco.Bairro.Should().Be("Centro");
    }

    [Fact]
    public void NaoDeveAlterarEnderecoParaNulo()
    {
        var tutor = new Tutor(
            nome: "Carlos Silva",
            cpf: "12345678909",
            telefone: "(62) 99999-8888",
            email: "carlos@email.com",
            endereco: EnderecoValido());

        var act = () => tutor.AlterarEndereco(null!);

        act.Should().Throw<DominioException>();
    }
}