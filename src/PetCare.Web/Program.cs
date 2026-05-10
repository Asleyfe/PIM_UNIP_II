using PetCare.Web.Middleware;
using PetCare.Infrastructure.Configuration;
using PetCare.Infrastructure.Data;
using PetCare.Domain.Interfaces.Repositories;
using PetCare.Infrastructure.Repositories;
using PetCare.Application.Services.Interfaces;
using PetCare.Application.Services;

var builder = WebApplication.CreateBuilder(args);

// Configuração do Dapper
DapperSetup.Configure();

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configurações Supabase
builder.Services.Configure<SupabaseSettings>(
    builder.Configuration.GetSection(SupabaseSettings.SectionName));

// Injeção de Dependência - Infra
builder.Services.AddSingleton<IConnectionFactory, NpgsqlConnectionFactory>();
builder.Services.AddScoped<ITutorRepository, TutorRepository>();
builder.Services.AddScoped<IAnimalRepository, AnimalRepository>();
builder.Services.AddScoped<IVeterinarioRepository, VeterinarioRepository>();
builder.Services.AddScoped<IAgendamentoRepository, AgendamentoRepository>();
builder.Services.AddScoped<IProdutoRepository, ProdutoRepository>();
builder.Services.AddScoped<IMovimentacaoEstoqueRepository, MovimentacaoEstoqueRepository>();
builder.Services.AddScoped<IProntuarioRepository, ProntuarioRepository>();
builder.Services.AddScoped<IVendaRepository, VendaRepository>();

// Injeção de Dependência - Application
builder.Services.AddScoped<ITutorService, TutorService>();
builder.Services.AddScoped<IAnimalService, AnimalService>();
builder.Services.AddScoped<IVeterinarioService, VeterinarioService>();
builder.Services.AddScoped<IAgendamentoService, AgendamentoService>();
builder.Services.AddScoped<IEstoqueService, EstoqueService>();
builder.Services.AddScoped<IProntuarioService, ProntuarioService>();
builder.Services.AddScoped<IVendaService, VendaService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseMiddleware<TratamentoErrosMiddleware>();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}
else
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllers();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
