# 📊 Framework de Mapeamento: RF → Camadas → Tasks

Este documento serve como guia para o time de desenvolvimento na decomposição de requisitos funcionais em camadas arquiteturais, garantindo que todas as etapas da implementação sejam cobertas.

---

## Exemplo Completo: RF03 (Agendamento de Consultas)

### Análise Arquitetural do RF03
**RF03:** O sistema deve permitir o agendamento de consultas veterinárias.

```text
┌─────────────────────────────────────────────────────────┐
│ IMPACTO ARQUITETURAL                                    │
├─────────────────────────────────────────────────────────┤
│ ✓ Camada de Dados       → Precisa de entidade/tabela   │
│ ✓ Camada de Negócio     → Precisa de classe/serviço    │
│ ✓ Camada de Aplicação   → Precisa de controller        │
│ ✓ Camada de Apresentação → Precisa de tela/formulário  │
└─────────────────────────────────────────────────────────┘
```

---

## Decomposição em Tasks por Camada

### Card #9 — Modelo Conceitual (Etapa 3 - Dados)
#### Subtasks — Requisitos que impactam o DER:

- [ ] **RF03** — Criar entidade `Agendamento`
      - Atributos: `id`, `data_hora`, `status`, `observacoes`
      - Relacionamentos: N:1 com `Tutor`, N:1 com `Animal`, N:1 com `Veterinario`
- [ ] **RF01** — Criar entidade `Tutor`
      - Atributos: `id`, `nome`, `cpf`, `telefone`, `email`, `endereco`
- [ ] **RF02** — Criar entidade `Animal`
      - Atributos: `id`, `nome`, `especie`, `raca`, `sexo`, `data_nascimento`, `peso`
- [ ] **RF04** — Adicionar constraint `UNIQUE` em `Agendamento`
      - Garantir que (`id_veterinario` + `data_hora`) seja único
- [ ] **RF05** — Criar entidade `Prontuario`
      - Relacionamento: N:1 com `Agendamento`

---

### Card #10 — Modelo Lógico (Etapa 3 - Dados)
#### Subtasks — Scripts SQL:

- [ ] **RF01** — Script DDL tabela `Tutor`
```sql
CREATE TABLE Tutor (
    id_tutor INT PRIMARY KEY IDENTITY(1,1),
    nome NVARCHAR(100) NOT NULL,
    cpf CHAR(11) UNIQUE NOT NULL,
    telefone VARCHAR(15),
    email VARCHAR(100),
    endereco NVARCHAR(200)
);
```
- [ ] **RF02** — Script DDL tabela `Animal`
- [ ] **RF03** — Script DDL tabela `Agendamento`
- [ ] **RF04** — Stored procedure `sp_ValidarConflito`
```sql
CREATE PROCEDURE sp_ValidarConflito
    @id_veterinario INT,
    @data_hora DATETIME
AS
BEGIN
    SELECT COUNT(*) FROM Agendamento
    WHERE id_veterinario = @id_veterinario
    AND data_hora = @data_hora
    AND status != 'Cancelado';
END
```
- [ ] **RF03** — Script de INSERT de dados de teste

---

### Card #14 — POO com C# (Etapa 4 - Negócio)
#### Subtasks — Classes e Serviços:

- [ ] **RF01** — Classe `Tutor.cs`
```csharp
public class Tutor
{
    public int Id { get; set; }
    public string Nome { get; set; }
    public string CPF { get; set; }
    public string Telefone { get; set; }
    public string Email { get; set; }
    public List<Animal> Animais { get; set; }
}
```
- [ ] **RF02** — Classe `Animal.cs`
- [ ] **RF03** — Classe `Agendamento.cs`
- [ ] **RF03** — Classe `AgendamentoService.cs` com método `CriarAgendamento()`
- [ ] **RF04** — Método `ValidarConflito()` no `AgendamentoService`
```csharp
public bool ValidarConflito(int idVeterinario, DateTime dataHora)
{
    // Lógica de validação usando LINQ ou chamada à SP
    return !_context.Agendamentos.Any(a => 
        a.IdVeterinario == idVeterinario && 
        a.DataHora == dataHora &&
        a.Status != "Cancelado");
}
```

---

### Card #13 — Arquitetura Geral (Etapa 4 - Aplicação)
#### Subtasks — Controllers:

- [ ] **RF03** — Criar `AgendamentoController.cs`
      - Endpoint `POST /api/agendamento`
      - Endpoint `GET /api/agendamento/{id}`
      - Endpoint `GET /api/agendamento/veterinario/{id}/dia/{data}`
- [ ] **RF01** — Criar `TutorController.cs`
- [ ] **RF02** — Criar `AnimalController.cs`

---

### Card #17 — Telas HTML (Etapa 5 - Apresentação)
#### Subtasks — Interfaces:

- [ ] **RF03** — Criar `agendamento.html`
      - Formulário: seletor de tutor, animal, veterinário, data/hora
      - JavaScript para validar conflito antes de enviar
- [ ] **RF01** — Criar `cadastro-tutor.html`
- [ ] **RF02** — Criar `cadastro-animal.html`
- [ ] **RF03** — Criar `agenda-dia.html` (visualização da agenda)
