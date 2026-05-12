namespace PetCare.Infrastructure.Configuration;

/// <summary>
/// Configurações do Supabase carregadas via IOptions a partir do appsettings.json.
/// Permite mudar credenciais sem recompilar.
/// </summary>
public class SupabaseSettings
{
    /// <summary>Nome da seção no appsettings.json. Usado no Program.cs.</summary>
    public const string SectionName = "Supabase";

    /// <summary>Connection string completa no formato Npgsql.</summary>
    public string ConnectionString { get; set; } = string.Empty;

    /// <summary>URL do projeto Supabase.</summary>
    public string Url { get; set; } = string.Empty;

    /// <summary>Chave pública anon do Supabase.</summary>
    public string AnonKey { get; set; } = string.Empty;

    /// <summary>Segredo JWT (JWT Secret) do Supabase para validar tokens no Back End.</summary>
    public string JwtSecret { get; set; } = string.Empty;
}