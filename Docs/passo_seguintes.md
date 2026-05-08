# 🚀 Próximos Passos - PetCare Web

Este documento detalha o que ainda falta ser implementado no sistema, organizado por camadas e módulos, para que o projeto saia do código de domínio e chegue até as telas funcionais.

---

## 📋 Checklist de Camadas por Módulo

Legenda: ✅ Concluído | ⏳ Pendente

| Módulo | Repositórios (Infra) | Services & DTOs (App) | Controllers (Web) | Views (HTML/JS) |
| :--- | :---: | :---: | :---: | :---: |
| **Tutores** | ✅ | ✅ | ✅ | ⏳ |
| **Animais** | ✅ | ✅ | ✅ | ⏳ |
| **Estoque** | ⏳ | ⏳ | ⏳ | ⏳ |
| **Agendamento** | ⏳ | ⏳ | ⏳ | ⏳ |
| **Prontuário** | ⏳ | ⏳ | ⏳ | ⏳ |
| **Vendas** | ⏳ | ⏳ | ⏳ | ⏳ |

---

## 🏗️ Detalhamento do que Criar em cada Camada

### 1. Camada de Infraestrutura (Repositories)
Responsável pela persistência e leitura de dados no banco PostgreSQL (via Dapper).
*   **Módulo Estoque**: `ProdutoRepository` e `MovimentacaoEstoqueRepository`.
*   **Módulo Atendimento**: `AgendamentoRepository` e `ProntuarioRepository`.
*   **Módulo Vendas**: `VendaRepository` e `ItemVendaRepository`.

### 2. Camada de Aplicação (Services & DTOs)
Responsável por orquestrar o fluxo de dados e aplicar validações de caso de uso.
*   **DTOs**: Objetos simplificados para entrada (`CreateDto`) e saída (`ResponseDto`).
*   **Services**: `AgendamentoService`, `EstoqueService`, `VendaService`, etc.

### 3. Camada Web (Controllers)
Ponto de entrada do sistema.
*   **Controllers de API**: Endpoints para comunicação assíncrona com o Front-end.
*   **Controllers MVC**: Ações que entregam as páginas HTML (Views) ao navegador.

### 4. Camada de Apresentação (Views .cshtml)
Interfaces de usuário baseadas em Razor Pages.
*   **Formulários**: Cadastro de tutores, animais, agendamentos e vendas.
*   **Listagens**: Tabelas com filtros e ações (editar/excluir).
*   **Dashboard**: Painel de indicadores de estoque e agenda.

---

## 🎯 Roteiro de Prioridade Sugerido

1.  **Interfaces de Usuário (Tutores e Animais)**:
    *   Criar as telas de listagem e cadastro para os módulos cujos back-ends já estão prontos.
2.  **Fluxo de Agendamento (Core)**:
    *   Implementar Repositório, Service e Controller para agendamentos.
    *   Criar a tela de agenda do veterinário.
3.  **Módulo de Estoque e Alertas**:
    *   Implementar persistência de produtos.
    *   Criar alertas visuais para estoque baixo e validade próxima.
4.  **Fechamento de Vendas e Financeiro**:
    *   Integrar produtos e clientes no registro de vendas.

---
*Documento gerado para guiar a transição da Sprint 2 para a Sprint 3.*
