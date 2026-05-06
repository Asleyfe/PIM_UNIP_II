# Testes de Integração — TutorRepository

**Arquivo:** `tests/PetCare.Tests/Infrastructure/TutorRepositoryIntegrationTests.cs`  
**Banco de dados:** Supabase (PostgreSQL)  
**Tabela testada:** `tutor`

---

## Pré-requisitos

Antes de executar qualquer teste manualmente, verifique:

- [ ] A tabela `tutor` está criada no Supabase
- [ ] O arquivo `tests/PetCare.Tests/appsettings.test.json` contém a connection string correta
- [ ] O projeto compila sem erros (`dotnet build`)

### Formato do appsettings.test.json

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=db.SEU_PROJETO.supabase.co;Port=5432;Database=postgres;Username=postgres;Password=SUA_SENHA;SSL Mode=Require"
  }
}
```

---

## Como executar

### Todos os testes de integração

```bash
cd tests/PetCare.Tests
dotnet test --filter "FullyQualifiedName~Infrastructure"
```

### Um teste específico pelo nome

```bash
dotnet test --filter "DisplayName=NOME DO TESTE"
```

### Todos os testes do projeto

```bash
cd PIM_UNIP_II
dotnet test --logger "console;verbosity=normal"
```

---

## Testes documentados

---

### 1. Deve conectar ao banco sem lançar exceção

**Método:** `DeveConectarAoBanco`  
**O que valida:** A connection string está correta e o Supabase está acessível.

**Passos manuais:**
1. Abrir o Supabase e confirmar que o projeto está ativo
2. Verificar que a senha no `appsettings.test.json` está correta
3. Executar o teste

**Comando:**
```bash
dotnet test --filter "DisplayName=Deve conectar ao banco sem lançar exceção"
```

**Resultado esperado:** Aprovado — nenhuma exceção lançada.

---

### 2. Listar deve retornar lista (vazia ou não)

**Método:** `ListarDeveRetornarLista`  
**O que valida:** O repositório consegue executar `SELECT` na tabela `tutor` e retornar um objeto não nulo.

**Passos manuais:**
1. Garantir que a tabela `tutor` existe (mesmo que vazia)
2. Executar o teste

**Comando:**
```bash
dotnet test --filter "DisplayName=Listar deve retornar lista (vazia ou não)"
```

**Resultado esperado:** Aprovado — retorna lista (pode ser vazia).

---

### 3. ObterPorId com id inexistente deve retornar null

**Método:** `ObterPorIdInexistenteDeveRetornarNull`  
**O que valida:** Buscar por um `id` que não existe retorna `null` em vez de lançar exceção.

**Passos manuais:**
1. Confirmar que não existe nenhum tutor com `id = 999999` no banco
2. Executar o teste

**Comando:**
```bash
dotnet test --filter "DisplayName=ObterPorId com id inexistente deve retornar null"
```

**Resultado esperado:** Aprovado — retorna `null`.

---

### 4. ExistePorCpf com CPF inexistente deve retornar false

**Método:** `ExistePorCpfInexistenteDeveRetornarFalse`  
**O que valida:** Verificar unicidade de CPF retorna `false` quando o CPF não está cadastrado.

**Passos manuais:**
1. Confirmar que não existe tutor com CPF `00000000000` na tabela
2. Executar o teste

**Comando:**
```bash
dotnet test --filter "DisplayName=ExistePorCpf com CPF inexistente deve retornar false"
```

**Resultado esperado:** Aprovado — retorna `false`.

---

### 5. Deve inserir um tutor e recuperá-lo do banco

**Método:** `DeveInserirTutorERecuperarDoBanco`  
**O que valida:** Ciclo completo de persistência — inserir, recuperar e verificar os dados no banco real.

**Dados usados no teste:**

| Campo     | Valor                       |
|-----------|-----------------------------|
| Nome      | João da Silva Teste         |
| CPF       | 52998224725                 |
| Telefone  | 62999990000                 |
| Email     | joao.teste@petcare.com      |
| Rua       | Rua T-30                    |
| Número    | 100                         |
| Bairro    | Setor Bueno                 |
| Cidade    | Goiânia                     |
| Estado    | GO                          |

**Passos manuais equivalentes (via Supabase ou psql):**

```sql
-- 1. Inserir o tutor
INSERT INTO tutor (nome, cpf, telefone, email, rua, numero, bairro, cidade, estado)
VALUES (
    'João da Silva Teste',
    '52998224725',
    '62999990000',
    'joao.teste@petcare.com',
    'Rua T-30',
    '100',
    'Setor Bueno',
    'Goiânia',
    'GO'
)
RETURNING id, created_at;

-- 2. Recuperar pelo Id retornado acima (substitua pelo id real)
SELECT * FROM tutor WHERE id = <id_retornado>;

-- 3. Verificar que o CPF existe
SELECT COUNT(1) FROM tutor WHERE cpf = '52998224725';

-- 4. Limpeza — remover o registro de teste
DELETE FROM tutor WHERE cpf = '52998224725';
```

**O que o teste automatizado verifica:**
- `Id > 0` — banco gerou o identificador
- `CreatedAt != default` — banco preencheu a data de criação
- `Nome`, `Cpf` e `Email` batem com os dados enviados
- `ObterPorId` retorna o tutor com os mesmos dados
- `Endereco.Cidade == "Goiânia"` — o endereço foi persistido corretamente
- `ExistePorCpf` retorna `true` após a inserção
- O registro é removido ao final (limpeza automática via `finally`)

**Comando:**
```bash
dotnet test --filter "DisplayName=Deve inserir um tutor e recuperá-lo do banco"
```

**Resultado esperado:** Aprovado — tutor inserido, recuperado e removido com sucesso.

---

## Estrutura dos arquivos envolvidos

```
tests/PetCare.Tests/
├── appsettings.test.json                          # connection string (não vai pro git)
└── Infrastructure/
    └── TutorRepositoryIntegrationTests.cs         # testes documentados aqui

src/PetCare.Infrastructure/
├── Data/
│   ├── IConnectionFactory.cs
│   └── NpgsqlConnectionFactory.cs
└── Repositories/
    └── TutorRepository.cs
```
