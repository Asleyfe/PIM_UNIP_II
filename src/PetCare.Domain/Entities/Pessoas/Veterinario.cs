using PetCare.Domain.Entities.Base;
using PetCare.Domain.Exceptions;

namespace PetCare.Domain.Entities.Pessoas;

/// <summary>
/// Representa um veterinário que atende na clínica.
/// Encapsula dados profissionais (CRMV) e contato.
/// O CRMV é único no sistema e identifica profissionalmente o veterinário.
/// </summary>
public class Veterinario : EntidadeBase
{
    /// <summary>Nome completo do veterinário.</summary>
    public string Nome { get; private set; }

    /// <summary>
    /// Conselho Regional de Medicina Veterinária. Único no sistema.
    /// Formato esperado: 5 ou 6 dígitos seguidos de barra e UF (ex: 12345/GO, 123456/SP).
    /// </summary>
    public string Crmv { get; private set; }

    /// <summary>Telefone de contato (opcional). Com DDD.</summary>
    public string? Telefone { get; private set; }

    /// <summary>E-mail de contato (opcional).</summary>
    public string? Email { get; private set; }

    /// <summary>
    /// Construtor sem parâmetros para Dapper. Não usar diretamente.
    /// </summary>
    protected Veterinario()
    {
        Nome = string.Empty;
        Crmv = string.Empty;
        Telefone = null;
        Email = null;
    }

    /// <summary>
    /// Cria um novo veterinário com validação de invariantes.
    /// </summary>
    public Veterinario(string nome, string crmv, string? telefone = null, string? email = null)
    {
        ValidarNome(nome);
        ValidarCrmv(crmv);
        ValidarTelefone(telefone);
        ValidarEmail(email);

        Nome = nome.Trim();
        Crmv = crmv.Trim().ToUpper();
        Telefone = string.IsNullOrWhiteSpace(telefone) ? null : telefone.Trim();
        Email = string.IsNullOrWhiteSpace(email) ? null : email.Trim().ToLower();
    }

    // =========================================================================
    // MÉTODOS DE COMPORTAMENTO
    // =========================================================================

    /// <summary>
    /// Atualiza informações de contato (telefone e e-mail).
    /// Nome e CRMV não podem ser alterados após cadastro.
    /// </summary>
    public void AtualizarContato(string? telefone, string? email)
    {
        ValidarTelefone(telefone);
        ValidarEmail(email);

        Telefone = string.IsNullOrWhiteSpace(telefone) ? null : telefone.Trim();
        Email = string.IsNullOrWhiteSpace(email) ? null : email.Trim().ToLower();
    }

    // =========================================================================
    // VALIDAÇÕES PRIVADAS
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

    private static void ValidarCrmv(string crmv)
    {
        if (string.IsNullOrWhiteSpace(crmv))
            throw new DominioException("CRMV é obrigatório.");

        var crmvLimpo = crmv.Trim();

        if (crmvLimpo.Length > 50)
            throw new DominioException("CRMV deve ter no máximo 50 caracteres.");

        // Formato esperado: dígitos/UF (ex: 12345/GO, 123456/SP)
        // Validamos só o formato básico — números, barra obrigatória, UF de 2 letras
        var partes = crmvLimpo.Split('/');
        if (partes.Length != 2)
            throw new DominioException("CRMV deve estar no formato número/UF (ex: 12345/GO).");

        var numero = partes[0];
        var uf = partes[1];

        if (string.IsNullOrWhiteSpace(numero) || !numero.All(char.IsDigit))
            throw new DominioException("CRMV deve conter apenas dígitos antes da barra.");

        if (uf.Length != 2 || !uf.All(char.IsLetter))
            throw new DominioException("CRMV deve terminar com a UF em 2 letras (ex: GO, SP).");
    }

    private static void ValidarTelefone(string? telefone)
    {
        // Telefone é opcional — null/vazio é válido
        if (string.IsNullOrWhiteSpace(telefone))
            return;

        if (telefone.Trim().Length > 20)
            throw new DominioException("Telefone deve ter no máximo 20 caracteres.");
    }

    private static void ValidarEmail(string? email)
    {
        // E-mail é opcional — null/vazio é válido
        if (string.IsNullOrWhiteSpace(email))
            return;

        var emailLimpo = email.Trim();

        if (emailLimpo.Length > 255)
            throw new DominioException("E-mail deve ter no máximo 255 caracteres.");

        if (!emailLimpo.Contains('@') || !emailLimpo.Contains('.'))
            throw new DominioException("E-mail inválido.");
    }
}