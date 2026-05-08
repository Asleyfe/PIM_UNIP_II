using PetCare.Domain.Entities.Base;
using PetCare.Domain.Exceptions;

namespace PetCare.Domain.Entities.Animais;

/// <summary>
/// Representa uma raça animal vinculada a uma espécie (ex: "Labrador" da espécie "Cachorro").
/// Funciona como tabela de catálogo (lookup) — populada uma vez, raramente alterada.
/// Diferente de Especie, o nome NÃO é único globalmente — pode haver "Persa" em gato
/// e "Persa" em outra espécie hipotética. A unicidade prática é (Nome + EspecieId).
/// </summary>
public class Raca : EntidadeBase
{
    /// <summary>Nome da raça (ex: "Labrador", "Siamês").</summary>
    public string Nome { get; private set; }

    /// <summary>Identificador da espécie a qual a raça pertence.</summary>
    public long EspecieId { get; private set; }

    /// <summary>
    /// Construtor sem parâmetros para Dapper. Não usar diretamente.
    /// </summary>
    protected Raca()
    {
        Nome = string.Empty;
    }

    /// <summary>
    /// Cria uma nova raça com validação de invariantes.
    /// </summary>
    /// <param name="nome">Nome da raça.</param>
    /// <param name="especieId">ID da espécie (deve existir no banco — validação de FK fica no Repository).</param>
    public Raca(string nome, long especieId)
    {
        ValidarNome(nome);
        ValidarEspecieId(especieId);

        Nome = NormalizarNome(nome);
        EspecieId = especieId;
    }

    // =========================================================================
    // MÉTODOS DE COMPORTAMENTO
    // =========================================================================

    /// <summary>
    /// Renomeia a raça. Operação rara — útil para correções de digitação.
    /// </summary>
    public void Renomear(string novoNome)
    {
        ValidarNome(novoNome);
        Nome = NormalizarNome(novoNome);
    }

    /// <summary>
    /// Move a raça para outra espécie. Operação muito rara — geralmente
    /// indica erro de cadastro inicial. Considere se não seria melhor criar
    /// uma raça nova e remover a antiga, dependendo do impacto em animais já cadastrados.
    /// </summary>
    public void TransferirParaEspecie(long novaEspecieId)
    {
        ValidarEspecieId(novaEspecieId);
        EspecieId = novaEspecieId;
    }

    // =========================================================================
    // VALIDAÇÕES E NORMALIZAÇÃO
    // =========================================================================

    private static void ValidarNome(string nome)
    {
        if (string.IsNullOrWhiteSpace(nome))
            throw new DominioException("Nome da raça é obrigatório.");
        if (nome.Trim().Length < 2)
            throw new DominioException("Nome da raça deve ter pelo menos 2 caracteres.");
        if (nome.Trim().Length > 255)
            throw new DominioException("Nome da raça deve ter no máximo 255 caracteres.");
    }

    private static void ValidarEspecieId(long especieId)
    {
        if (especieId <= 0)
            throw new DominioException("EspecieId deve ser um identificador válido (maior que zero).");
    }

    /// <summary>
    /// Normaliza o nome: trim + capitalização da primeira letra de cada palavra.
    /// Garante consistência: "labrador retriever" e "LABRADOR RETRIEVER" viram "Labrador Retriever".
    /// </summary>
    private static string NormalizarNome(string nome)
    {
        var palavras = nome.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);
        var normalizadas = palavras.Select(p =>
            char.ToUpper(p[0]) + (p.Length > 1 ? p[1..].ToLower() : ""));
        return string.Join(' ', normalizadas);
    }
}