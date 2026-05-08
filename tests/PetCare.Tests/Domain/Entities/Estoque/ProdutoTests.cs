using FluentAssertions;
using PetCare.Domain.Entities.Estoque;
using Xunit;

namespace PetCare.Tests.Domain.Entities.Estoque;

public class ProdutoTests
{
    [Fact(DisplayName = "Deve criar um produto válido")]
    public void DeveCriarProdutoValido()
    {
        // Arrange & Act
        var produto = new Produto("Ração Premium", 150.00m, "Acessórios", 5, "Ração para cães");

        // Assert
        produto.Nome.Should().Be("Ração Premium");
        produto.Preco.Should().Be(150.00m);
        produto.QuantidadeEstoque.Should().Be(0);
    }

    [Fact(DisplayName = "Deve lançar exceção ao criar produto sem nome")]
    public void DeveLancarExcecaoSemNome()
    {
        var act = () => new Produto("", 10m, "Cat", 1);
        act.Should().Throw<ArgumentException>().WithMessage("O nome do produto é obrigatório.");
    }

    [Theory(DisplayName = "Deve validar obrigatoriedade de validade para perecíveis")]
    [InlineData("Rações")]
    [InlineData("Medicamentos")]
    public void DeveExigirValidadeParaPereciveis(string categoria)
    {
        var act = () => new Produto("Teste", 10m, categoria, 1, null, null);
        act.Should().Throw<ArgumentException>().WithMessage($"A validade é obrigatória para a categoria {categoria}.");
    }

    [Fact(DisplayName = "Deve atualizar o estoque corretamente")]
    public void DeveAtualizarEstoque()
    {
        // Arrange
        var produto = new Produto("Brinquedo", 25m, "Acessórios", 2);

        // Act
        produto.AtualizarEstoque(10); // Entrada
        produto.AtualizarEstoque(-3); // Saída

        // Assert
        produto.QuantidadeEstoque.Should().Be(7);
    }

    [Fact(DisplayName = "Deve alertar quando atingir estoque mínimo")]
    public void DeveAlertarEstoqueMinimo()
    {
        // Arrange
        var produto = new Produto("Brinquedo", 25m, "Acessórios", 5);
        produto.AtualizarEstoque(5);

        // Act & Assert
        produto.PrecisaReposição().Should().BeTrue();
    }

    [Fact(DisplayName = "Deve identificar produto próximo ao vencimento")]
    public void DeveIdentificarProximoVencimento()
    {
        // Arrange
        var validade = DateTime.UtcNow.AddDays(15);
        var produto = new Produto("Ração", 100m, "Rações", 2, null, validade);

        // Assert
        produto.ProximoAoVencimento(30).Should().BeTrue();
    }
}