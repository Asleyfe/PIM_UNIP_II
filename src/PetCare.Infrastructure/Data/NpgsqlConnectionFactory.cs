using System.Data;
using Npgsql;

namespace PetCare.Infrastructure.Data;

public class NpgsqlConnectionFactory : IConnectionFactory
{
    private readonly string _connectionString;

    public NpgsqlConnectionFactory(string connectionString)
    {
        _connectionString = connectionString;
    }

    public IDbConnection CreateConnection()
    {
        return new NpgsqlConnection(_connectionString);
    }
}
