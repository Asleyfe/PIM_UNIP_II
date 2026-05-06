namespace PetCare.Domain.Exceptions;

/// <summary>
/// Lançada quando há tentativa de venda/saída superior ao estoque disponível.
/// Convertida em HTTP 400 pelo middleware.
/// </summary>
public class EstoqueInsuficienteException : DominioException
{
    public EstoqueInsuficienteException(long produtoId, int solicitado, int disponivel)
        : base($"Estoque insuficiente do produto {produtoId}. Solicitado: {solicitado}, disponível: {disponivel}.") { }
}