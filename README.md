# Case Itau Asset Management

Plataforma fullstack para gerenciamento de fundos de investimento, desenvolvida como case tecnico para a vaga de Desenvolvedor Pleno no Itau Asset Management.

O projeto foi construido a partir de um backend legado (.NET Core 3.1, SQLite, SQL inline com vulnerabilidades) e migrado para uma arquitetura moderna com Clean Architecture, .NET 8, SQL Server, JWT, Redis, testes unitarios e frontend Angular.

---

## Arquitetura

O backend segue os principios de **Clean Architecture** com separacao em 4 camadas e dependencias unidirecionais:

```
CaseItau_Backend/
├── src/
│   ├── CaseItau.API           -> Apresentacao (Controllers, Middlewares, DI)
│   ├── CaseItau.Application   -> Logica de aplicacao (Services, DTOs, Validators, AutoMapper)
│   ├── CaseItau.Domain        -> Dominio (Entidades, Interfaces, Excecoes)
│   └── CaseItau.Infra         -> Infraestrutura (EF Core, Repositorios, Migrations)
└── tests/
    └── CaseItau.Tests         -> Testes unitarios

CaseItau_FrontEnd/             -> Frontend Angular 15
```

**Fluxo de dependencias:**

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
| Runtime | .NET 8 (LTS) |
| Banco de dados | SQL Server 2022 |
| ORM | Entity Framework Core 8 |
| Autenticacao | JWT Bearer com criptografia AES-256-CBC |
| Validacao | FluentValidation 11 |
| Mapeamento | AutoMapper 12 |
| Cache | Redis 7 com Polly (Retry + Circuit Breaker) |
| Logging | Serilog com sink para Console e AWS CloudWatch |
| Documentacao | Swagger / Swashbuckle |
| Testes | xUnit, Moq, FluentAssertions |
| Containerizacao | Docker + Docker Compose |
| CI/CD | GitHub Actions |
| Resiliencia | Polly 8 (Retry com backoff exponencial + Circuit Breaker) |
| Rate Limiting | Fixed Window (100 req/min) |
| Health Checks | SQL Server + Redis |
| Idempotencia | Middleware com cache Redis (24h) |
| Feature Flags | CRUD via API com toggle |

### Frontend

| Categoria | Tecnologia |
|---|---|
| Framework | Angular 15 |
| UI | Nebular (ngx-admin) |
| HTTP | HttpClient com interceptors |

---

## Estrutura do Backend

### CaseItau.Domain

Camada mais interna, sem dependencias externas.

- **Entities** — `TbFundo`, `TbTipoFundo`, `TbMovimentacaoFundo`, `TbPosicaoFundo`, `TbFeatureFlag`
- **Interfaces** — Contratos de repositorio (`IFundoRepository`, `ITipoFundoRepository`, etc.) e `IUnitOfWork`
- **Exceptions** — `DomainException` (400) e `NotFoundException` (404)

### CaseItau.Application

Logica de negocio e orquestracao.

- **Services** — `FundoService` (CRUD completo com validacao de duplicidade de codigo/CNPJ), `MovimentacaoService` (controle de posicao diaria, validacao de patrimonio negativo), `AuthService` (JWT + AES), `TipoFundoService`, `FeatureFlagService`
- **DTOs** — Request/Response separados para cada endpoint
- **Validators** — FluentValidation com validacao real de CNPJ (digitos verificadores)
- **Mappings** — AutoMapper com mapeamentos explicitos entre entidades e DTOs

### CaseItau.Infra

Integracao com SQL Server via EF Core.

- **Data** — `DboContext`, `BaseDbContext`, Fluent API mappings, `UnitOfWork`
- **Repositories** — Implementacoes concretas dos contratos do Domain
- **Migrations** — Criacao automatica do banco e seed de dados (tipos de fundo + feature flag)

### CaseItau.API

Camada de apresentacao e configuracao.

- **Controllers** — `FundoController`, `AuthController`, `MovimentacaoController`, `TipoFundoController`, `FeatureFlagController`, `DiagnosticsController`
- **Middlewares** — `ExceptionMiddleware` (tratamento centralizado com status codes adequados), `IdempotencyMiddleware` (cache de respostas por Idempotency-Key)
- **Extensions** — `DependencyInjection` centralizado com EF, repos, services, JWT, Redis, health checks
- **Services** — `RedisCacheService` com Polly (retry + circuit breaker)

---

## Como Executar

### Pre-requisitos

- [Docker](https://www.docker.com/) com Docker Compose

### Subindo o ambiente completo

```bash
docker compose up --build
```

Esse comando sobe 4 containers:

| Container | Porta | Descricao |
|---|---|---|
| `caseitau-sqlserver` | 1433 | SQL Server 2022 |
| `caseitau-redis` | 6379 | Redis 7 (cache) |
| `caseitau-api` | 5000 | API .NET 8 |
| `caseitau-frontend` | 4200 | Frontend Angular |

O banco e criado automaticamente via EF Core Migrations na inicializacao da API.

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

Requer SQL Server e Redis rodando localmente (ou via Docker apenas para infra):

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

| Metodo | Rota | Descricao | Auth |
|---|---|---|---|
| POST | `/api/auth/login` | Autentica e retorna token JWT criptografado | Publico |

Credenciais padrão: `admin` / `admin123`

### Fundos

| Metodo | Rota | Descricao | Auth |
|---|---|---|---|
| GET | `/api/fundo` | Lista todos (suporta paginacao com `?page=1&pageSize=20`) | JWT |
| GET | `/api/fundo/{codigo}` | Busca por codigo | JWT |
| POST | `/api/fundo` | Cria novo fundo | JWT |
| PUT | `/api/fundo/{codigo}` | Atualiza fundo | JWT |
| DELETE | `/api/fundo/{codigo}` | Remove fundo | JWT |

### Movimentacoes

| Metodo | Rota | Descricao | Auth |
|---|---|---|---|
| POST | `/api/movimentacao/{codigoFundo}` | Registra movimentacao (aporte/resgate) | JWT |
| GET | `/api/movimentacao/{codigoFundo}` | Lista movimentacoes do fundo | JWT |
| GET | `/api/movimentacao/{codigoFundo}/evolucao-patrimonial` | Evolucao diaria do patrimonio | JWT |

### Tipos de Fundo

| Metodo | Rota | Descricao | Auth |
|---|---|---|---|
| GET | `/api/tipofundo` | Lista tipos de fundo | JWT |

### Feature Flags

| Metodo | Rota | Descricao | Auth |
|---|---|---|---|
| GET | `/api/featureflag` | Lista todas as flags | JWT |
| GET | `/api/featureflag/{chave}` | Busca flag por chave | JWT |
| GET | `/api/featureflag/{chave}/enabled` | Verifica se esta habilitada | Publico |
| PUT | `/api/featureflag/{chave}/toggle?habilitado=true` | Habilita/desabilita flag | JWT |

### Health Check

| Metodo | Rota | Descricao | Auth |
|---|---|---|---|
| GET | `/health` | Status do SQL Server e Redis | Publico |

---

## Testes

56 testes unitarios cobrindo services, validators e AutoMapper:

```bash
cd CaseItau_Backend
dotnet test
```

| Categoria | Escopo | Quantidade |
|---|---|---|
| Services | FundoService, MovimentacaoService, TipoFundoService, FeatureFlagService | 37 |
| Validators | CreateFundo, UpdateFundo, CreateMovimentacao, CNPJ | 16 |
| Mappings | MappingProfile (configuracao valida) | 3 |

---

## Configuracao por Ambiente

| Arquivo | Ambiente | Caracteristicas |
|---|---|---|
| `appsettings.json` | Base | Connection string, JWT, Redis, AWS |
| `appsettings.Development.json` | Desenvolvimento | Debug logging, JWT 120min, Redis 30s, `Database:ResetOnStartup` |
| `appsettings.Staging.json` | UAT | Placeholders para CI/CD, CloudWatch habilitado |
| `appsettings.Production.json` | Producao | Warning logging, JWT 30min, Redis 300s, Encrypt=true |

---

## Migracao do Legado

O projeto partiu de um backend legado com os seguintes problemas criticos:

| Problema | Legado | Solucao |
|---|---|---|
| SQL Injection | Concatenacao direta de input | EF Core com queries parametrizadas |
| Connection Leak | `SQLiteConnection` sem Dispose | DI com lifecycle gerenciado |
| .NET 3.1 | Fim de suporte dez/2022 | .NET 8 LTS |
| SQLite | Sem concorrencia | SQL Server 2022 |
| Sem arquitetura | Tudo no controller | Clean Architecture 4 camadas |
| Sem validacao | Aceita qualquer input | FluentValidation + CNPJ real |
| Sem autenticacao | API publica | JWT Bearer + AES-256-CBC |
| Sem testes | Zero cobertura | 56 testes unitarios |
| Bug patrimonio NULL | `decimal.Parse(null)` crasheava | Tabela separada de posicoes |
| Sem tratamento de erro | 500 generico | ExceptionMiddleware com status codes |

---

## Postman

A collection `CaseItau.postman_collection.json` inclui todos os endpoints com scripts que salvam o token automaticamente na variavel `{{token}}`.

Importe o environment `CaseItau.postman_environment.json` para ter as variaveis pre-configuradas.
