using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using PetCare.Domain.Entities.Pessoas;
using PetCare.Domain.ValueObjects;
using PetCare.Infrastructure.Configuration;
using PetCare.Infrastructure.Data;
using PetCare.Infrastructure.Repositories;


namespace PetCare.Tests.Infrastructure;

/// <summary>
/// Testes de integração: conectam ao banco real (Supabase).
/// Requerem a tabela 'tutor' criada no PostgreSQL.
/// </summary>
public class TutorRepositoryIntegrationTests
{
    private readonly TutorRepository _repository;

    public TutorRepositoryIntegrationTests()
    {
        var config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.test.json", optional: false)
            .Build();

        var connectionString = config.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("Connection string não encontrada em appsettings.test.json");

        var factory = new NpgsqlConnectionFactory(
            Options.Create(new SupabaseSettings { ConnectionString = connectionString }));
        _repository = new TutorRepository(factory);
    }

    [Fact(DisplayName = "Deve conectar ao banco sem lançar exceção")]
    public async Task DeveConectarAoBanco()
    {
        // Arrange & Act
        var act = async () => await _repository.Listar();

        // Assert
        await act.Should().NotThrowAsync();
    }

    [Fact(DisplayName = "Listar deve retornar lista (vazia ou não)")]
    public async Task ListarDeveRetornarLista()
    {
        // Act
        var tutores = await _repository.Listar();

        // Assert
        tutores.Should().NotBeNull();
    }

    [Fact(DisplayName = "ObterPorId com id inexistente deve retornar null")]
    public async Task ObterPorIdInexistenteDeveRetornarNull()
    {
        // Act
        var tutor = await _repository.ObterPorId(999999);

        // Assert
        tutor.Should().BeNull();
    }

    [Fact(DisplayName = "ExistePorCpf com CPF inexistente deve retornar false")]
    public async Task ExistePorCpfInexistenteDeveRetornarFalse()
    {
        // Act
        var existe = await _repository.ExistePorCpf("00000000000");

        // Assert
        existe.Should().BeFalse();
    }

    [Fact(DisplayName = "Deve inserir um tutor e recuperá-lo do banco")]
    public async Task DeveInserirTutorERecuperarDoBanco()
    {
        // Arrange
        var endereco = new Endereco("Rua T-30", "100", "Setor Bueno", "Goiânia", "GO");
        var tutor = new Tutor(
            nome: "João da Silva Teste",
            cpf: "52998224725",        // CPF válido usado apenas em testes
            telefone: "62999990000",
            email: "joao.teste@petcare.com",
            endereco: endereco
        );

        long idGerado = 0;
        try
        {
            // Act — inserir
            var inserido = await _repository.Inserir(tutor);

            // Assert — banco gerou Id e CreatedAt
            inserido.Id.Should().BeGreaterThan(0);
            inserido.CreatedAt.Should().NotBe(default);
            inserido.Nome.Should().Be("João da Silva Teste");
            inserido.Cpf.Should().Be("52998224725");
            inserido.Email.Should().Be("joao.teste@petcare.com");

            idGerado = inserido.Id;

            // Assert — consegue recuperar pelo Id
            var recuperado = await _repository.ObterPorId(idGerado);
            recuperado.Should().NotBeNull();
            recuperado!.Nome.Should().Be(inserido.Nome);
            recuperado.Endereco.Cidade.Should().Be("Goiânia");

            // Assert — CPF agora existe no banco
            var existe = await _repository.ExistePorCpf("52998224725");
            existe.Should().BeTrue();
        }
        finally
        {
            // Limpeza: remove o registro para o teste ser repetível
            if (idGerado > 0)
                await _repository.Remover(idGerado);
        }
    }
}
