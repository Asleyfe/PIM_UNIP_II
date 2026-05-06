using System.Data;

namespace PetCare.Infrastructure.Data;

/// <summary>
/// Abstração para criação de conexões com o banco de dados.
/// Permite que repositórios sejam testáveis (via mock) e desacoplados
/// do provedor concreto (Npgsql, SqlClient, etc.).
/// </summary>
public interface IConnectionFactory
{
    /// <summary>
    /// Cria uma nova conexão com o banco. O caller é responsável por dispor
    /// (using var conn = factory.CreateConnection();).
    /// </summary>
    IDbConnection CreateConnection();
}