using PetCare.API.Middleware;
using PetCare.Infrastructure.Configuration;
using PetCare.Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);

// =============================================================================
// CONFIGURAÇÃO DO DAPPER (chamada única, antes de qualquer query)
// =============================================================================
DapperSetup.Configure();

// =============================================================================
// CONFIGURAÇÕES (appsettings)
// =============================================================================
builder.Services.Configure<SupabaseSettings>(
    builder.Configuration.GetSection(SupabaseSettings.SectionName));

// =============================================================================
// INFRAESTRUTURA — Conexão com banco
// =============================================================================
builder.Services.AddSingleton<IConnectionFactory, NpgsqlConnectionFactory>();

// =============================================================================
// REPOSITÓRIOS — (vão entrar aqui conforme implementarmos)
// builder.Services.AddScoped<ITutorRepository, TutorRepository>();
// =============================================================================

// =============================================================================
// SERVICES (Application) — (entram aqui conforme implementarmos)
// builder.Services.AddScoped<ITutorService, TutorService>();
// =============================================================================

// =============================================================================
// API
// =============================================================================
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// =============================================================================
// PIPELINE HTTP
// =============================================================================

// Middleware de tratamento global de erros (PRIMEIRO!)
app.UseMiddleware<TratamentoErrosMiddleware>();

// Swagger (só em desenvolvimento)
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Servir arquivos estáticos (wwwroot/) — front-end vai aqui na Sprint 3
app.UseDefaultFiles();
app.UseStaticFiles();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();