namespace PetCare.Application.DTOs.Veterinario;

public class VeterinarioCreateDto
{
    public string Nome { get; set; } = string.Empty;
    public string Crmv { get; set; } = string.Empty;
    public string? Telefone { get; set; }
    public string? Email { get; set; }
}
