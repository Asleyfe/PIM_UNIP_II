namespace PetCare.Domain.Exceptions;

/// <summary>
/// Exceção base para todas as violações de regra de negócio do sistema.
/// Capturada pelo TratamentoErrosMiddleware e convertida em HTTP 400 por padrão.
/// </summary>
public class DominioException : Exception
{
    public DominioException(string mensagem) : base(mensagem) { }

    public DominioException(string mensagem, Exception innerException)
        : base(mensagem, innerException) { }
}