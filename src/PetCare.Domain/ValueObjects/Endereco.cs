using PetCare.Domain.Exceptions;

namespace PetCare.Domain.ValueObjects;

/// <summary>
/// Value Object que representa um endereço completo.
/// Imutável: uma vez criado, não pode ser alterado — para mudar o endereço,
/// cria-se um novo Endereco e atribui à entidade.
/// Espelha os campos rua, numero, bairro, cidade, estado da tabela tutor.
/// </summary>

public class Endereco
{
	public string Rua { get; private set; }
	public string Numero { get; private set; }
	public string Bairro { get; private set; }
	public string Cidade { get; private set; }
	public string Estado { get; private set; }


    /// <summary>
    /// Constructor sem parametros para o Dappert. Não usar diretamente.
    /// </summary>
    protected Endereco()
	{
		Rua = string.Empty;
		Numero = string.Empty;
		Bairro = string.Empty;
		Cidade = string.Empty;
		Estado = string.Empty;
    }

	public Endereco(string rua, string numero, string bairro, string cidade, string estado)
	{
		if (string.IsNullOrWhiteSpace(rua))
			throw new DominioException("Rua é Obrigatoria.");
        if (string.IsNullOrWhiteSpace(numero))
            throw new DominioException("Numero é Obrigatorio.");
        if (string.IsNullOrWhiteSpace(rua))
            throw new DominioException("Rua é Obrigatoria.");
        if (string.IsNullOrWhiteSpace(bairro))
            throw new DominioException("Bairro é obrigatório.");
        if (string.IsNullOrWhiteSpace(cidade))
            throw new DominioException("Cidade é obrigatória.");
        if (string.IsNullOrWhiteSpace(estado))
            throw new DominioException("Estado é obrigatório.");
        if (estado.Trim().Length > 50)
            throw new DominioException("Estado deve ter no máximo 50 caracteres.");

        Rua = rua.Trim();
        Numero = numero.Trim();
        Bairro= bairro.Trim();
        Cidade = cidade.Trim();
        Estado= estado.Trim();
    }
    /// <summary>
    /// Representação textual completa do eendereço.
    /// util para logs, exibição em telas e mensagens.
    /// </summary>
    public override string ToString()
        => $"{Rua}, {Numero} — {Bairro}, {Cidade}/{Estado}";
}
