using System.Diagnostics;
using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using PetCare.Infrastructure.Configuration;
using PetCare.Infrastructure.Data;
using PetCare.Infrastructure.Repositories;
using Xunit.Abstractions;

namespace PetCare.Tests.Infrastructure;

public class DatabaseReadDiagnosticsTests
{
    private static readonly TimeSpan QueryTimeout = TimeSpan.FromSeconds(12);
    private readonly NpgsqlConnectionFactory _factory;
    private readonly ITestOutputHelper _output;

    public DatabaseReadDiagnosticsTests(ITestOutputHelper output)
    {
        _output = output;

        var config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.test.json", optional: false)
            .Build();

        var connectionString = Environment.GetEnvironmentVariable("PETCARE_DIAGNOSTIC_CONNECTION")
            ?? config.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("Connection string não encontrada em appsettings.test.json");

        _factory = new NpgsqlConnectionFactory(
            Options.Create(new SupabaseSettings { ConnectionString = connectionString }));
    }

    [Fact(DisplayName = "Diagnóstico read-only do banco deve listar tempo de todas as consultas")]
    public async Task DiagnosticoReadOnlyDoBanco()
    {
        var failures = new List<string>();
        var hoje = DateOnly.FromDateTime(DateTime.Now);
        var inicio = DateTime.UtcNow.AddDays(-30);
        var fim = DateTime.UtcNow;

        await Check("Banco.SELECT 1", async () =>
        {
            using var connection = _factory.CreateConnection();
            await connection.ExecuteScalarAsync<int>("SELECT 1");
        }, failures);

        await Check("Tutor.Listar", async () => await new TutorRepository(_factory).Listar(), failures);
        await Check("Tutor.BuscarPorTermo", async () => await new TutorRepository(_factory).BuscarPorTermo("Ricardo"), failures);
        await Check("Animal.Listar", async () => await new AnimalRepository(_factory).Listar(), failures);
        await Check("Veterinario.Listar", async () => await new VeterinarioRepository(_factory).Listar(), failures);
        await Check("Produto.Listar", async () => await new ProdutoRepository(_factory).Listar(), failures);
        await Check("Produto.ListarComEstoqueBaixo", async () => await new ProdutoRepository(_factory).ListarComEstoqueBaixo(), failures);
        await Check("Produto.ListarProximosDoVencimento", async () => await new ProdutoRepository(_factory).ListarProximosDoVencimento(), failures);
        await Check("Agendamento.Listar", async () => await new AgendamentoRepository(_factory).Listar(), failures);
        await Check("Agendamento.ListarAgendaDoDia", async () => await new AgendamentoRepository(_factory).ListarAgendaDoDia(hoje), failures);
        await Check("Prontuario.Listar", async () => await new ProntuarioRepository(_factory).Listar(), failures);
        await Check("Historico.Listar", async () => await new HistoricoRepository(_factory).Listar(), failures);
        await Check("Lembrete.Listar", async () => await new LembreteRepository(_factory).Listar(), failures);
        await Check("MovimentacaoEstoque.Listar", async () => await new MovimentacaoEstoqueRepository(_factory).Listar(), failures);
        await Check("Venda.Listar", async () => await new VendaRepository(_factory).Listar(), failures);
        await Check("Venda.ListarPorPeriodo", async () => await new VendaRepository(_factory).ListarPorPeriodo(inicio, fim), failures);

        var relatorios = new RelatorioRepository(_factory);
        await Check("Relatorio.ObterFaturamentoTotal", async () => await relatorios.ObterFaturamentoTotal(inicio, fim), failures);
        await Check("Relatorio.ObterFaturamentoPorCategoria", async () => await relatorios.ObterFaturamentoPorCategoria(inicio, fim), failures);
        await Check("Relatorio.ObterFaturamentoMensal", async () => await relatorios.ObterFaturamentoMensal(inicio, fim), failures);
        await Check("Relatorio.ObterDesempenhoProfissionais", async () => await relatorios.ObterDesempenhoProfissionais(inicio, fim), failures);
        await Check("Relatorio.ObterProdutosMaisVendidos", async () => await relatorios.ObterProdutosMaisVendidos(inicio, fim, 10), failures);

        Assert.True(
            failures.Count == 0,
            "Consultas com falha/timeout:" + Environment.NewLine + string.Join(Environment.NewLine, failures));
    }

    private async Task Check(string name, Func<Task> action, List<string> failures)
    {
        var stopwatch = Stopwatch.StartNew();
        try
        {
            var task = action();
            var completed = await Task.WhenAny(task, Task.Delay(QueryTimeout));
            stopwatch.Stop();

            if (completed != task)
            {
                var message = $"{name}: TIMEOUT após {QueryTimeout.TotalSeconds:n0}s";
                failures.Add(message);
                _output.WriteLine(message);
                return;
            }

            await task;
            _output.WriteLine($"{name}: OK em {stopwatch.ElapsedMilliseconds}ms");
        }
        catch (Exception ex)
        {
            stopwatch.Stop();
            var message = $"{name}: ERRO em {stopwatch.ElapsedMilliseconds}ms - {ex.GetType().Name}: {ex.Message}";
            failures.Add(message);
            _output.WriteLine(message);
        }
    }

    private async Task Check<T>(string name, Func<Task<T>> action, List<string> failures)
    {
        await Check(name, async () => { _ = await action(); }, failures);
    }
}
