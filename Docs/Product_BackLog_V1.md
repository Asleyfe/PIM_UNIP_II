# 📋 Product Backlog - PIM UNIP III — PetCare Goiânia

Este documento contém a lista de requisitos funcionais decompostos para facilitar o planejamento das sprints e o acompanhamento das entregas.

> **Última atualização:** Sprint 1 (26/04/2026)  
> **Total de RFs:** 15 | **Sprints:** 5

---

## 📊 Visão Geral por Sprint

| Sprint | Período | RFs |
|--------|---------|-----|
| Sprint 1 | 24/04 – 26/04 | Fundação e planejamento (Etapas 1 e 2) |
| Sprint 2 | 28/04 – 03/05 | RF001, RF002, RF014, RF015, RF016 |
| Sprint 3 | 05/05 – 10/05 | RF003, RF004, RF005, RF006, RF007, RF012 |
| Sprint 4 | 12/05 – 17/05 | RF009, RF010, RF011, RF013 |
| Sprint 5 | 19/05 – 22/05 | Integração, testes e entrega |

---

## 🛠️ Requisitos Funcionais

---

### RF001 — Gerenciar Agendamento de Consultas
**Prioridade:** Alta | **Sprint:** 2 | **MoSCoW:** Must have | **Esforço:** 22h

#### 📝 Descrição:
O sistema deve permitir que funcionários da recepção cadastrem, editem, visualizem e cancelem agendamentos de consultas veterinárias. O sistema deve também permitir que os clientes visualizem sua agenda de consultas.

#### ✅ Critérios de Aceite:
- [ ] Permitir seleção de Tutor, Animal e Veterinário.
- [ ] Validar se o veterinário possui disponibilidade no horário escolhido.
- [ ] Sistema deve impedir agendamento duplicado (mesmo veterinário + mesmo horário).
- [ ] Registrar data, hora e observações do agendamento.
- [ ] Permitir edição e cancelamento de agendamentos existentes.
- [ ] Agendamento confirmado deve aparecer na agenda do veterinário.

#### 🏗️ Impacto Arquitetural:
- **Camada de Dados (Etapa 3)** → Card #9 e #10
- **Camada de Negócio (Etapa 4)** → Card #14
- **Camada de Aplicação (Etapa 4)** → Card #13
- **Camada de Apresentação (Etapa 5)** → Card #17

#### 📊 Decomposição de Tarefas:

| Task | Descrição | Card | Est. | Responsável |
| :--- | :--- | :---: | :---: | :--- |
| **RF001-T1** | Criar entidade `Agendamento` no DER | #9 | 1h | _________ |
| **RF001-T2** | Definir relacionamentos (Tutor, Animal, Veterinário) | #9 | 1h | _________ |
| **RF001-T3** | Script DDL (Data Definition Language) tabela `Agendamento` | #10 | 1h | _________ |
| **RF001-T4** | Stored procedure `sp_ValidarConflito` | #10 | 2h | _________ |
| **RF001-T5** | Classe `Agendamento.cs` | #14 | 2h | _________ |
| **RF001-T6** | Classe `AgendamentoService.cs` com validação de conflito | #14 | 3h | _________ |
| **RF001-T7** | Controller `AgendamentoController.cs` | #13 | 3h | _________ |
| **RF001-T8** | Tela `agendamento.html` com seletores | #17 | 4h | _________ |
| **RF001-T9** | Tela `agenda-dia.html` (visualização de agenda) | #17 | 4h | _________ |
| **RF001-T10** | JavaScript para buscar horários disponíveis | #17 | 1h | _________ |

**⏱️ Total estimado:** 22 horas

---

### RF002 — Centralizar Agenda da Clínica
**Prioridade:** Alta | **Sprint:** 2 | **MoSCoW:** Must have | **Esforço:** 6h

#### 📝 Descrição:
O sistema deve permitir que funcionários autorizados acessem uma agenda única e centralizada da clínica, com visão geral de todos os atendimentos do dia/semana.

#### ✅ Critérios de Aceite:
- [ ] Agenda acessível por todos os funcionários autorizados.
- [ ] Exibir atendimentos do dia agrupados por veterinário.
- [ ] Filtros por data, veterinário e status do atendimento.

#### 🏗️ Impacto Arquitetural:
- **Camada de Dados (Etapa 3)** → Card #10 (view agregada)
- **Camada de Aplicação (Etapa 4)** → Card #13
- **Camada de Apresentação (Etapa 5)** → Card #17

#### 📊 Decomposição de Tarefas:

| Task | Descrição | Card | Est. | Responsável |
| :--- | :--- | :---: | :---: | :--- |
| **RF002-T1** | View SQL `vw_AgendaDia` | #10 | 1h | _________ |
| **RF002-T2** | Endpoint de consulta no Controller | #13 | 1h | _________ |
| **RF002-T3** | Tela `agenda-central.html` com filtros | #17 | 4h | _________ |

**⏱️ Total estimado:** 6 horas

---

### RF003 — Cadastrar e Consultar Histórico Clínico do Animal
**Prioridade:** Alta | **Sprint:** 3 | **MoSCoW:** Must have | **Esforço:** 12h

#### 📝 Descrição:
O sistema deve permitir que veterinários registrem e consultem o histórico clínico completo dos animais, incluindo consultas anteriores, diagnósticos e tratamentos.

#### ✅ Critérios de Aceite:
- [ ] Histórico vinculado ao animal e ordenado por data.
- [ ] Veterinário pode registrar nova entrada a partir de um agendamento realizado.
- [ ] Consulta de histórico disponível para todos os veterinários.

#### 🏗️ Impacto Arquitetural:
- **Camada de Dados (Etapa 3)** → Card #9 e #10
- **Camada de Negócio (Etapa 4)** → Card #14
- **Camada de Apresentação (Etapa 5)** → Card #17

#### 📊 Decomposição de Tarefas:

| Task | Descrição | Card | Est. | Responsável |
| :--- | :--- | :---: | :---: | :--- |
| **RF003-T1** | Criar entidade `HistoricoClinico` no DER | #9 | 1h | _________ |
| **RF003-T2** | Script DDL tabela `HistoricoClinico` | #10 | 1h | _________ |
| **RF003-T3** | Classe `HistoricoClinico.cs` | #14 | 2h | _________ |
| **RF003-T4** | Classe `HistoricoService.cs` | #14 | 2h | _________ |
| **RF003-T5** | Controller `HistoricoController.cs` | #13 | 2h | _________ |
| **RF003-T6** | Tela `historico-animal.html` | #17 | 4h | _________ |

**⏱️ Total estimado:** 12 horas

---

### RF004 — Armazenar Prontuário Digital dos Animais
**Prioridade:** Alta | **Sprint:** 3 | **MoSCoW:** Must have | **Esforço:** 19h

#### 📝 Descrição:
O sistema deve permitir que veterinários e funcionários autorizados armazenem e acessem prontuários digitais dos animais, contendo queixa principal, anamnese, exame físico, diagnóstico, prescrição e observações.

#### ✅ Critérios de Aceite:
- [ ] Prontuário vinculado a um agendamento já realizado.
- [ ] Campos de texto livre para descrições clínicas.
- [ ] Data e hora de registro automáticas.
- [ ] Histórico completo do animal acessível a qualquer veterinário.
- [ ] O prontuário deve ser um registro único por consulta (não deve permitir múltiplas entradas por consulta).
- [ ] Deve haver um "Check-in" para marcar a entrada do animal na consulta e um "Check-out" para marcar a saída e finalizar o prontuário.
- [ ] Deve ser possivel filtrar por animal e historico(prontuarios), validar a evolução do animal.

#### 🏗️ Impacto Arquitetural:
- **Camada de Dados (Etapa 3)** → Card #9 e #10
- **Camada de Negócio (Etapa 4)** → Card #14
- **Camada de Aplicação (Etapa 4)** → Card #13
- **Camada de Apresentação (Etapa 5)** → Card #17

#### 📊 Decomposição de Tarefas:

| Task | Descrição | Card | Est. | Responsável |
| :--- | :--- | :---: | :---: | :--- |
| **RF004-T1** | Criar entidade `Prontuario` no DER | #9 | 1h | _________ |
| **RF004-T2** | Relacionamento 1:1 com Agendamento | #9 | 1h | _________ |
| **RF004-T3** | Script DDL tabela `Prontuario` | #10 | 2h | _________ |
| **RF004-T4** | Classe `Prontuario.cs` | #14 | 2h | _________ |
| **RF004-T5** | Classe `ProntuarioService.cs` | #14 | 3h | _________ |
| **RF004-T6** | Controller `ProntuarioController.cs` | #13 | 2h | _________ |
| **RF004-T7** | Tela `prontuario.html` (formulário completo) | #17 | 5h | _________ |
| **RF004-T8** | Tela `historico-prontuarios.html` | #17 | 3h | _________ |

**⏱️ Total estimado:** 19 horas

---

### RF005 — Controlar Estoque de Produtos
**Prioridade:** Alta | **Sprint:** 3 | **MoSCoW:** Must have | **Esforço:** 15h

#### 📝 Descrição:
O sistema deve permitir que funcionários do pet shop cadastrem novos produtos, registrem entradas, saídas e consultem o estoque de produtos (rações, medicamentos, acessórios).

#### ✅ Critérios de Aceite:
- [ ] Cadastro de produto com código único, descrição, categoria, validade (opcional), preço e estoque mínimo.
- [ ] Deve possuir um campo quantidade.
- [ ] Deve conter categoria (Raçoes, Medicamentos, Acessorios, etc).
- [ ] Validade deve ser obrigatória para medicamentos e raçoes.
- [ ] Registro de entrada (compra) e saída (venda/uso).
- [ ] Em caso de uso, deve ser informado o funcionario que utilizou o produto.
- [ ] Estoque nunca pode ficar negativo(Opcional com alerta de estoque minimo).
- [ ] Alerta visual quando estoque atingir nível mínimo.

#### 🏗️ Impacto Arquitetural:
- **Camada de Dados (Etapa 3)** → Card #9 e #10
- **Camada de Negócio (Etapa 4)** → Card #14
- **Camada de Aplicação (Etapa 4)** → Card #13
- **Camada de Apresentação (Etapa 5)** → Card #17

#### 📊 Decomposição de Tarefas:

| Task | Descrição | Card | Est. | Responsável |
| :--- | :--- | :---: | :---: | :--- |
| **RF005-T1** | Criar entidades `Produto` e `MovimentacaoEstoque` no DER | #9 | 1h | _________ |
| **RF005-T2** | Script DDL tabelas `Produto` e `MovimentacaoEstoque` | #10 | 2h | _________ |
| **RF005-T3** | Trigger para atualizar estoque automaticamente | #10 | 3h | _________ |
| **RF005-T4** | Classes `Produto.cs` e `MovimentacaoEstoque.cs` | #14 | 2h | _________ |
| **RF005-T5** | Métodos `RegistrarEntrada()` e `RegistrarSaida()` | #14 | 3h | _________ |
| **RF005-T6** | Tela `estoque.html` com cadastro e movimentação | #17 | 4h | _________ |

**⏱️ Total estimado:** 15 horas

---

### RF006 — Controlar Validade de Produtos
**Prioridade:** Alta | **Sprint:** 3 | **MoSCoW:** Must have | **Esforço:** 6h

#### 📝 Descrição:
O sistema deve permitir que funcionários do pet shop registrem e monitorem a validade dos produtos em estoque, exibindo alertas para produtos próximos ao vencimento.

#### ✅ Critérios de Aceite:
- [ ] Campo de data de validade obrigatório no cadastro de produtos perecíveis.
- [ ] Alerta exibido no dashboard para produtos vencendo nos próximos 30 dias.
- [ ] Lista de produtos vencidos ou próximos ao vencimento acessível no módulo de estoque.

#### 🏗️ Impacto Arquitetural:
- **Camada de Dados (Etapa 3)** → Card #10 (view)
- **Camada de Negócio (Etapa 4)** → Card #14
- **Camada de Aplicação (Etapa 4)** → Card #13
- **Camada de Apresentação (Etapa 5)** → Card #17

#### 📊 Decomposição de Tarefas:

| Task | Descrição | Card | Est. | Responsável |
| :--- | :--- | :---: | :---: | :--- |
| **RF006-T1** | View SQL `vw_ProdutosAVencer` | #10 | 1h | _________ |
| **RF006-T2** | Método `ObterProdutosAVencer()` no Service | #14 | 2h | _________ |
| **RF006-T3** | Componente de alerta de validade no dashboard | #17 | 3h | _________ |

**⏱️ Total estimado:** 6 horas

---

### RF007 — Registrar Vendas de Produtos e Serviços
**Prioridade:** Alta | **Sprint:** 3 | **MoSCoW:** Should have | **Esforço:** 10h

#### 📝 Descrição:
O sistema deve permitir que funcionários da recepção ou pet shop registrem vendas de produtos e serviços, gerando registro financeiro e atualizando o estoque automaticamente.

#### ✅ Critérios de Aceite:
- [ ] Venda vincula produto ou serviço a um tutor/Cliente.
- [ ] Saída de estoque gerada automaticamente a partir da venda.
- [ ] Registro do valor total da venda para fins de relatório financeiro.
- [ ] Histórico de vendas consultável por período e por Cliente.

#### 🏗️ Impacto Arquitetural:
- **Camada de Dados (Etapa 3)** → Card #9 e #10
- **Camada de Negócio (Etapa 4)** → Card #14
- **Camada de Aplicação (Etapa 4)** → Card #13
- **Camada de Apresentação (Etapa 5)** → Card #17

#### 📊 Decomposição de Tarefas:

| Task | Descrição | Card | Est. | Responsável |
| :--- | :--- | :---: | :---: | :--- |
| **RF007-T1** | Criar entidade `Venda` e `ItemVenda` no DER | #9 | 1h | _________ |
| **RF007-T2** | Script DDL tabelas `Venda` e `ItemVenda` | #10 | 2h | _________ |
| **RF007-T3** | Classes `Venda.cs` e `VendaService.cs` | #14 | 3h | _________ |
| **RF007-T4** | Controller `VendaController.cs` | #13 | 2h | _________ |
| **RF007-T5** | Tela `registrar-venda.html` | #17 | 2h | _________ |

**⏱️ Total estimado:** 10 horas

---

### RF009 — Enviar Lembretes Automáticos para Clientes
**Prioridade:** Média | **Sprint:** 4 | **MoSCoW:** Must have | **Esforço:** 8h

#### 📝 Descrição:
O sistema deve permitir que funcionários configurem o envio automático de lembretes (vacinas a vencer, consultas agendadas) para os tutores via WhatsApp e e-mail.

#### ✅ Critérios de Aceite:
- [ ] Lembrete disparado automaticamente X dias antes da vacina vencer (configurável).
- [ ] Lembrete de confirmação de consulta enviado 24h antes.
- [ ] Registro de lembretes enviados no histórico do animal.

#### 🏗️ Impacto Arquitetural:
- **Camada de Dados (Etapa 3)** → Card #10
- **Camada de Negócio (Etapa 4)** → Card #14
- **Camada de Aplicação (Etapa 4)** → Card #13
- **Camada de Apresentação (Etapa 5)** → Card #17

#### 📊 Decomposição de Tarefas:

| Task | Descrição | Card | Est. | Responsável |
| :--- | :--- | :---: | :---: | :--- |
| **RF009-T1** | Tabela `LembreteEnviado` para histórico | #10 | 1h | _________ |
| **RF009-T2** | Classe `LembreteService.cs` com lógica de disparo | #14 | 3h | _________ |
| **RF009-T3** | Configuração de templates de mensagem | #14 | 2h | _________ |
| **RF009-T4** | Tela de configuração de lembretes | #17 | 2h | _________ |

**⏱️ Total estimado:** 8 horas

---

### RF010 — Gerar Relatórios Gerenciais
**Prioridade:** Média | **Sprint:** 4 | **MoSCoW:** Should have | **Esforço:** 12h

#### 📝 Descrição:
O sistema deve permitir que o proprietário visualize relatórios gerenciais sobre o desempenho da clínica: serviços mais realizados, produtos mais vendidos, faturamento por serviço e por categoria de produto, e desempenho dos profissionais.

#### ✅ Critérios de Aceite:
- [ ] Filtros por data inicial, data final e veterinário (opcional).
- [ ] Exportação em PDF ou CSV.
- [ ] Gráficos de evolução mensal (barras e linhas).
- [ ] Relatório de faturamento para fechamento mensal.

#### 🏗️ Impacto Arquitetural:
- **Camada de Dados (Etapa 3)** → Card #10 (views agregadas)
- **Camada de Negócio (Etapa 4)** → Card #14
- **Camada de Aplicação (Etapa 4)** → Card #13
- **Camada de Apresentação (Etapa 5)** → Card #17
- **Análise de Dados (Etapa 7)** → Card #26 e #27

#### 📊 Decomposição de Tarefas:

| Task | Descrição | Card | Est. | Responsável |
| :--- | :--- | :---: | :---: | :--- |
| **RF010-T1** | View SQL `vw_RelatorioAtendimentos` | #10 | 2h | _________ |
| **RF010-T2** | Script Python com pandas para análise | #26 | 4h | _________ |
| **RF010-T3** | Gráficos matplotlib (barras + linha) | #27 | 3h | _________ |
| **RF010-T4** | Tela `relatorio.html` com filtros | #17 | 3h | _________ |

**⏱️ Total estimado:** 12 horas

---

### RF011 — Exibir Painel Gerencial (Dashboard)
**Prioridade:** Média | **Sprint:** 4 | **MoSCoW:** Must have | **Esforço:** 10h

#### 📝 Descrição:
O sistema deve permitir que o proprietário visualize um painel com indicadores operacionais e financeiros da clínica: número de atendimentos do dia, alertas de estoque, produtos a vencer, faturamento do mês.

#### ✅ Critérios de Aceite:
- [ ] Dashboard exibido imediatamente após o login do proprietário.
- [ ] Indicadores atualizados em tempo real (ou ao recarregar).
- [ ] Alertas de estoque baixo e validade próxima visíveis no painel.
- [ ] Badge numérico com pendências de cada módulo.

#### 🏗️ Impacto Arquitetural:
- **Camada de Dados (Etapa 3)** → Card #10
- **Camada de Aplicação (Etapa 4)** → Card #13
- **Camada de Apresentação (Etapa 5)** → Card #17

#### 📊 Decomposição de Tarefas:

| Task | Descrição | Card | Est. | Responsável |
| :--- | :--- | :---: | :---: | :--- |
| **RF011-T1** | View SQL `vw_IndicadoresDashboard` | #10 | 2h | _________ |
| **RF011-T2** | Controller `DashboardController.cs` | #13 | 2h | _________ |
| **RF011-T3** | Tela `dashboard.html` com cards de indicadores | #17 | 4h | _________ |
| **RF011-T4** | Integração alertas de estoque (RF005/RF006) | #17 | 2h | _________ |

**⏱️ Total estimado:** 10 horas

---

### RF012 — Permitir Acesso via Dispositivos Móveis
**Prioridade:** Alta | **Sprint:** 3 | **MoSCoW:** Must have | **Esforço:** 8h

#### 📝 Descrição:
O sistema deve permitir que veterinários acessem o sistema por dispositivos móveis para consulta de informações clínicas, garantindo responsividade em todas as telas.

#### ✅ Critérios de Aceite:
- [ ] Layout responsivo em resoluções mobile (375px) e desktop (1280px).
- [ ] Funcionalidades de consulta acessíveis sem degradação em celular.
- [ ] Menus e formulários adaptados para toque.

#### 🏗️ Impacto Arquitetural:
- **Camada de Apresentação (Etapa 5)** → Card #17 e #18

#### 📊 Decomposição de Tarefas:

| Task | Descrição | Card | Est. | Responsável |
| :--- | :--- | :---: | :---: | :--- |
| **RF012-T1** | Media queries CSS para todas as telas | #18 | 4h | _________ |
| **RF012-T2** | Testes de responsividade (375px e 1280px) | #18 | 2h | _________ |
| **RF012-T3** | Ajustes de UX para navegação em toque | #17 | 2h | _________ |

**⏱️ Total estimado:** 8 horas

---

### RF013 — Oferecer Recursos Básicos de Acessibilidade
**Prioridade:** Média | **Sprint:** 4 | **MoSCoW:** Should have | **Esforço:** 6h

#### 📝 Descrição:
O sistema deve permitir que tutores com necessidades especiais utilizem funcionalidades acessíveis, incorporando princípios básicos de LIBRAS e boas práticas de acessibilidade digital.

#### ✅ Critérios de Aceite:
- [ ] Atributos `alt` em todas as imagens e ícones.
- [ ] Contraste adequado (WCAG AA) em elementos críticos.
- [ ] Glossário de termos do sistema com sugestão de tradução em LIBRAS.
- [ ] Ícones visuais com descrição textual acessível.

#### 🏗️ Impacto Arquitetural:
- **Camada de Apresentação (Etapa 5)** → Card #17
- **LIBRAS (Etapa 8)** → Card #31

#### 📊 Decomposição de Tarefas:

| Task | Descrição | Card | Est. | Responsável |
| :--- | :--- | :---: | :---: | :--- |
| **RF013-T1** | Revisão de contraste e atributos `alt` | #17 | 2h | _________ |
| **RF013-T2** | Glossário de termos do sistema | #31 | 2h | _________ |
| **RF013-T3** | Proposta de ícones com descrição em LIBRAS | #31 | 2h | _________ |

**⏱️ Total estimado:** 6 horas

---

### RF014 — Cadastrar Tutores
**Prioridade:** Alta | **Sprint:** 2 | **MoSCoW:** Must have | **Esforço:** 17h

#### 📝 Descrição:
O sistema deve permitir que funcionários da recepção cadastrem, editem e consultem dados dos tutores, com validação de CPF e prevenção de duplicação.

#### ✅ Critérios de Aceite:
- [ ] Todos os campos obrigatórios devem ser validados (nome, CPF, telefone, e-mail, endereço).
- [ ] CPF deve seguir o formato válido (11 dígitos).
- [ ] Sistema deve impedir cadastro de CPF duplicado.
- [ ] Busca de tutores por nome (parcial), CPF ou telefone.
- [ ] Mensagem de sucesso/erro deve ser exibida ao usuário.

#### 🏗️ Impacto Arquitetural:
- **Camada de Dados (Etapa 3)** → Card #9 e #10
- **Camada de Negócio (Etapa 4)** → Card #14
- **Camada de Aplicação (Etapa 4)** → Card #13
- **Camada de Apresentação (Etapa 5)** → Card #17

#### 📊 Decomposição de Tarefas:

| Task | Descrição | Card | Est. | Responsável |
| :--- | :--- | :---: | :---: | :--- |
| **RF014-T1** | Criar entidade `Tutor` no DER com atributos | #9 | 1h | _________ |
| **RF014-T2** | Script DDL tabela `Tutor` com constraints | #10 | 1h | _________ |
| **RF014-T3** | Script de validação de CPF (função SQL) | #10 | 2h | _________ |
| **RF014-T4** | Criar índices em `Tutor` (nome, CPF, telefone) | #10 | 1h | _________ |
| **RF014-T5** | Classe `Tutor.cs` com encapsulamento | #14 | 2h | _________ |
| **RF014-T6** | Classe `TutorService.cs` com validações e busca | #14 | 3h | _________ |
| **RF014-T7** | Controller `TutorController.cs` (POST/GET) | #13 | 2h | _________ |
| **RF014-T8** | Tela `cadastro-tutor.html` responsiva | #17 | 3h | _________ |
| **RF014-T9** | Validação de formulário e autocomplete em JavaScript | #17 | 2h | _________ |

**⏱️ Total estimado:** 17 horas

---

### RF015 — Cadastrar Animais
**Prioridade:** Alta | **Sprint:** 2 | **MoSCoW:** Must have | **Esforço:** 13h

#### 📝 Descrição:
O sistema deve permitir que funcionários da recepção ou veterinários cadastrem, editem e consultem dados dos animais vinculados a um tutor, com campos de espécie, raça, sexo, data de nascimento e peso.

#### ✅ Critérios de Aceite:
- [ ] Animal deve estar obrigatoriamente vinculado a um tutor.
- [ ] Data de nascimento não pode ser futura.
- [ ] Peso deve ser numérico e maior que zero.
- [ ] Especies predefinidas no sistema (cachorro, gato, etc) e raças predefinidas por especie.
- [ ] Sistema deve permitir cadastrar múltiplos animais para o mesmo tutor.

#### 🏗️ Impacto Arquitetural:
- **Camada de Dados (Etapa 3)** → Card #9 e #10
- **Camada de Negócio (Etapa 4)** → Card #14
- **Camada de Aplicação (Etapa 4)** → Card #13
- **Camada de Apresentação (Etapa 5)** → Card #17

#### 📊 Decomposição de Tarefas:

| Task | Descrição | Card | Est. | Responsável |
| :--- | :--- | :---: | :---: | :--- |
| **RF015-T1** | Criar entidade `Animal` e `Raca` no DER | #9 | 1h | _________ |
| **RF015-T2** | Script DDL tabela `Animal` com FK para Tutor e `Raca` | #10 | 1h | _________ |
| **RF015-T3** | Classe `Animal.cs` com validações | #14 | 2h | _________ |
| **RF015-T4** | Classe `AnimalService.cs` | #14 | 2h | _________ |
| **RF015-T5** | Controller `AnimalController.cs` | #13 | 2h | _________ |
| **RF015-T6** | Tela `cadastro-animal.html` | #17 | 3h | _________ |
| **RF015-T7** | Integração com busca de tutor (autocomplete) | #17 | 2h | _________ |

**⏱️ Total estimado:** 13 horas

---

### RF016 — Vincular Animal ao Tutor
**Prioridade:** Alta | **Sprint:** 2 | **MoSCoW:** Must have | **Esforço:** 4h

#### 📝 Descrição:
O sistema deve permitir que funcionários autorizados associem um ou mais animais a um tutor, garantindo a integridade do vínculo e permitindo consulta pela relação tutor → animais.

#### ✅ Critérios de Aceite:
- [ ] Um tutor pode ter múltiplos animais vinculados.
- [ ] Um animal pertence a exatamente um tutor.
- [ ] Vinculação validada com constraint de FK no banco de dados.
- [ ] Consulta de animais do tutor disponível em todas as telas relevantes.

#### 🏗️ Impacto Arquitetural:
- **Camada de Dados (Etapa 3)** → Card #9 e #10 (relacionamento 1:N)
- **Camada de Negócio (Etapa 4)** → Card #14

#### 📊 Decomposição de Tarefas:

| Task | Descrição | Card | Est. | Responsável |
| :--- | :--- | :---: | :---: | :--- |
| **RF016-T1** | Definir relacionamento 1:N Tutor → Animal no DER | #9 | 1h | _________ |
| **RF016-T2** | Constraint FK `Animal.TutorId → Tutor.Id` | #10 | 1h | _________ |
| **RF016-T3** | Método `ObterAnimaisDoTutor(tutorId)` no Service | #14 | 2h | _________ |

**⏱️ Total estimado:** 4 horas

---

## 📚 Referências

SCHWABER, Ken; SUTHERLAND, Jeff. **The Scrum Guide**. Scrum.org, 2020.

COHN, Mike. **User Stories Applied: For Agile Software Development**. Addison-Wesley, 2004.

PRESSMAN, Roger S. **Engenharia de Software: Uma Abordagem Profissional**. 8ª ed. Porto Alegre: AMGH, 2016.