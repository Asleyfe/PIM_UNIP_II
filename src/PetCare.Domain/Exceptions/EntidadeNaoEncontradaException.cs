namespace PetCare.Domain.Exceptions;

/// <summary>
/// Lançada quando uma entidade buscada por ID não existe no banco.
/// Convertida em HTTP 404 pelo middleware.
/// </summary>
public class EntidadeNaoEncontradaException : DominioException
{
    public EntidadeNaoEncontradaException(string entidade, long id)
        : base($"{entidade} com identificador {id} não encontrado(a).") { }
}