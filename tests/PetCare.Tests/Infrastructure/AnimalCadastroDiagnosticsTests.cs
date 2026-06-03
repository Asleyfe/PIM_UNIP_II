using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using PetCare.Application.DTOs.Animal;
using PetCare.Application.Services;
using PetCare.Infrastructure.Configuration;
using PetCare.Infrastructure.Data;
using PetCare.Infrastructure.Repositories;
using Xunit.Abstractions;

namespace PetCare.Tests.Infrastructure;

public class AnimalCadastroDiagnosticsTests
{
    private readonly NpgsqlConnectionFactory _factory;
    private readonly ITestOutputHelper _output;

    public AnimalCadastroDiagnosticsTests(ITestOutputHelper output)
    {
        _output = output;

        var config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.test.json", optional: false)
            .Build();

        var connectionString = Environment.GetEnvironmentVariable("PETCARE_DIAGNOSTIC_CONNECTION")
            ?? config.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("Connection string não encontrada.");

        _factory = new NpgsqlConnectionFactory(
            Options.Create(new SupabaseSettings { ConnectionString = connectionString }));
    }

    [Fact(DisplayName = "Diagnóstico Animal - deve cadastrar com payload da tela")]
    public async Task DeveCadastrarAnimalComPayloadDaTela()
    {
        var dto = new AnimalCreateDto
        {
            Nome = $"Web Teste {DateTime.UtcNow:yyyyMMddHHmmss}",
            DataNascimento = new DateOnly(2023, 2, 3),
            Peso = 9.49m,
            Sexo = "M",
            RacaId = 6,
            TutorId = 13
        };

        await ValidarReferencias(dto);
        await ValidarInsertDireto(dto);

        var service = new AnimalService(new AnimalRepository(_factory));
        var animal = await service.Cadastrar(dto);

        _output.WriteLine($"Animal cadastrado via serviço: id={animal.Id}, nome={animal.Nome}, sexo={animal.Sexo}");

        using var connection = _factory.CreateConnection();
        await connection.ExecuteAsync("DELETE FROM animal WHERE id = @Id", new { animal.Id });
    }

    private async Task ValidarReferencias(AnimalCreateDto dto)
    {
        using var connection = _factory.CreateConnection();

        var tutorExiste = await connection.ExecuteScalarAsync<bool>(
            "SELECT EXISTS (SELECT 1 FROM tutor WHERE id = @TutorId)",
            new { dto.TutorId });

        var racaExiste = await connection.ExecuteScalarAsync<bool>(
            "SELECT EXISTS (SELECT 1 FROM raca WHERE id = @RacaId)",
            new { dto.RacaId });

        _output.WriteLine($"Tutor {dto.TutorId} existe: {tutorExiste}");
        _output.WriteLine($"Raça {dto.RacaId} existe: {racaExiste}");

        Assert.True(tutorExiste, $"Tutor {dto.TutorId} não existe.");
        Assert.True(racaExiste, $"Raça {dto.RacaId} não existe.");
    }

    private async Task ValidarInsertDireto(AnimalCreateDto dto)
    {
        using var connection = _factory.CreateConnection();
        connection.Open();
        using var transaction = connection.BeginTransaction();

        var id = await connection.ExecuteScalarAsync<long>(
            """
            INSERT INTO animal (nome, data_nascimento, peso, sexo, tutor_id, raca_id)
            VALUES (@Nome, @DataNascimento, @Peso, @Sexo, @TutorId, @RacaId)
            RETURNING id
            """,
            new
            {
                dto.Nome,
                DataNascimento = dto.DataNascimento.ToDateTime(TimeOnly.MinValue),
                dto.Peso,
                dto.Sexo,
                dto.TutorId,
                dto.RacaId
            },
            transaction);

        _output.WriteLine($"Insert SQL direto funcionou dentro de transação: id={id}");
        transaction.Rollback();
    }
}
