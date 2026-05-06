using PetCare.Domain.Entities.Base;
using PetCare.Domain.Exceptions;
using PetCare.Domain.ValueObjects;

namespace PetCare.Domain.Entities.Pessoas;

/// <summary>
/// Representa um tutor de animal — cliente da PetCare.
/// Encapsula dados pessoais, contato e endereço, com validações de invariantes
/// no momento da criação e em qualquer alteração.
/// </summary>
public class Tutor : EntidadeBase
{
    /// <summary>Nome completo do tutor.</summary>
    public string Nome { get; private set; }

    /// <summary>CPF (somente dígitos, 11 caracteres). Único no sistema.</summary>
    public string Cpf { get; private set; }

    /// <summary>Telefone de contato (com DDD).</summary>
    public string Telefone { get; private set; }

    /// <summary>E-mail de contato. Único no sistema.</summary>
    public string Email { get; private set; }

    /// <summary>Endereço completo, encapsulado como Value Object.</summary>
    public Endereco Endereco { get; private set; }

    /// <summary>
    /// Construtor sem parâmetros, usado pelo Dapper para hidratação.
    /// Inicializa propriedades com valores neutros que serão sobrescritos pela hidratação.
    /// Não usar diretamente em código de aplicação.
    /// </summary>
    protected Tutor()
    {
        Nome = string.Empty;
        Cpf = string.Empty;
        Telefone = string.Empty;
        Email = string.Empty;
        Endereco = null!; // será preenchido pelo Dapper
    }

    /// <summary>
    /// Cria um novo tutor com validação de todos os invariantes.
    /// </summary>
    public Tutor(string nome, string cpf, string telefone, string email, Endereco endereco)
    {
        ValidarNome(nome);
        ValidarCpf(cpf);
        ValidarTelefone(telefone);
        ValidarEmail(email);
        if (endereco is null)
            throw new DominioException("Endereço é obrigatório.");

        Nome = nome.Trim();
        Cpf = cpf.Trim();
        Telefone = telefone.Trim();
        Email = email.Trim().ToLower();
        Endereco = endereco;
    }

    // =========================================================================
    // MÉTODOS DE COMPORTAMENTO
    // Em vez de expor setters, expomos ações que fazem sentido no negócio.
    // =========================================================================

    /// <summary>
    /// Atualiza informações de contato (telefone e e-mail) preservando dados imutáveis.
    /// Nome e CPF não podem ser alterados — se necessário, isso vira outra operação.
    /// </summary>
    public void AtualizarContato(string telefone, string email)
    {
        ValidarTelefone(telefone);
        ValidarEmail(email);

        Telefone = telefone.Trim();
        Email = email.Trim().ToLower();
    }

    /// <summary>
    /// Substitui o endereço atual por um novo. O endereço antigo é descartado.
    /// </summary>
    public void AlterarEndereco(Endereco novoEndereco)
    {
        if (novoEndereco is null)
            throw new DominioException("Endereço é obrigatório.");

        Endereco = novoEndereco;
    }

    // =========================================================================
    // VALIDAÇÕES PRIVADAS
    // Centralizadas aqui para que tanto o construtor quanto os métodos de
    // comportamento usem as mesmas regras.
    // =========================================================================

    private static void ValidarNome(string nome)
    {
        if (string.IsNullOrWhiteSpace(nome))
            throw new DominioException("Nome é obrigatório.");
        if (nome.Trim().Length < 3)
            throw new DominioException("Nome deve ter pelo menos 3 caracteres.");
        if (nome.Trim().Length > 255)
            throw new DominioException("Nome deve ter no máximo 255 caracteres.");
    }

    private static void ValidarCpf(string cpf)
    {
        if (string.IsNullOrWhiteSpace(cpf))
            throw new DominioException("CPF é obrigatório.");

        var cpfLimpo = cpf.Trim();

        if (cpfLimpo.Length != 11)
            throw new DominioException("CPF deve ter exatamente 11 dígitos.");

        if (!cpfLimpo.All(char.IsDigit))
            throw new DominioException("CPF deve conter apenas dígitos.");

        // O algoritmo dos dígitos verificadores já é validado pela função
        // fn_validar_cpf no Postgres (CONSTRAINT chk_cpf_valido).
        // Aqui validamos só o formato — o banco recusa CPFs matematicamente inválidos.
    }

    private static void ValidarTelefone(string telefone)
    {
        if (string.IsNullOrWhiteSpace(telefone))
            throw new DominioException("Telefone é obrigatório.");
        if (telefone.Trim().Length > 20)
            throw new DominioException("Telefone deve ter no máximo 20 caracteres.");
    }

    private static void ValidarEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new DominioException("E-mail é obrigatório.");
        if (email.Trim().Length > 255)
            throw new DominioException("E-mail deve ter no máximo 255 caracteres.");
        if (!email.Contains('@') || !email.Contains('.'))
            throw new DominioException("E-mail inválido.");
    }
}