using PetCare.Domain.Entities.Base;
using PetCare.Domain.Enums;
using PetCare.Domain.Exceptions;

namespace PetCare.Domain.Entities.Animais;

/// <summary>
/// Representa um animal de estimação cadastrado na clínica.
/// Vinculado a um Tutor (proprietário) e a uma Raca (que por sua vez aponta para Especie).
/// O peso é mutável (animais ganham/perdem peso ao longo da vida);
/// a data de nascimento e o sexo são imutáveis após cadastro.
/// </summary>
public class Animal : EntidadeBase
{
    /// <summary>Nome do animal (ex: "Rex", "Mia").</summary>
    public string Nome { get; private set; }

    /// <summary>Data de nascimento. Imutável após cadastro.</summary>
    public DateOnly DataNascimento { get; private set; }

    /// <summary>Peso atual em kg. Atualizado a cada consulta via AtualizarPeso().</summary>
    public decimal Peso { get; private set; }

    /// <summary>Sexo do animal. Imutável após cadastro.</summary>
    public Sexo Sexo { get; private set; }

    /// <summary>FK para o tutor (proprietário do animal).</summary>
    public long TutorId { get; private set; }

    /// <summary>FK para a raça do animal (que por sua vez aponta para Especie).</summary>
    public long RacaId { get; private set; }

    // =========================================================================
    // PROPRIEDADES CALCULADAS (não persistidas — derivam de outras propriedades)
    // =========================================================================

    /// <summary>
    /// Idade do animal em anos completos, calculada a partir da data de nascimento.
    /// Não é persistida no banco — sempre derivada de DataNascimento.
    /// </summary>
    public int IdadeAnos
    {
        get
        {
            var hoje = DateOnly.FromDateTime(DateTime.UtcNow);
            var idade = hoje.Year - DataNascimento.Year;

            // Se ainda não fez aniversário este ano, tira 1 da conta
            if (DataNascimento > hoje.AddYears(-idade))
                idade--;

            return idade;
        }
    }

    /// <summary>
    /// Indica se o animal é filhote (menos de 1 ano de idade).
    /// Útil para regras de negócio (ex: vacinas específicas de filhote).
    /// </summary>
    public bool EhFilhote => IdadeAnos < 1;

    // =========================================================================
    // CONSTRUTORES
    // =========================================================================

    /// <summary>
    /// Construtor sem parâmetros para Dapper. Não usar diretamente.
    /// </summary>
    protected Animal()
    {
        Nome = string.Empty;
    }

    /// <summary>
    /// Cria um novo animal com validação de invariantes.
    /// </summary>
    public Animal(
        string nome,
        DateOnly dataNascimento,
        decimal peso,
        Sexo sexo,
        long tutorId,
        long racaId)
    {
        ValidarNome(nome);
        ValidarDataNascimento(dataNascimento);
        ValidarPeso(peso);
        ValidarId(tutorId, nameof(tutorId));
        ValidarId(racaId, nameof(racaId));

        Nome = nome.Trim();
        DataNascimento = dataNascimento;
        Peso = peso;
        Sexo = sexo;
        TutorId = tutorId;
        RacaId = racaId;
    }

    // =========================================================================
    // MÉTODOS DE COMPORTAMENTO
    // =========================================================================

    /// <summary>
    /// Atualiza o peso do animal. Comum a cada consulta veterinária.
    /// </summary>
    public void AtualizarPeso(decimal novoPeso)
    {
        ValidarPeso(novoPeso);
        Peso = novoPeso;
    }

    /// <summary>
    /// Transfere o animal para outro tutor. Operação rara — útil em casos
    /// de adoção, falecimento do tutor anterior, ou venda do animal.
    /// </summary>
    public void TransferirParaTutor(long novoTutorId)
    {
        ValidarId(novoTutorId, nameof(novoTutorId));
        TutorId = novoTutorId;
    }

    /// <summary>
    /// Corrige a raça cadastrada. Útil quando o cadastro inicial foi feito
    /// com raça incorreta (ex: filhote SRD que depois revelou ser de raça definida).
    /// </summary>
    public void CorrigirRaca(long novaRacaId)
    {
        ValidarId(novaRacaId, nameof(novaRacaId));
        RacaId = novaRacaId;
    }

    // =========================================================================
    // VALIDAÇÕES PRIVADAS
    // =========================================================================

    private static void ValidarNome(string nome)
    {
        if (string.IsNullOrWhiteSpace(nome))
            throw new DominioException("Nome do animal é obrigatório.");
        if (nome.Trim().Length < 2)
            throw new DominioException("Nome do animal deve ter pelo menos 2 caracteres.");
        if (nome.Trim().Length > 255)
            throw new DominioException("Nome do animal deve ter no máximo 255 caracteres.");
    }

    private static void ValidarDataNascimento(DateOnly dataNascimento)
    {
        var hoje = DateOnly.FromDateTime(DateTime.UtcNow);

        if (dataNascimento > hoje)
            throw new DominioException("Data de nascimento não pode ser no futuro.");

        // Sanidade: animal mais velho que 50 anos é improvável (papagaios chegam a 60+,
        // mas o cadastro provavelmente é erro de digitação)
        if (dataNascimento < hoje.AddYears(-50))
            throw new DominioException("Data de nascimento muito antiga. Verifique o ano.");
    }

    private static void ValidarPeso(decimal peso)
    {
        if (peso <= 0)
            throw new DominioException("Peso deve ser maior que zero.");

        // numeric(5,2) no banco aceita até 999.99
        if (peso > 999.99m)
            throw new DominioException("Peso não pode exceder 999,99 kg.");
    }

    private static void ValidarId(long id, string nomeCampo)
    {
        if (id <= 0)
            throw new DominioException($"{nomeCampo} deve ser um identificador válido (maior que zero).");
    }
}