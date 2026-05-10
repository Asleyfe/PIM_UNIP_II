namespace PetCare.Application.DTOs.Agendamento;

public class AgendamentoResponseDto
{
    public long Id { get; set; }
    public long TutorId { get; set; }
    public string TutorNome { get; set; } = string.Empty;
    public long AnimalId { get; set; }
    public string AnimalNome { get; set; } = string.Empty;
    public long VeterinarioId { get; set; }
    public string VeterinarioNome { get; set; } = string.Empty;
    public DateTime DataHoraConsulta { get; set; }
    public string Status { get; set; } = string.Empty;
    public string? Observacao { get; set; }
    public DateTime CreatedAt { get; set; }
}
