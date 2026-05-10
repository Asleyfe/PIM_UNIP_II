namespace PetCare.Application.DTOs.Atendimento;

public class ProntuarioCreateDto
{
    public long AgendamentoId { get; set; }
    public string QueixaPrincipal { get; set; } = string.Empty;
    public string? RelatoTutor { get; set; }
    public string? ExameFisico { get; set; }
    public string? Diagnostico { get; set; }
    public string? Prescricao { get; set; }
    public string? Observacoes { get; set; }
}
