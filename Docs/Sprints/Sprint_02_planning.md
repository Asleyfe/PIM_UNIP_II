# 🗓️ Sprint Planning — Sprint 2
> **PIM III — Sistema PetCare Goiânia**  
> **Data:** Segunda-feira, 28 de abril de 2026  
> **Duração:** 2 horas (19h00 — 21h00)

---

## 📊 Informações da Sprint

| Item | Detalhes |
|------|----------|
| **Sprint** | Sprint 2 |
| **Período** | 28/04/2026 a 03/05/2026 (6 dias, 3 úteis) |
| **Objetivo da Sprint** | Implementar a base de dados e as funcionalidades core do sistema (cadastros e agendamento) |
| **Capacidade do Time** | 4 pessoas × 5 horas = **20 horas** disponíveis |
| **Product Owner** | Carlos Mendes (empresário fictício — PetCare Goiânia) |
| **Scrum Master** | [Seu nome] |
| **Development Team** | [Nomes dos 3-4 membros] |

---

## 🎯 Meta da Sprint (Sprint Goal)

> **"Ao final desta Sprint, o sistema PetCare deve permitir cadastrar tutores e animais, e realizar agendamentos de consultas com validação de conflito de horário, com dados persistidos no banco SQL Server."**

### Critérios de Sucesso:
- ✅ Banco de dados modelado e criado (DER + DDL)
- ✅ Classes C# principais implementadas com POO
- ✅ É possível cadastrar um tutor e um animal no sistema
- ✅ É possível agendar uma consulta e o sistema impede conflitos

---

## 📦 Product Backlog Items Selecionados

| ID | Requisito Funcional | Prioridade | Story Points | Justificativa |
|----|---------------------|------------|--------------|---------------|
| **RF01** | Cadastro de Tutores | Alta | 8 | Base do sistema — sem tutor não há agendamento |
| **RF02** | Cadastro de Animais | Alta | 5 | Vinculado ao tutor — essencial para consultas |
| **RF03** | Agendamento de Consultas | Alta | 13 | Funcionalidade core do negócio |
| **RF04** | Validação de Conflito de Horário | Alta | 3 | Integrado ao RF03 — regra crítica |
| **RF10** | Busca de Tutores | Média | 5 | Facilita operação da recepcionista |

**Total:** 34 Story Points

---

## 🛠️ Decomposição em Tasks Técnicas

### 📘 RF01 — Cadastro de Tutores

**Critérios de Aceite:**
- ✓ Todos os campos obrigatórios validados (nome, CPF, telefone, email)
- ✓ CPF único no sistema (não permitir duplicação)
- ✓ Formato de CPF validado (11 dígitos numéricos)
- ✓ Mensagem de sucesso/erro exibida ao usuário

**Decomposição:**

| Task ID | Descrição | Card | Responsável | Estimativa | Status |
|---------|-----------|------|-------------|------------|--------|
| RF01-T1 | Criar entidade `Tutor` no DER com atributos | #9 | @maria | 1h | 📋 To Do |
| RF01-T2 | Script DDL tabela `Tutor` com constraints | #10 | @joao | 1h | 📋 To Do |
| RF01-T3 | Script de validação de CPF (função SQL) | #10 | @maria | 2h | 📋 To Do |
| RF01-T4 | Classe `Tutor.cs` com encapsulamento | #14 | @pedro | 2h | 📋 To Do |
| RF01-T5 | Classe `TutorService.cs` com validações | #14 | @pedro | 3h | 📋 To Do |

**Subtotal RF01:** 9 horas

---

### 🐾 RF02 — Cadastro de Animais

**Critérios de Aceite:**
- ✓ Animal vinculado obrigatoriamente a um tutor
- ✓ Data de nascimento não pode ser futura
- ✓ Peso numérico e maior que zero
- ✓ Múltiplos animais para o mesmo tutor

**Decomposição:**

| Task ID | Descrição | Card | Responsável | Estimativa | Status |
|---------|-----------|------|-------------|------------|--------|
| RF02-T1 | Criar entidade `Animal` no DER | #9 | @maria | 1h | 📋 To Do |
| RF02-T2 | Definir relacionamento Tutor 1:N Animal | #9 | @maria | 30min | 📋 To Do |
| RF02-T3 | Script DDL tabela `Animal` com FK | #10 | @joao | 1h | 📋 To Do |
| RF02-T4 | Classe `Animal.cs` com propriedades | #14 | @ana | 2h | 📋 To Do |
| RF02-T5 | Método validação data de nascimento | #14 | @ana | 1h | 📋 To Do |

**Subtotal RF02:** 5,5 horas

---

### 📅 RF03 — Agendamento de Consultas

**Critérios de Aceite:**
- ✓ Animal selecionado pertence ao tutor selecionado
- ✓ Data/hora não pode ser no passado
- ✓ Sistema impede agendamento duplicado (mesmo vet + mesmo horário)
- ✓ Agendamento aparece na agenda do veterinário

**Decomposição:**

| Task ID | Descrição | Card | Responsável | Estimativa | Status |
|---------|-----------|------|-------------|------------|--------|
| RF03-T1 | Criar entidade `Agendamento` no DER | #9 | @pedro | 1h | 📋 To Do |
| RF03-T2 | Definir relacionamentos (Tutor, Animal, Vet) | #9 | @pedro | 1h | 📋 To Do |
| RF03-T3 | Script DDL tabela `Agendamento` | #10 | @joao | 1h | 📋 To Do |
| RF03-T4 | Stored procedure `sp_ValidarConflito` | #10 | @joao | 2h | 📋 To Do |
| RF03-T5 | Classe `Agendamento.cs` | #14 | @ana | 2h | 📋 To Do |
| RF03-T6 | Classe `AgendamentoService.cs` | #14 | @ana | 3h | 📋 To Do |

**Subtotal RF03:** 10 horas (tasks de front-end vão para Sprint 3)

---

### ⚠️ RF04 — Validação de Conflito de Horário

**Critérios de Aceite:**
- ✓ Validação considera apenas agendamentos não cancelados
- ✓ Mensagem de erro informa qual horário está ocupado

**Decomposição:**

| Task ID | Descrição | Card | Responsável | Estimativa | Status |
|---------|-----------|------|-------------|------------|--------|
| RF04-T1 | Constraint UNIQUE (vet + data_hora) | #10 | @joao | 30min | 📋 To Do |
| RF04-T2 | Método `ValidarConflito()` no Service | #14 | @ana | Integrado RF03-T6 | 📋 To Do |

**Subtotal RF04:** 0,5 horas (integrado com RF03)

---

### 🔍 RF10 — Busca de Tutores

**Critérios de Aceite:**
- ✓ Busca case-insensitive por nome, CPF ou telefone
- ✓ Mínimo 3 caracteres para iniciar busca
- ✓ Resultados limitados a 10 registros

**Decomposição:**

| Task ID | Descrição | Card | Responsável | Estimativa | Status |
|---------|-----------|------|-------------|------------|--------|
| RF10-T1 | Criar índices em `Tutor` (nome, cpf, telefone) | #10 | @maria | 1h | 📋 To Do |
| RF10-T2 | Método `BuscarTutores(termo)` no Service | #14 | @pedro | 2h | 📋 To Do |

**Subtotal RF10:** 3 horas

---

## 📊 Sprint Backlog Consolidado

### Por Membro do Time:

| Membro | Tasks Atribuídas | Horas Estimadas | Percentual da Capacidade |
|--------|------------------|-----------------|--------------------------|
| **@joao** | RF01-T2, RF02-T3, RF03-T3, RF03-T4, RF04-T1 | 5,5h | ⚖️ 110% (aceitável) |
| **@maria** | RF01-T1, RF01-T3, RF02-T1, RF02-T2, RF10-T1 | 5,5h | ⚖️ 110% (aceitável) |
| **@pedro** | RF01-T4, RF01-T5, RF03-T1, RF03-T2, RF10-T2 | 9h | ⚠️ 180% (sobrecarga!) |
| **@ana** | RF02-T4, RF02-T5, RF03-T5, RF03-T6 | 8h | ⚠️ 160% (sobrecarga!) |

**TOTAL ESTIMADO:** 28 horas  
**CAPACIDADE DISPONÍVEL:** 20 horas  
**⚠️ ALERTA:** Time está 40% sobrecarregado!

### ⚙️ Ajuste Realizado no Planning:

Decisão do time: **mover tasks de front-end do RF03 para Sprint 3**

Após ajuste:

| Membro | Tasks Ajustadas | Horas Finais |
|--------|-----------------|--------------|
| @joao | RF01-T2, RF02-T3, RF03-T3, RF03-T4, RF04-T1 | 5,5h ✅ |
| @maria | RF01-T1, RF01-T3, RF02-T1, RF02-T2, RF10-T1 | 5,5h ✅ |
| @pedro | RF01-T4, RF01-T5, RF03-T1, RF03-T2, RF10-T2 | 9h ⚠️ |
| @ana | RF02-T4, RF02-T5, RF03-T5, RF03-T6 | 8h ⚠️ |

**Observação:** Pedro e Ana aceitaram carga maior pois têm mais disponibilidade esta semana.

---

## 📋 Definition of Done (DoD) da Sprint

Uma task só está **DONE** quando:

- [x] Código implementado e testado localmente
- [x] Commit realizado no repositório Git com mensagem descritiva
- [x] Pull request criado e revisado por outro membro
- [x] Merge realizado na branch `main`
- [x] Documentação técnica atualizada (comentários no código)
- [x] Task marcada como concluída no GitHub Projects

Um **RF está DONE** quando:
- [x] Todas as suas tasks estão concluídas
- [x] Critérios de aceite validados
- [x] Demonstração funcional gravada ou apresentada
- [x] Seção correspondente escrita no documento do PIM

A **Sprint está DONE** quando:
- [x] Sprint Goal foi atingida
- [x] Todos os RFs selecionados estão DONE
- [x] Sprint Review realizada
- [x] Sprint Retrospectiva documentada

---

## 🔗 Dependências Identificadas

| Task Dependente | Depende de | Motivo |
|-----------------|------------|--------|
| RF01-T2 (DDL Tutor) | RF01-T1 (DER Tutor) | Precisa do modelo conceitual pronto |
| RF02-T3 (DDL Animal) | RF01-T2 (DDL Tutor) | Tabela Animal tem FK para Tutor |
| RF03-T3 (DDL Agendamento) | RF01-T2, RF02-T3 | Tabela Agendamento tem FKs para Tutor e Animal |
| RF03-T6 (AgendamentoService) | RF03-T5 (Classe Agendamento) | Service usa a classe de domínio |

**Ordem de execução recomendada:**
1. **Dia 1 (28/04):** RF01-T1, RF02-T1, RF03-T1 (DER completo)
2. **Dia 2 (29/04):** RF01-T2, RF02-T3, RF03-T3 (DDL das tabelas)
3. **Dia 3 (30/04 a 03/05):** Demais tasks em paralelo

---

## 🚧 Riscos e Impedimentos Identificados

| Risco | Probabilidade | Impacto | Mitigação |
|-------|---------------|---------|-----------|
| Ambiente SQL Server não configurado em todas as máquinas | Média | Alto | @joao vai criar script de setup e compartilhar |
| Membros com pouca experiência em C# | Alta | Médio | Pair programming: @pedro (experiente) auxilia @ana |
| Sobrecarga de @pedro e @ana | Baixa | Médio | Caso necessário, @joao e @maria podem pegar tasks extras |
| Conflito de agenda na quinta (01/05 — feriado) | Alta | Baixo | Planejar trabalho extra na quarta e sexta |

---

## 📅 Cerimônias Agendadas

| Cerimônia | Data/Hora | Duração | Participantes |
|-----------|-----------|---------|---------------|
| **Sprint Planning** | 28/04 (seg) 19h | 2h | Todos (obrigatório) |
| **Daily Stand-up** | Todos os dias 19h | 15min | Todos (assíncrono no WhatsApp OK) |
| **Sprint Review** | 03/05 (sáb) 14h | 1h | Todos (obrigatório) |
| **Sprint Retrospective** | 03/05 (sáb) 15h | 30min | Todos (obrigatório) |

---

## 💬 Acordos do Time

Durante o Planning, o time acordou:

1. ✅ **Comunicação:** Daily stand-up será assíncrono no WhatsApp (todos postam até 19h)
2. ✅ **Pair Programming:** Pedro e Ana vão trabalhar juntos nas classes C# (terça 19h-21h)
3. ✅ **Horário de trabalho:** Segunda a sexta 19h-23h, sábado 9h-18h
4. ✅ **Impedimentos:** Avisar imediatamente no grupo se travar em alguma task
5. ✅ **Code Review:** Todo PR precisa de aprovação de outro membro antes do merge
6. ✅ **Commits:** Usar padrão `[RF##-T#] Descrição` (ex: `[RF01-T2] Cria tabela Tutor`)

---

## 📝 Observações e Decisões Técnicas

### Decisão 1: Arquitetura em Camadas
**Contexto:** Como organizar o código C#?  
**Decisão:** Adotar arquitetura em 4 camadas (Apresentação, Aplicação, Negócio, Dados)  
**Justificativa:** Facilita manutenção e atende requisito da Etapa 4 do PIM  
**Responsável:** @pedro vai criar a estrutura base de pastas

### Decisão 2: Validação de CPF
**Contexto:** Validar CPF no banco ou no C#?  
**Decisão:** Validar nos dois lugares (defesa em profundidade)  
**Justificativa:** Função SQL garante integridade mesmo se o C# for bypassed; C# dá feedback mais rápido ao usuário  
**Responsável:** @maria (SQL) e @pedro (C#)

### Decisão 3: Stored Procedure vs LINQ
**Contexto:** Como validar conflito de agendamento?  
**Decisão:** Usar LINQ no C#, criar SP só para comparação de performance na documentação  
**Justificativa:** LINQ é mais fácil de debugar e testar; SP fica como exemplo de otimização  
**Responsável:** @ana implementa LINQ, @joao implementa SP

---

## 📚 Referências Consultadas

- SCHWABER, Ken; SUTHERLAND, Jeff. **The Scrum Guide**. 2020.
- COHN, Mike. **Agile Estimating and Planning**. Prentice Hall, 2005.
- SOMMERVILLE, Ian. **Engenharia de Software**. 10ª ed. Pearson, 2018.
- PRODUCT_BACKLOG_REFERENCIA.md (documento interno do projeto)

---

## ✅ Checklist de Validação do Planning

Antes de encerrar a reunião, validamos:

- [x] Sprint Goal está claro para todos?
- [x] Todos os RFs selecionados têm critérios de aceite definidos?
- [x] Todas as tasks têm responsável atribuído?
- [x] Estimativas foram validadas pelo time?
- [x] Dependências entre tasks foram identificadas?
- [x] Capacidade do time está respeitada (ou sobrecarga justificada)?
- [x] Definition of Done está clara?
- [x] Riscos foram mapeados?
- [x] Próximas cerimônias estão agendadas?

---

**Assinaturas (validação do time):**

- ☑️ **Scrum Master:** [Seu nome] — Facilitou o Planning e documentou
- ☑️ **@joao** — Comprometido com suas tasks
- ☑️ **@maria** — Comprometida com suas tasks
- ☑️ **@pedro** — Comprometido com suas tasks (carga alta aceita)
- ☑️ **@ana** — Comprometida com suas tasks (carga alta aceita)

---

**Data de criação:** 28/04/2026  
**Última atualização:** 28/04/2026  
**Próxima revisão:** 03/05/2026 (Sprint Review)

