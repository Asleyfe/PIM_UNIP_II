namespace PetCare.Application.DTOs.Comunicacao;

public class LembreteResponseDto
{
    public long Id { get; set; }
    public long TutorId { get; set; }
    public long AnimalId { get; set; }
    public long? AgendamentoId { get; set; }
    public string Tipo { get; set; } = string.Empty;
    public string MeioEnvio { get; set; } = string.Empty;
    public string StatusEnvio { get; set; } = string.Empty;
    public DateTime DataEnvio { get; set; }
    public string? Mensagem { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class LembreteCreateDto
{
    public long TutorId { get; set; }
    public long AnimalId { get; set; }
    public long? AgendamentoId { get; set; }
    public string Tipo { get; set; } = string.Empty;
    public string MeioEnvio { get; set; } = string.Empty;
    public string? Mensagem { get; set; }
}
