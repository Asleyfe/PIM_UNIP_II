# 📋 Product Backlog - PIM UNIP II

Este documento contém a lista de requisitos funcionais decompostos para facilitar o planejamento das sprints e o acompanhamento das entregas.

---

## 🛠️ Requisitos da Sprint Atual

### RF01 — Cadastro de Tutores
**Prioridade:** Alta | **Sprint:** 2 | **MoSCoW:** Must have

#### 📝 Descrição:
O sistema deve permitir o cadastro completo de tutores com os seguintes campos obrigatórios: nome, CPF, telefone, e-mail e endereço. O sistema deve validar o formato do CPF e não permitir duplicação.

#### ✅ Critérios de Aceite:
- [ ] Todos os campos obrigatórios devem ser validados.
- [ ] CPF deve seguir o formato válido (11 dígitos).
- [ ] Sistema deve impedir cadastro de CPF duplicado.
- [ ] Mensagem de sucesso/erro deve ser exibida ao usuário.
- [ ] Armazenar CPF sem mascaras (apenas numeros).
- [ ] Armazenar telefone sem mascaras (apenas numeros).

#### 🏗️ Impacto Arquitetural:
- **Camada de Dados (Etapa 3)** → Card #9 e #10
- **Camada de Negócio (Etapa 4)** → Card #14
- **Camada de Aplicação (Etapa 4)** → Card #13
- **Camada de Apresentação (Etapa 5)** → Card #17

#### 📊 Decomposição de Tarefas:

| Task | Descrição | Card | Est. | Responsável |
| :--- | :--- | :---: | :---: | :--- |
| **RF01-T1** | Criar entidade `Tutor` no DER com atributos | #9 | 1h | _________ |
| **RF01-T2** | Script DDL tabela `Tutor` com constraints | #10 | 1h | _________ |
| **RF01-T3** | Script de validação de CPF (função SQL) | #10 | 2h | _________ |
| **RF01-T4** | Classe `Tutor.cs` com encapsulamento | #14 | 2h | _________ |
| **RF01-T5** | Classe `TutorService.cs` com validações | #14 | 3h | _________ |
| **RF01-T6** | Controller `TutorController.cs` (POST/GET) | #13 | 2h | _________ |
| **RF01-T7** | Tela `cadastro-tutor.html` responsiva | #17 | 4h | _________ |
| **RF01-T8** | Validação de formulário em JavaScript | #17 | 2h | _________ |

**⏱️ Total estimado:** 17 horas

---

### RF03 — Agendamento de Consultas
**Prioridade:** Alta | **Sprint:** 3 | **MoSCoW:** Must have

#### 📝 Descrição:
O sistema deve permitir o agendamento de consultas veterinárias, garantindo que não haja conflitos de horário para o mesmo veterinário.

#### ✅ Critérios de Aceite:
- [ ] Permitir seleção de Tutor, Animal e Veterinário.
- [ ] Validar se o veterinário possui disponibilidade no horário escolhido.
- [ ] Registrar data, hora e observações do agendamento.
- [ ] Exibir visualização de agenda diária.

#### 🏗️ Impacto Arquitetural:
- **Camada de Dados (Etapa 3)** → Card #9 e #10
- **Camada de Negócio (Etapa 4)** → Card #14
- **Camada de Aplicação (Etapa 4)** → Card #13
- **Camada de Apresentação (Etapa 5)** → Card #17

#### 📊 Decomposição de Tarefas:

| Task | Descrição | Card | Est. | Responsável |
| :--- | :--- | :---: | :---: | :--- |
| **RF03-T1** | Criar entidade `Agendamento` no DER | #9 | 1h | _________ |
| **RF03-T2** | Script DDL tabela `Agendamento` | #10 | 1h | _________ |
| **RF03-T3** | SP `sp_ValidarConflito` (Banco de Dados) | #10 | 2h | _________ |
| **RF03-T4** | Classe `Agendamento.cs` (Model) | #14 | 1h | _________ |
| **RF03-T5** | `AgendamentoService.cs` (Lógica de conflito) | #14 | 3h | _________ |
| **RF03-T6** | Controller `AgendamentoController.cs` | #13 | 2h | _________ |
| **RF03-T7** | Tela `agendamento.html` (Formulário) | #17 | 4h | _________ |
| **RF03-T8** | Tela `agenda-dia.html` (Visualização) | #17 | 4h | _________ |

**⏱️ Total estimado:** 18 horas