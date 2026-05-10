using Dapper;
using PetCare.Domain.Entities.Pessoas;
using PetCare.Domain.Interfaces.Repositories;
using PetCare.Domain.ValueObjects;
using PetCare.Infrastructure.Data;

namespace PetCare.Infrastructure.Repositories;

public class TutorRepository : ITutorRepository
{
    private readonly IConnectionFactory _connectionFactory;

    public TutorRepository(IConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<Tutor?> ObterPorId(long id)
    {
        const string sql = """
            SELECT id, nome, cpf, telefone, email,
                   rua, numero, bairro, cidade, estado,
                   created_at AS CreatedAt
            FROM tutor
            WHERE id = @Id
            """;

        using var connection = _connectionFactory.CreateConnection();
        var row = await connection.QuerySingleOrDefaultAsync(sql, new { Id = id });
        return row is null ? null : MapearTutor(row);
    }

    public async Task<IEnumerable<Tutor>> Listar()
    {
        const string sql = """
            SELECT id, nome, cpf, telefone, email,
                   rua, numero, bairro, cidade, estado,
                   created_at AS CreatedAt
            FROM tutor
            ORDER BY nome
            """;

        using var connection = _connectionFactory.CreateConnection();
        var rows = await connection.QueryAsync(sql);
        return rows.Select(MapearTutor);
    }

    public async Task<Tutor> Inserir(Tutor tutor)
    {
        const string sql = """
            INSERT INTO tutor (nome, cpf, telefone, email, rua, numero, bairro, cidade, estado)
            VALUES (@Nome, @Cpf, @Telefone, @Email, @Rua, @Numero, @Bairro, @Cidade, @Estado)
            RETURNING id, created_at AS CreatedAt
            """;

        using var connection = _connectionFactory.CreateConnection();
        var resultado = await connection.QuerySingleAsync(sql, new
        {
            tutor.Nome,
            tutor.Cpf,
            tutor.Telefone,
            tutor.Email,
            tutor.Endereco.Rua,
            tutor.Endereco.Numero,
            tutor.Endereco.Bairro,
            tutor.Endereco.Cidade,
            tutor.Endereco.Estado
        });

        // preenche Id e CreatedAt gerados pelo banco
        tutor.SetId((long)resultado.id);
        tutor.SetCreatedAt((DateTime)resultado.createdat);

        return tutor;
    }

    public async Task<bool> Atualizar(Tutor tutor)
    {
        const string sql = """
            UPDATE tutor
            SET nome = @Nome, telefone = @Telefone, email = @Email,
                rua = @Rua, numero = @Numero, bairro = @Bairro,
                cidade = @Cidade, estado = @Estado
            WHERE id = @Id
            """;

        using var connection = _connectionFactory.CreateConnection();
        var linhasAfetadas = await connection.ExecuteAsync(sql, new
        {
            tutor.Id,
            tutor.Nome,
            tutor.Telefone,
            tutor.Email,
            tutor.Endereco.Rua,
            tutor.Endereco.Numero,
            tutor.Endereco.Bairro,
            tutor.Endereco.Cidade,
            tutor.Endereco.Estado
        });

        return linhasAfetadas > 0;
    }

    public async Task<bool> Remover(long id)
    {
        const string sql = "DELETE FROM tutor WHERE id = @Id";

        using var connection = _connectionFactory.CreateConnection();
        var linhasAfetadas = await connection.ExecuteAsync(sql, new { Id = id });
        return linhasAfetadas > 0;
    }

    public async Task<bool> ExistePorCpf(string cpf)
    {
        const string sql = "SELECT COUNT(1) FROM tutor WHERE cpf = @Cpf";

        using var connection = _connectionFactory.CreateConnection();
        var count = await connection.ExecuteScalarAsync<int>(sql, new { Cpf = cpf });
        return count > 0;
    }

    public async Task<IEnumerable<Tutor>> BuscarPorTermo(string termo)
    {
        const string sql = """
            SELECT id, nome, cpf, telefone, email,
                   rua, numero, bairro, cidade, estado,
                   created_at AS CreatedAt
            FROM tutor
            WHERE nome ILIKE @Termo OR cpf = @Termo OR telefone = @Termo
            ORDER BY nome
            """;

        using var connection = _connectionFactory.CreateConnection();
        var rows = await connection.QueryAsync(sql, new { Termo = $"%{termo}%" });
        return rows.Select(MapearTutor);
    }

    private static Tutor MapearTutor(dynamic row)
    {
        var endereco = new Endereco(
            (string)row.rua,
            (string)row.numero,
            (string)row.bairro,
            (string)row.cidade,
            (string)row.estado
        );

        var tutor = new Tutor(
            (string)row.nome,
            (string)row.cpf,
            (string)row.telefone,
            (string)row.email,
            endereco
        );

        tutor.SetId((long)row.id);
        tutor.SetCreatedAt((DateTime)row.createdat);

        return tutor;
    }
}
