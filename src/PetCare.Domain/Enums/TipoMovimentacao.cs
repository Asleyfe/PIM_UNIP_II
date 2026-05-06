namespace PetCare.Domain.Enums;

/// <summary>
/// Tipos de movimentação de estoque.
/// Espelha a constraint chk_tipo_mov da tabela movimentacao_estoque.
/// </summary>
public enum TipoMovimentacao
{
    Entrada,
    Saida
}