using Dapper;
using PetCare.Domain.Entities.Animais;
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
        const string sql = "SELECT * FROM animal WHERE id = @Id";
        using var conn = _connectionFactory.CreateConnection();
        return await conn.QuerySingleOrDefaultAsync<Animal>(sql, new { Id = id });
    }

    public async Task<IEnumerable<Animal>> Listar()
    {
        const string sql = "SELECT * FROM animal ORDER BY nome";
        using var conn = _connectionFactory.CreateConnection();
        return await conn.QueryAsync<Animal>(sql);
    }

    public async Task<Animal> Inserir(Animal animal)
    {
        const string sql = """
            INSERT INTO animal (nome, data_nascimento, peso, sexo, tutor_id, raca_id)
            VALUES (@Nome, @DataNascimento, @Peso, @Sexo, @TutorId, @RacaId)
            RETURNING id, created_at AS CreatedAt
            """;

        using var conn = _connectionFactory.CreateConnection();
        var result = await conn.QuerySingleAsync(sql, new
        {
            animal.Nome,
            animal.DataNascimento,
            animal.Peso,
            Sexo = animal.Sexo.ToString(),
            animal.TutorId,
            animal.RacaId
        });

        animal.SetId((long)result.id);
        animal.SetCreatedAt((DateTime)result.createdat);

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
            animal.DataNascimento,
            animal.Peso,
            Sexo = animal.Sexo.ToString(),
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
        const string sql = "SELECT * FROM animal WHERE tutor_id = @TutorId ORDER BY nome";
        using var conn = _connectionFactory.CreateConnection();
        return await conn.QueryAsync<Animal>(sql, new { TutorId = tutorId });
    }
}