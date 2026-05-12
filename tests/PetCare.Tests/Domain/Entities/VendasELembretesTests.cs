using FluentAssertions;
using PetCare.Domain.Entities.Vendas;
using PetCare.Domain.Entities.Comunicacao;
using PetCare.Domain.Enums;
using Xunit;

namespace PetCare.Tests.Domain.Entities;

public class VendaTests
{
    [Fact(DisplayName = "Deve criar uma venda e calcular o total corretamente")]
    public void DeveCriarVendaECalcularTotal()
    {
        // Arrange
        var venda = new Venda(1, "Cartão de Crédito");

        // Act
        venda.AdicionarItem(101, 2, 50.00m); // 100
        venda.AdicionarItem(102, 1, 35.50m); // 35.50

        // Assert
        venda.ValorTotal.Should().Be(135.50m);
        venda.Itens.Should().HaveCount(2);
    }
}

public class LembreteTests
{
    [Fact(DisplayName = "Deve criar registro de lembrete enviado")]
    public void DeveCriarLembreteEnviado()
    {
        // Arrange & Act
        var lembrete = new LembreteEnviado(1, 1, TipoLembrete.CONSULTA, MeioEnvio.WHATSAPP, StatusEnvio.ENVIADO, "Olá, sua consulta está agendada!");

        // Assert
        lembrete.TutorId.Should().Be(1);
        lembrete.StatusEnvio.Should().Be(StatusEnvio.ENVIADO);
        lembrete.DataEnvio.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
    }
}