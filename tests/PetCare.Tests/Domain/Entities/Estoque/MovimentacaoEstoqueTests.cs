using FluentAssertions;
using PetCare.Domain.Entities.Estoque;
using PetCare.Domain.Enums;
using Xunit;

namespace PetCare.Tests.Domain.Entities.Estoque;

public class MovimentacaoEstoqueTests
{
    [Fact(DisplayName = "Deve criar movimentação válida")]
    public void DeveCriarMovimentacaoValida()
    {
        // Arrange & Act
        var mov = new MovimentacaoEstoque(1, 10, TipoMovimentacao.Entrada);

        // Assert
        mov.ProdutoId.Should().Be(1);
        mov.Quantidade.Should().Be(10);
        mov.Tipo.Should().Be(TipoMovimentacao.Entrada);
    }

    [Fact(DisplayName = "Não deve permitir quantidade zero ou negativa")]
    public void NaoDevePermitirQuantidadeInvalida()
    {
        var act = () => new MovimentacaoEstoque(1, 0, TipoMovimentacao.Entrada);
        act.Should().Throw<ArgumentException>().WithMessage("A quantidade da movimentação deve ser maior que zero.");
    }
}