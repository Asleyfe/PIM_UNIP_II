using PetCare.Domain.Entities.Base;

namespace PetCare.Domain.Interfaces.Repositories;

/// <summary>
/// Contrato base com operações CRUD comuns a todos os repositórios.
/// As implementações ficam em PetCare.Infrastructure usando Dapper.
/// </summary>
/// <typeparam name="T">Tipo da entidade. Deve herdar de EntidadeBase.</typeparam>
public interface IRepositorioBase<T> where T : EntidadeBase
{
    /// <summary>
    /// Busca uma entidade pelo seu identificador.
    /// </summary>
    /// <returns>A entidade ou null se não encontrada.</returns>
    Task<T?> ObterPorId(long id);

    /// <summary>
    /// Lista todas as entidades. Use com cautela em tabelas grandes.
    /// </summary>
    Task<IEnumerable<T>> Listar();

    /// <summary>
    /// Insere uma nova entidade. Retorna a entidade com Id preenchido.
    /// </summary>
    Task<T> Inserir(T entidade);

    /// <summary>
    /// Atualiza uma entidade existente.
    /// </summary>
    /// <returns>True se a entidade foi atualizada, false se não foi encontrada.</returns>
    Task<bool> Atualizar(T entidade);

    /// <summary>
    /// Remove uma entidade pelo identificador.
    /// </summary>
    /// <returns>True se removida, false se não encontrada.</returns>
    Task<bool> Remover(long id);
}