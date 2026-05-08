using PetCare.Domain.Entities.Base;
using PetCare.Domain.Exceptions;

namespace PetCare.Domain.Entities.Animais;

/// <summary>
/// Representa uma espécie animal (Cachorro, Gato, Pássaro, etc.).
/// Funciona como tabela de catálogo (lookup) — populada uma vez e raramente alterada.
/// O nome é único no sistema (constraint UNIQUE no banco).
/// </summary>
public class Especie : EntidadeBase
{
    /// <summary>Nome da espécie (ex: "Cachorro", "Gato"). Único no sistema.</summary>
    public string Nome { get; private set; }

    /// <summary>
    /// Construtor sem parâmetros para Dapper. Não usar diretamente.
    /// </summary>
    protected Especie()
    {
        Nome = string.Empty;
    }

    /// <summary>
    /// Cria uma nova espécie com validação de invariantes.
    /// </summary>
    public Especie(string nome)
    {
        ValidarNome(nome);
        Nome = NormalizarNome(nome);
    }

    // =========================================================================
    // MÉTODO DE COMPORTAMENTO
    // =========================================================================

    /// <summary>
    /// Renomeia a espécie. Operação rara — geralmente espécies são imutáveis,
    /// mas pode ser útil para correções de digitação no cadastro.
    /// </summary>
    public void Renomear(string novoNome)
    {
        ValidarNome(novoNome);
        Nome = NormalizarNome(novoNome);
    }

    // =========================================================================
    // VALIDAÇÕES E NORMALIZAÇÃO
    // =========================================================================

    private static void ValidarNome(string nome)
    {
        if (string.IsNullOrWhiteSpace(nome))
            throw new DominioException("Nome da espécie é obrigatório.");
        if (nome.Trim().Length < 2)
            throw new DominioException("Nome da espécie deve ter pelo menos 2 caracteres.");
        if (nome.Trim().Length > 255)
            throw new DominioException("Nome da espécie deve ter no máximo 255 caracteres.");
    }

    /// <summary>
    /// Normaliza o nome: trim + capitalização (primeira letra maiúscula, resto minúscula).
    /// Garante consistência: "cachorro", "CACHORRO" e "Cachorro" viram a mesma coisa.
    /// </summary>
    private static string NormalizarNome(string nome)
    {
        var trim = nome.Trim();
        return char.ToUpper(trim[0]) + trim[1..].ToLower();
    }
}