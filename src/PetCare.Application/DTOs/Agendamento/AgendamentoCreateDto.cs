namespace PetCare.Application.DTOs.Agendamento;

public class AgendamentoCreateDto
{
    public long TutorId { get; set; }
    public long AnimalId { get; set; }
    public long VeterinarioId { get; set; }
    public DateTime DataHoraConsulta { get; set; }
    public decimal Preco { get; set; }
    public string? Observacao { get; set; }
}
