namespace PetCare.Application.DTOs.Atendimento;

public class ProntuarioResponseDto
{
    public long Id { get; set; }
    public long AgendamentoId { get; set; }
    public string QueixaPrincipal { get; set; } = string.Empty;
    public string? RelatoTutor { get; set; }
    public string? ExameFisico { get; set; }
    public string? Diagnostico { get; set; }
    public string? Prescricao { get; set; }
    public string? Observacoes { get; set; }
    public DateTime DataRegistro { get; set; }
    public DateTime CreatedAt { get; set; }
}
