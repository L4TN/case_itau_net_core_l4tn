# Case Itau Asset Management

Case tecnico para a vaga de Desenvolvedor Pleno no Itau Asset Management.

O desafio consistia em receber um backend legado (.NET Core 3.1 com SQLite) contendo uma API de fundos com diversos problemas de qualidade, seguranca e arquitetura, e refatora-lo aplicando boas praticas, corrigir o bug reportado, e criar um frontend que consuma a API.

---

## O que foi entregue

### 1. Refatoracao completa do backend

O codigo legado tinha um unico controller com SQL inline, SQL Injection em todos os endpoints, connection leak, sem validacao, sem autenticacao, sem testes e sem arquitetura. Foi migrado para Clean Architecture com .NET 10, SQL Server, EF Core, JWT, Redis, testes unitarios e Docker.

### 2. Bug corrigido

**Problema reportado:** "Apos a inclusao de um novo fundo via API, os metodos GET estao retornando erro."

**Causa raiz:** O POST inseria `NULL` na coluna `PATRIMONIO`. O GET fazia `decimal.Parse(reader[4].ToString())` — quando `PATRIMONIO` era NULL, `reader[4].ToString()` retornava string vazia, e `decimal.Parse("")` lancava `FormatException`, crasheando a API para todos os fundos.

**Solucao:** Alem de corrigir o parse, a coluna `PATRIMONIO` foi removida da tabela `FUNDO` e substituida por duas novas tabelas (`Tb_Movimentacao_Fundo` e `Tb_Posicao_Fundo`) que registram cada movimentacao individualmente e mantem o historico de posicao diaria do patrimonio. Isso elimina o bug na raiz e adiciona rastreabilidade completa.

### 3. Frontend Angular

Aplicacao web em Angular 15 (Nebular/ngx-admin) que consome todos os endpoints da API: autenticacao, cadastro de fundos, movimentacao patrimonial, consulta de posicoes e tipos de fundo.

---

## Refatoracao do Modelo de Dados

O modelo legado tinha apenas 2 tabelas (`TIPO_FUNDO` e `FUNDO`) com o patrimonio armazenado como uma coluna `NUMERIC` na propria tabela de fundos, sem historico nem rastreabilidade.

O novo modelo normaliza os dados em 5 tabelas:

```
Tb_Tipo_Fundo          Tipos de fundo (RENDA FIXA, ACOES, MULTI MERCADO)
  |
  └── Tb_Fundo         Cadastro de fundos (codigo unico, CNPJ unico)
        |
        ├── Tb_Movimentacao_Fundo    Registro individual de cada aporte/resgate com data e valor
        |
        └── Tb_Posicao_Fundo         Posicao diaria do patrimonio com controle de concorrencia (RowVersion)

Tb_Feature_Flag        Feature flags para toggle de funcionalidades (ex: cache Redis)
```

**O que mudou:**

| Aspecto | Legado | Refatorado |
|---|---|---|
| Patrimonio | Coluna `NUMERIC` na tabela `FUNDO` | Tabela `Tb_Posicao_Fundo` com snapshot diario |
| Movimentacoes | `UPDATE FUNDO SET PATRIMONIO = PATRIMONIO + valor` | Tabela `Tb_Movimentacao_Fundo` com registro individual |
| Historico | Sem historico, so o valor atual | Evolucao patrimonial dia a dia |
| Concorrencia | Nenhuma | `RowVersion` na posicao para controle otimista |
| Tipos de fundo | Existia no banco mas sem entidade C# | Entidade `TbTipoFundo` com navegacao |

---

## Arquitetura

O backend segue **Clean Architecture** com 4 camadas e dependencias unidirecionais:

```
CaseItau_Backend/
├── src/
│   ├── CaseItau.API           -> Apresentacao (Controllers, Middlewares, DI, Swagger)
│   ├── CaseItau.Application   -> Logica de aplicacao (Services, DTOs, Validators, AutoMapper)
│   ├── CaseItau.Domain        -> Dominio (Entidades, Interfaces, Excecoes)
│   └── CaseItau.Infra         -> Infraestrutura (EF Core, SQL Server, Repositorios, Migrations)
└── tests/
    └── CaseItau.Tests         -> Testes unitarios (xUnit, Moq, FluentAssertions)

CaseItau_FrontEnd/             -> Frontend Angular 15 (Nebular/ngx-admin)
```

```
API -> Application -> Domain
Infra -> Domain
Tests -> Application, Domain
```

---

## Stack Tecnologica

### Backend

| Categoria | Tecnologia |
|---|---|
| Runtime | .NET 10 |
| Banco de dados | SQL Server 2022 |
| ORM | Entity Framework Core 10 com Fluent API |
| Autenticacao | JWT Bearer com criptografia AES-256-CBC |
| Validacao | FluentValidation 11 com validacao real de CNPJ |
| Mapeamento | AutoMapper 12 |
| Cache | Redis 7 com Polly (Retry + Circuit Breaker) |
| Logging | Serilog com sink para Console e AWS CloudWatch |
| Documentacao | Swagger / Swashbuckle com suporte a JWT |
| Testes | xUnit, Moq, FluentAssertions — 56 testes |
| Containerizacao | Docker + Docker Compose |
| CI/CD | GitHub Actions |
| Resiliencia | Polly 8 (Retry com backoff exponencial + Circuit Breaker) |
| Rate Limiting | Fixed Window (100 req/min) |
| Health Checks | SQL Server + Redis |
| Idempotencia | Middleware com cache Redis (24h) |
| Feature Flags | CRUD via API com toggle |
| Concorrencia | RowVersion (optimistic concurrency) nas posicoes |

### Frontend

| Categoria | Tecnologia |
|---|---|
| Framework | Angular 15 |
| UI | Nebular (ngx-admin) |
| HTTP | HttpClient com interceptors JWT |

---

## Como Executar

### Pre-requisitos

- [Docker](https://www.docker.com/) com Docker Compose

### Subindo o ambiente completo

```bash
docker compose up --build
```

Sobe 4 containers:

| Container | Porta | Descricao |
|---|---|---|
| `caseitau-sqlserver` | 1433 | SQL Server 2022 |
| `caseitau-redis` | 6379 | Redis 7 (cache) |
| `caseitau-api` | 5000 | API .NET 10 |
| `caseitau-frontend` | 4200 | Frontend Angular |

O banco e criado automaticamente via EF Core Migrations na inicializacao da API, incluindo seed dos tipos de fundo (RENDA FIXA, ACOES, MULTI MERCADO) e feature flag do cache Redis.

### Acessando os servicos

| Servico | URL |
|---|---|
| Swagger UI | http://localhost:5000/swagger |
| Frontend | http://localhost:4200 |
| Health Check | http://localhost:5000/health |

### Executando sem Docker

**Backend:**

```bash
cd CaseItau_Backend
dotnet restore
dotnet build
dotnet run --project src/CaseItau.API
```

Requer SQL Server e Redis rodando localmente (ou suba apenas a infra via Docker):

```bash
docker compose up sqlserver redis -d
```

**Frontend:**

```bash
cd CaseItau_FrontEnd
npm install
npm start
```

Acesse http://localhost:4200

---

## Endpoints da API

### Autenticacao

| Metodo | Rota | Descricao |
|---|---|---|
| POST | `/api/auth/login` | Autentica e retorna token JWT criptografado (publico) |

Credenciais: `admin` / `admin123`

### Fundos (requer JWT)

| Metodo | Rota | Descricao |
|---|---|---|
| GET | `/api/fundo` | Lista todos os fundos (suporta `?page=1&pageSize=20`) |
| GET | `/api/fundo/{codigo}` | Retorna detalhes de um fundo pelo codigo |
| POST | `/api/fundo` | Cadastra um novo fundo |
| PUT | `/api/fundo/{codigo}` | Edita um fundo existente |
| DELETE | `/api/fundo/{codigo}` | Exclui um fundo |

### Movimentacoes (requer JWT)

| Metodo | Rota | Descricao |
|---|---|---|
| POST | `/api/movimentacao/{codigoFundo}` | Registra aporte ou resgate no patrimonio |
| GET | `/api/movimentacao/{codigoFundo}` | Lista historico de movimentacoes |
| GET | `/api/movimentacao/{codigoFundo}/evolucao-patrimonial` | Evolucao diaria do patrimonio |

### Tipos de Fundo (requer JWT)

| Metodo | Rota | Descricao |
|---|---|---|
| GET | `/api/tipofundo` | Lista tipos de fundo cadastrados |

### Feature Flags

| Metodo | Rota | Descricao |
|---|---|---|
| GET | `/api/featureflag` | Lista todas as flags (requer JWT) |
| GET | `/api/featureflag/{chave}/enabled` | Verifica se esta habilitada (publico) |
| PUT | `/api/featureflag/{chave}/toggle?habilitado=true` | Habilita/desabilita (requer JWT) |

### Health Check

| Metodo | Rota | Descricao |
|---|---|---|
| GET | `/health` | Status do SQL Server e Redis (publico) |

---

## Testes

56 testes unitarios cobrindo a camada de aplicacao:

```bash
cd CaseItau_Backend
dotnet test
```

| Categoria | Escopo | Testes |
|---|---|---|
| Services | FundoService, MovimentacaoService, TipoFundoService, FeatureFlagService | 37 |
| Validators | CreateFundo, UpdateFundo, CreateMovimentacao, validacao de CNPJ | 16 |
| Mappings | Configuracao do AutoMapper | 3 |

---

## Problemas identificados e corrigidos no legado

| # | Problema | Severidade | Solucao aplicada |
|---|---|---|---|
| 1 | SQL Injection em todos os endpoints | Critica | EF Core com queries parametrizadas |
| 2 | Connection Leak (SQLiteConnection sem Dispose) | Critica | DI com lifecycle gerenciado |
| 3 | .NET Core 3.1 (sem suporte desde dez/2022) | Alta | .NET 10 |
| 4 | SQLite em producao (sem concorrencia) | Alta | SQL Server 2022 |
| 5 | Connection string hardcoded 6x | Alta | appsettings + DI |
| 6 | Sem arquitetura (data access no controller) | Alta | Clean Architecture 4 camadas |
| 7 | Sem validacao de input | Alta | FluentValidation + CNPJ real |
| 8 | Sem autenticacao | Alta | JWT Bearer + AES-256-CBC |
| 9 | Sem testes | Alta | 56 testes unitarios |
| 10 | Bug: `decimal.Parse(null)` no GET apos POST | Media | Tabela separada de posicoes |
| 11 | Sem DTOs (entidade exposta na API) | Media | DTOs de request/response |
| 12 | Sem tratamento de erro | Media | ExceptionMiddleware centralizado |
| 13 | Sem async/await | Media | Todos os endpoints async |
| 14 | POST retorna void | Media | Retorna 201 Created |
| 15 | GET retorna null | Media | Retorna 404 Not Found |
| 16 | Sem CORS | Media | CORS configurado para Angular |
| 17 | Sem Swagger | Baixa | Swagger com suporte a JWT |
| 18 | Patrimonio como coluna unica | Design | Tabelas de movimentacao e posicao |

---

## Configuracao por Ambiente

| Arquivo | Ambiente | Caracteristicas |
|---|---|---|
| `appsettings.json` | Base | Connection string, JWT, Redis |
| `appsettings.Development.json` | Dev | Debug logging, JWT 120min, `Database:ResetOnStartup` |
| `appsettings.Staging.json` | UAT | Placeholders para CI/CD |
| `appsettings.Production.json` | Producao | Warning logging, JWT 30min, CloudWatch habilitado |

---

## Postman

A collection `CaseItau_Backend/CaseItau.postman_collection.json` inclui todos os endpoints com scripts que salvam o token automaticamente.

Importe o environment `CaseItau_Backend/CaseItau.postman_environment.json` para as variaveis pre-configuradas.
