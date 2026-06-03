using Microsoft.Extensions.Configuration;
using Npgsql;

namespace PetCare.Tests.Infrastructure;

public class DatabaseConnectionSmokeTests
{
    [Fact(DisplayName = "Smoke DB - deve conectar e executar SELECT 1")]
    public async Task DeveConectarEExecutarSelect1()
    {
        var config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.test.json", optional: false)
            .Build();

        var connectionString = Environment.GetEnvironmentVariable("PETCARE_DIAGNOSTIC_CONNECTION")
            ?? config.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("Connection string não encontrada.");

        await using var connection = new NpgsqlConnection(connectionString);
        await connection.OpenAsync();

        await using var command = new NpgsqlCommand("select 1", connection);
        var result = await command.ExecuteScalarAsync();

        Assert.Equal(1, Convert.ToInt32(result));
    }
}
