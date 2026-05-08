namespace PetCare.Application.DTOs.Animal;

public class AnimalResponseDto
{
    public long Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public DateOnly DataNascimento { get; set; }
    public decimal Peso { get; set; }
    public string Sexo { get; set; } = string.Empty;
    public long TutorId { get; set; }
    public long RacaId { get; set; }
    public int IdadeAnos { get; set; }
    public bool EhFilhote { get; set; }
}

public class AnimalCreateDto
{
    public string Nome { get; set; } = string.Empty;
    public DateOnly DataNascimento { get; set; }
    public decimal Peso { get; set; }
    public string Sexo { get; set; } = string.Empty; // "M" ou "F"
    public long TutorId { get; set; }
    public long RacaId { get; set; }
}