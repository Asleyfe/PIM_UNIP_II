using System.Data;
using Microsoft.Extensions.Options;
using Npgsql;
using PetCare.Infrastructure.Configuration;

namespace PetCare.Infrastructure.Data;

/// <summary>
/// Cria conexões com o Postgres do Supabase usando Npgsql.
/// Lê a connection string do IOptions<SupabaseSettings>, que vem do appsettings.
/// </summary>
public class NpgsqlConnectionFactory : IConnectionFactory
{
    private readonly string _connectionString;

    public NpgsqlConnectionFactory(IOptions<SupabaseSettings> settings)
    {
        _connectionString = settings.Value.ConnectionString;

        if (string.IsNullOrWhiteSpace(_connectionString))
            throw new InvalidOperationException(
                "Connection string do Supabase não configurada. " +
                "Verifique a seção 'Supabase:ConnectionString' em appsettings.json.");
    }

    public IDbConnection CreateConnection() => new NpgsqlConnection(_connectionString);
}