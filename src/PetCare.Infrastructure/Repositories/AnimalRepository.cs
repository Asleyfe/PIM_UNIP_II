using Dapper;
using PetCare.Domain.Entities.Animais;
using PetCare.Domain.Enums;
using PetCare.Domain.Interfaces.Repositories;
using PetCare.Infrastructure.Data;

namespace PetCare.Infrastructure.Repositories;

public class AnimalRepository : IAnimalRepository
{
    private readonly IConnectionFactory _connectionFactory;

    public AnimalRepository(IConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<Animal?> ObterPorId(long id)
    {
        const string sql = """
            SELECT id, nome, data_nascimento AS DataNascimento, peso, sexo,
                   tutor_id AS TutorId, raca_id AS RacaId
            FROM animal
            WHERE id = @Id
            """;

        using var conn = _connectionFactory.CreateConnection();
        var row = await conn.QuerySingleOrDefaultAsync(sql, new { Id = id });
        return row is null ? null : MapearAnimal(row);
    }

    public async Task<IEnumerable<Animal>> Listar()
    {
        const string sql = """
            SELECT id, nome, data_nascimento AS DataNascimento, peso, sexo,
                   tutor_id AS TutorId, raca_id AS RacaId
            FROM animal
            ORDER BY nome
            """;

        using var conn = _connectionFactory.CreateConnection();
        var rows = await conn.QueryAsync(sql);
        return rows.Select(MapearAnimal);
    }

    public async Task<Animal> Inserir(Animal animal)
    {
        const string sql = """
            INSERT INTO animal (nome, data_nascimento, peso, sexo, tutor_id, raca_id)
            VALUES (@Nome, @DataNascimento, @Peso, @Sexo, @TutorId, @RacaId)
            RETURNING id
            """;

        using var conn = _connectionFactory.CreateConnection();
        var result = await conn.QuerySingleAsync(sql, new
        {
            animal.Nome,
            DataNascimento = animal.DataNascimento.ToDateTime(TimeOnly.MinValue),
            animal.Peso,
            Sexo = ConverterSexoParaBanco(animal.Sexo),
            animal.TutorId,
            animal.RacaId
        });

        animal.SetId((long)result.id);
        animal.SetCreatedAt(DateTime.UtcNow);

        return animal;
    }

    public async Task<bool> Atualizar(Animal animal)
    {
        const string sql = """
            UPDATE animal
            SET nome = @Nome, data_nascimento = @DataNascimento, peso = @Peso, 
                sexo = @Sexo, tutor_id = @TutorId, raca_id = @RacaId
            WHERE id = @Id
            """;

        using var conn = _connectionFactory.CreateConnection();
        var affected = await conn.ExecuteAsync(sql, new
        {
            animal.Id,
            animal.Nome,
            DataNascimento = animal.DataNascimento.ToDateTime(TimeOnly.MinValue),
            animal.Peso,
            Sexo = ConverterSexoParaBanco(animal.Sexo),
            animal.TutorId,
            animal.RacaId
        });

        return affected > 0;
    }

    public async Task<bool> Remover(long id)
    {
        const string sql = "DELETE FROM animal WHERE id = @Id";
        using var conn = _connectionFactory.CreateConnection();
        var affected = await conn.ExecuteAsync(sql, new { Id = id });
        return affected > 0;
    }

    public async Task<IEnumerable<Animal>> ObterPorTutorId(long tutorId)
    {
        const string sql = """
            SELECT id, nome, data_nascimento AS DataNascimento, peso, sexo,
                   tutor_id AS TutorId, raca_id AS RacaId
            FROM animal
            WHERE tutor_id = @TutorId
            ORDER BY nome
            """;

        using var conn = _connectionFactory.CreateConnection();
        var rows = await conn.QueryAsync(sql, new { TutorId = tutorId });
        return rows.Select(MapearAnimal);
    }

    private static Animal MapearAnimal(dynamic row)
    {
        var sexo = ConverterSexoDoBanco((string)row.sexo);

        var animal = new Animal(
            (string)row.nome,
            ConverterDataNascimento(row.datanascimento),
            (decimal)row.peso,
            sexo,
            (long)row.tutorid,
            (long)row.racaid
        );

        animal.SetId((long)row.id);
        animal.SetCreatedAt(DateTime.UtcNow);

        return animal;
    }

    private static string ConverterSexoParaBanco(Sexo sexo)
    {
        return sexo == Sexo.Macho ? "M" : "F";
    }

    private static Sexo ConverterSexoDoBanco(string valor)
    {
        return valor.Trim().ToUpperInvariant() switch
        {
            "M" or "MACHO" => Sexo.Macho,
            "F" or "FEMEA" or "FÊMEA" => Sexo.Femea,
            _ => throw new ArgumentException($"Sexo inválido no banco: {valor}")
        };
    }

    private static DateOnly ConverterDataNascimento(object valor)
    {
        return valor switch
        {
            DateOnly dateOnly => dateOnly,
            DateTime dateTime => DateOnly.FromDateTime(dateTime),
            _ => DateOnly.Parse(valor.ToString()!)
        };
    }
}
