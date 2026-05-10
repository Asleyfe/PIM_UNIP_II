using FluentAssertions;
using Moq;
using PetCare.Application.DTOs.Vendas;
using PetCare.Application.Services;
using PetCare.Domain.Entities.Estoque;
using PetCare.Domain.Entities.Pessoas;
using PetCare.Domain.Entities.Vendas;
using PetCare.Domain.Interfaces.Repositories;
using PetCare.Domain.ValueObjects;

namespace PetCare.Tests.Application.Services;

public class VendaServiceTests
{
    private readonly Mock<IVendaRepository> _vendaRepoMock;
    private readonly Mock<ITutorRepository> _tutorRepoMock;
    private readonly Mock<IProdutoRepository> _produtoRepoMock;
    private readonly VendaService _service;

    public VendaServiceTests()
    {
        _vendaRepoMock = new Mock<IVendaRepository>();
        _tutorRepoMock = new Mock<ITutorRepository>();
        _produtoRepoMock = new Mock<IProdutoRepository>();

        _service = new VendaService(
            _vendaRepoMock.Object,
            _tutorRepoMock.Object,
            _produtoRepoMock.Object
        );
    }

    [Fact]
    public async Task RealizarVenda_DeveFalhar_QuandoEstoqueInsuficiente()
    {
        // Arrange
        var dto = new VendaCreateDto
        {
            TutorId = 1,
            FormaPagamento = "PIX",
            Itens = new List<ItemVendaCreateDto>
            {
                new() { ProdutoId = 10, Quantidade = 5, PrecoUnitario = 50 }
            }
        };

        var endereco = new Endereco("Rua", "123", "Bairro", "Cidade", "GO");
        var tutor = new Tutor("Tutor", "12345678901", "123", "a@a.com", endereco);
        var produto = new Produto("Ração", 50, "Alimentos", 5);
        // Estoque atual é 0 (padrão do construtor)

        _tutorRepoMock.Setup(r => r.ObterPorId(dto.TutorId)).ReturnsAsync(tutor);
        _produtoRepoMock.Setup(r => r.ObterPorId(10)).ReturnsAsync(produto);

        // Act
        Func<Task> act = async () => await _service.RealizarVenda(dto);

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("*Estoque insuficiente*");
    }

    [Fact]
    public async Task RealizarVenda_DeveCalcularValorTotalCorretamente()
    {
        // Arrange
        var dto = new VendaCreateDto
        {
            TutorId = 1,
            FormaPagamento = "Cartão",
            Itens = new List<ItemVendaCreateDto>
            {
                new() { ProdutoId = 1, Quantidade = 2, PrecoUnitario = 100 },
                new() { ProdutoId = 2, Quantidade = 1, PrecoUnitario = 50 }
            }
        };

        var endereco = new Endereco("Rua", "123", "Bairro", "Cidade", "GO");
        var tutor = new Tutor("Tutor", "12345678901", "123", "a@a.com", endereco);
        var p1 = new Produto("P1", 100, "Cat", 1);
        p1.AtualizarEstoque(10);
        var p2 = new Produto("P2", 50, "Cat", 1);
        p2.AtualizarEstoque(10);

        _tutorRepoMock.Setup(r => r.ObterPorId(dto.TutorId)).ReturnsAsync(tutor);
        _produtoRepoMock.Setup(r => r.ObterPorId(1)).ReturnsAsync(p1);
        _produtoRepoMock.Setup(r => r.ObterPorId(2)).ReturnsAsync(p2);

        _vendaRepoMock.Setup(r => r.InserirComItens(It.IsAny<Venda>(), It.IsAny<IEnumerable<ItemVenda>>()))
            .ReturnsAsync((Venda v, IEnumerable<ItemVenda> i) => v);

        // Act
        var resultado = await _service.RealizarVenda(dto);

        // Assert
        resultado.ValorTotal.Should().Be(250); // (2 * 100) + (1 * 50)
        _vendaRepoMock.Verify(r => r.InserirComItens(It.Is<Venda>(v => v.ValorTotal == 250), It.IsAny<IEnumerable<ItemVenda>>()), Times.Once);
    }
}
