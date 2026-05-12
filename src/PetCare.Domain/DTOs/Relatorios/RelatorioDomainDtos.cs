namespace PetCare.Domain.DTOs.Relatorios;

public class FaturamentoPorCategoriaDto
{
    public string Categoria { get; set; } = string.Empty;
    public decimal Valor { get; set; }
}

public class FaturamentoMensalDto
{
    public string MesAno { get; set; } = string.Empty;
    public decimal Valor { get; set; }
}

public class RelatorioDesempenhoProfissionalDto
{
    public long VeterinarioId { get; set; }
    public string NomeVeterinario { get; set; } = string.Empty;
    public int TotalAtendimentos { get; set; }
    public decimal ValorGerado { get; set; }
}

public class RelatorioProdutosMaisVendidosDto
{
    public long ProdutoId { get; set; }
    public string NomeProduto { get; set; } = string.Empty;
    public int QuantidadeVendida { get; set; }
    public decimal TotalArrecadado { get; set; }
}
