namespace PetCare.Application.DTOs.Atendimento;

public class HistoricoClinicoResponseDto
{
    public long Id { get; set; }
    public long AnimalId { get; set; }
    public long VeterinarioId { get; set; }
    public string Descricao { get; set; } = string.Empty;
    public DateTime DataRegistro { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class HistoricoClinicoCreateDto
{
    public long AnimalId { get; set; }
    public long VeterinarioId { get; set; }
    public string Descricao { get; set; } = string.Empty;
}
