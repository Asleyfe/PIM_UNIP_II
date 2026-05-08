namespace PetCare.Domain.Entities.Base;

/// <summary>
/// Classe base para todas as entidades persistidas do sistema.
/// Contém propriedades comuns: identificador único e data de criação.
/// </summary>
public abstract class EntidadeBase
{
    /// <summary>
    /// Identificador único da entidade. Gerado pelo banco (BIGSERIAL).
    /// </summary>
    public long Id { get; protected set; }

    /// <summary>
    /// Data e hora de criação do registro. Preenchido pelo banco (DEFAULT NOW()).
    /// </summary>
    public DateTime CreatedAt { get; protected set; }

    /// <summary>
    /// Permite que a infraestrutura (Repository) preencha o ID gerado pelo banco.
    /// </summary>
    public void SetId(long id) => Id = id;

    /// <summary>
    /// Permite que a infraestrutura (Repository) preencha a data de criação gerada pelo banco.
    /// </summary>
    public void SetCreatedAt(DateTime createdAt) => CreatedAt = createdAt;

    /// <summary>
    /// Construtor protegido sem parâmetros, requerido pelo Dapper para hidratação
    /// de objetos a partir do banco. Não deve ser chamado diretamente pelo código de aplicação.
    /// </summary>
    protected EntidadeBase() { }
}