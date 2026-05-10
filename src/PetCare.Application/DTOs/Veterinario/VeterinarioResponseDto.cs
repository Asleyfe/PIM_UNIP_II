namespace PetCare.Application.DTOs.Veterinario;

public class VeterinarioResponseDto
{
    public long Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Crmv { get; set; } = string.Empty;
    public string? Telefone { get; set; }
    public string? Email { get; set; }
    public DateTime CreatedAt { get; set; }
}
