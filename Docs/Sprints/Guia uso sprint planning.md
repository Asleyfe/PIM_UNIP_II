# 📚 Guia de Uso dos Artefatos de Sprint Planning
> **Como documentar, armazenar e usar os Sprint Planning no PIM III**

---

## 🎯 Por que documentar o Sprint Planning?

### ✅ **Para o Projeto:**
- Referência durante a Sprint (o que foi acordado)
- Rastreabilidade de decisões técnicas
- Evidência de trabalho colaborativo

### ✅ **Para o PIM (avaliação acadêmica):**
- Comprova aplicação prática do Scrum
- Demonstra decomposição técnica dos requisitos
- Mostra distribuição de trabalho entre membros
- Anexo obrigatório na seção de Engenharia de Software Ágil

### ✅ **Para sua carreira:**
- Portfólio profissional (GitHub público)
- Demonstra maturidade em gestão de projetos
- Modelo replicável em projetos futuros

---

## 📂 Onde Armazenar

### **Estrutura Recomendada no Repositório:**

```
PIM_UNIP_III/
├── README.md
├── docs/
│   ├── 01_negocio/
│   │   └── caracterizacao_petcare.md
│   ├── 02_requisitos/
│   │   ├── PRODUCT_BACKLOG_REFERENCIA.md ⭐
│   │   ├── requisitos_funcionais.md
│   │   └── requisitos_nao_funcionais.md
│   ├── 03_sprints/
│   │   ├── TEMPLATE_SPRINT_PLANNING.md ⭐
│   │   ├── SPRINT_01_PLANNING.md ⭐
│   │   ├── SPRINT_02_PLANNING.md ⭐
│   │   ├── SPRINT_03_PLANNING.md ⭐
│   │   ├── SPRINT_04_PLANNING.md ⭐
│   │   └── SPRINT_05_PLANNING.md ⭐
│   ├── 04_arquitetura/
│   │   ├── diagrama_camadas.md
│   │   └── decisoes_tecnicas.md
│   └── 05_retrospectivas/
│       ├── SPRINT_01_RETRO.md
│       ├── SPRINT_02_RETRO.md
│       └── ...
├── src/
│   ├── database/
│   ├── backend/
│   └── frontend/
└── anexos_pim/
    └── (PDFs para anexar no trabalho final)
```

---

## 🔄 Ciclo de Vida do Artefato

### **ANTES da Sprint Planning:**

1. **Scrum Master prepara:**
   - Copia o `TEMPLATE_SPRINT_PLANNING.md`
   - Renomeia para `SPRINT_0X_PLANNING.md`
   - Preenche seção "Informações da Sprint" (datas, capacidade)
   - Seleciona RFs do Product Backlog que entrarão na Sprint
   - Compartilha com o time 1 dia antes da reunião

2. **Development Team prepara:**
   - Lê o documento pré-preenchido
   - Consulta `PRODUCT_BACKLOG_REFERENCIA.md` para entender decomposição sugerida
   - Pensa em suas disponibilidades de tempo

---

### **DURANTE a Sprint Planning:**

1. **Início (15 min):**
   - Scrum Master projeta o documento na tela (compartilhamento de tela)
   - Todos revisam o Sprint Goal proposto
   - Ajustes no objetivo se necessário

2. **Decomposição (1h):**
   - Para cada RF selecionado:
     - Scrum Master lê critérios de aceite
     - Time discute decomposição técnica
     - **Um membro (rodízio) vai preenchendo a tabela de tasks AO VIVO**
   - Ferramenta: Google Docs compartilhado (edição simultânea) ou Scrum Master digita enquanto time dita

3. **Atribuição (30 min):**
   - Preencher coluna "Responsável" da tabela
   - Calcular "Sprint Backlog Consolidado"
   - Identificar sobrecarga e ajustar

4. **Fechamento (15 min):**
   - Preencher seção "Dependências"
   - Preencher seção "Riscos"
   - Validar checklist
   - **Todos "assinam" digitalmente** (☑️ no final do documento)

---

### **DEPOIS da Sprint Planning:**

1. **Scrum Master:**
   - Salva documento final no repositório Git
   - Faz commit: `git commit -m "docs: Sprint 2 Planning completo"`
   - Cria os checkboxes correspondentes nos Cards do GitHub Projects
   - Posta no grupo do WhatsApp: "Sprint 2 Planning finalizado! Link: [URL do arquivo no GitHub]"

2. **Development Team:**
   - Cada membro vai nos Cards do GitHub Projects que pegou
   - Cria suas subtasks como checkboxes (copiando do Planning)
   - Começa a trabalhar seguindo a ordem de dependências

---

### **DURANTE a Sprint:**

O documento é usado para:
- ✅ Consultar o que foi acordado (ex: "espera, quantas horas eu estimei mesmo?")
- ✅ Validar se uma task está no escopo da Sprint atual
- ✅ Relembrar decisões técnicas tomadas no Planning

**⚠️ Se algo mudar durante a Sprint:**
- Atualizar o documento (seção "Observações" no final)
- Adicionar nota: `**ATUALIZAÇÃO [Data]:** [Mudança realizada e justificativa]`

---

### **FINAL da Sprint (Review/Retro):**

1. **Na Sprint Review:**
   - Scrum Master abre o documento
   - Compara: "Meta da Sprint" vs "O que foi entregue"
   - Atualiza coluna "Status" das tasks (Done/Não Done)
   - Adiciona seção final: "Resultados da Sprint"

2. **Na Sprint Retrospective:**
   - Criar documento separado: `SPRINT_0X_RETRO.md`
   - Referenciar o Planning: "Baseado no planejamento em SPRINT_0X_PLANNING.md..."

---

## 📝 Como Usar no Documento do PIM

### **Etapa 2 — Engenharia de Software Ágil (Card #8)**

No seu documento Word do PIM, na seção **2.5 Definição das Iterações e Critérios de Aceite**, você vai escrever:

```markdown
### 2.5 Definição das Iterações e Critérios de Aceite

O projeto foi organizado em 5 Sprints, seguindo a metodologia Scrum
conforme proposto por Schwaber e Sutherland (2020). Cada Sprint teve
duração de 3 a 5 dias úteis, com cerimônias formais de Planning,
Daily Stand-up, Review e Retrospectiva.

A decomposição de requisitos em tarefas técnicas foi realizada de forma
colaborativa durante as reuniões de Sprint Planning, com participação
ativa de todos os membros do Development Team. O processo seguiu as
seguintes etapas:

1. **Seleção dos RFs:** Com base na priorização MoSCoW (seção 2.4),
   o Product Owner e o time selecionaram os requisitos funcionais
   que entrariam em cada Sprint.

2. **Decomposição técnica:** Cada RF foi analisado quanto ao seu
   impacto arquitetural (Dados, Negócio, Aplicação, Apresentação),
   resultando na identificação de tasks técnicas específicas.

3. **Atribuição e estimativa:** As tasks foram distribuídas entre
   os membros do time, respeitando a capacidade de trabalho disponível
   (aproximadamente 5 horas por pessoa por Sprint de 3 dias).

4. **Documentação:** Todo Sprint Planning foi documentado e armazenado
   no repositório do projeto, garantindo rastreabilidade completa das
   decisões técnicas e organizacionais.

**Exemplo: Sprint 2 — Dados e Arquitetura**

A Sprint 2 teve como meta implementar a base de dados e as
funcionalidades core do sistema. Foram selecionados 5 requisitos
funcionais (RF01, RF02, RF03, RF04, RF10), resultando em 25 tasks
técnicas distribuídas entre 4 membros do time.

O documento completo do Sprint Planning da Sprint 2 encontra-se
no Apêndice A deste trabalho.

[TABELA RESUMIDA — copiar do Sprint Planning:]

| RF   | Requisito               | Tasks | Horas | Status  |
|------|-------------------------|-------|-------|---------|
| RF01 | Cadastro de Tutores     | 5     | 9h    | ✅ Done |
| RF02 | Cadastro de Animais     | 5     | 5.5h  | ✅ Done |
| RF03 | Agendamento de Consultas| 6     | 10h   | ✅ Done |
| RF04 | Validação de Conflito   | 2     | 0.5h  | ✅ Done |
| RF10 | Busca de Tutores        | 2     | 3h    | ✅ Done |

**Critérios de Aceite — Exemplo RF03:**

Para o requisito RF03 (Agendamento de Consultas), foram definidos
os seguintes critérios de aceite durante o Sprint Planning:

✓ O animal selecionado deve pertencer ao tutor selecionado
✓ Data/hora não pode ser no passado
✓ Sistema deve impedir agendamento duplicado (mesmo vet + mesmo horário)
✓ Agendamento confirmado deve aparecer na agenda do veterinário

Estes critérios foram validados na Sprint Review através de
demonstração funcional do sistema com dados de teste.

**Referência completa:** Consultar Apêndice A — Sprint Planning (Sprint 2)
```

---

### **Apêndice A — Sprint Planning Documents**

No final do documento Word do PIM, na seção de Apêndices, você vai:

**Opção 1: Incluir todos os Plannings**
```
APÊNDICE A — SPRINT 1 PLANNING
[Colar o conteúdo completo do SPRINT_01_PLANNING.md]

APÊNDICE B — SPRINT 2 PLANNING
[Colar o conteúdo completo do SPRINT_02_PLANNING.md]

... (até Sprint 5)
```

**Opção 2: Incluir resumo + link para GitHub** (mais enxuto)
```
APÊNDICE A — DOCUMENTAÇÃO DOS SPRINT PLANNINGS

Os documentos completos de Sprint Planning de todas as 5 Sprints
do projeto encontram-se armazenados no repositório GitHub do projeto:

https://github.com/[seu-usuario]/PIM_UNIP_III/tree/main/docs/03_sprints

Lista de documentos:
• SPRINT_01_PLANNING.md — Fundação e Planejamento
• SPRINT_02_PLANNING.md — Dados e Arquitetura
• SPRINT_03_PLANNING.md — Interface e Experiência
• SPRINT_04_PLANNING.md — Dados e Aspectos Humanos
• SPRINT_05_PLANNING.md — Fechamento e Entrega

Abaixo, apresenta-se um resumo executivo de cada Sprint Planning.

[Incluir apenas as tabelas "Sprint Backlog Consolidado" de cada uma]
```

---

## 📊 Exemplo de Tabela Resumida para o PIM

Use essa tabela no corpo do texto (Etapa 2):

```markdown
**Tabela X — Resumo das Sprints do Projeto**

| Sprint | Período | Foco | RFs | Tasks | Horas | Status |
|--------|---------|------|-----|-------|-------|--------|
| 1 | 24-26/04 | Fundação e Planejamento | 7 cards documentação | 8 | 16h | ✅ Completa |
| 2 | 28/04-03/05 | Dados e Arquitetura | RF01,02,03,04,10 | 25 | 28h | ✅ Completa |
| 3 | 05-10/05 | Interface e Experiência | RF05,06,07,08 | 22 | 32h | ✅ Completa |
| 4 | 12-17/05 | Dados e Aspectos Humanos | RF09,11,12,13 | 18 | 24h | ✅ Completa |
| 5 | 19-22/05 | Fechamento e Entrega | RF14+ (integração) | 12 | 20h | ✅ Completa |

**Total:** 5 Sprints | 20 RFs implementados | 85 tasks técnicas | 120 horas de trabalho
```

---

## 🎯 Checklist de Qualidade do Artefato

Um Sprint Planning está **BEM DOCUMENTADO** se:

### Conteúdo:
- [ ] Sprint Goal está claro e mensurável
- [ ] Todos os RFs têm critérios de aceite listados
- [ ] Decomposição técnica está completa (nenhuma task genérica tipo "fazer tudo")
- [ ] Cada task tem responsável, estimativa e card vinculado
- [ ] Dependências entre tasks estão mapeadas
- [ ] Riscos foram identificados com mitigações
- [ ] Definition of Done está explícita

### Formato:
- [ ] Tabelas estão bem formatadas (alinhamento correto)
- [ ] Usa markdown corretamente (cabeçalhos, listas, checkboxes)
- [ ] Emojis ajudam na leitura (📋 📊 🎯 ✅)
- [ ] Seções estão numeradas ou organizadas logicamente

### Rastreabilidade:
- [ ] RFs fazem referência ao Product Backlog
- [ ] Tasks fazem referência aos Cards do GitHub Projects
- [ ] Decisões técnicas têm justificativa documentada
- [ ] Há assinaturas/validação do time no final

### Para o PIM:
- [ ] Citações de autores (Schwaber, Sutherland, Cohn)
- [ ] Terminologia Scrum está correta (não inventa termos)
- [ ] Demonstra trabalho colaborativo (não parece trabalho individual)
- [ ] Pode ser anexado no documento Word sem edição pesada

---

## 💡 Dicas Profissionais

### ✅ **DO (Faça):**
- Preencher o Planning AO VIVO durante a reunião (não depois)
- Ser específico nas descrições de tasks ("Criar stored procedure X" > "Trabalhar no banco")
- Documentar decisões técnicas importantes (por que escolheu abordagem X?)
- Usar linguagem profissional mas não robótica
- Incluir horas reais trabalhadas depois (seção "Resultados")

### ❌ **DON'T (Não faça):**
- Inventar dados fictícios só para preencher o documento
- Copiar/colar de outros projetos sem adaptar ao contexto PetCare
- Deixar seções em branco ("TODO: preencher depois")
- Usar jargão excessivo só para "parecer técnico"
- Esquecer de commitar o arquivo no Git

---

## 📚 Referências para Documentar

No documento do PIM, ao mencionar Sprint Planning, cite:

**SCHWABER, Ken; SUTHERLAND, Jeff.** *The Scrum Guide: The Definitive Guide to Scrum: The Rules of the Game.* 2020. Disponível em: https://scrumguides.org. Acesso em: [data].

**COHN, Mike.** *Agile Estimating and Planning.* Prentice Hall, 2005.

**SUTHERLAND, Jeff.** *Scrum: A Arte de Fazer o Dobro do Trabalho na Metade do Tempo.* São Paulo: LeYa, 2014.

---

## 🚀 Resumo Executivo

| Quando | O que fazer | Onde | Quem |
|--------|-------------|------|------|
| **Antes da Sprint** | Preparar template preenchido | Google Docs | Scrum Master |
| **Durante Planning** | Preencher colaborativamente | Reunião (2h) | Todo o time |
| **Após Planning** | Commitar no Git + criar checkboxes | GitHub | Scrum Master |
| **Durante Sprint** | Consultar quando necessário | docs/03_sprints/ | Qualquer membro |
| **Após Sprint** | Atualizar status + resultados | GitHub | Scrum Master |
| **Final do PIM** | Anexar/referenciar no documento Word | Apêndice | Responsável pelo doc |

---
