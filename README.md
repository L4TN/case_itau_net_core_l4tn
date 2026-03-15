# Case de engenharia Itau - .Net (Itau Asset Management)

Este fork consiste no desafio recebido de um backend legado (.NET Core 3.1 com SQLite) contendo uma API de fundos.

Os pontos propostos foram:

- Refatorar o código utilizando boas práticas, bibliotecas adequadas e padrões de projeto.
- Identificar e corrigir o bug: após inclusão de um novo fundo via API, os métodos `GET` retornavam erro.
- Criar uma aplicação web em **Angular** que consuma todos os métodos da API.

---

## O que desenvolvi

### 1. Refatoração do backend

O código legado tinha SQL Injection em todos os endpoints, connection leak, sem validações, sem autenticação, sem testes e sem arquitetura. Migrei para Clean Architecture com .NET 10 (LTS), SQL Server, EF Core, JWT, Redis, 56 testes unitários e Docker.

### 2. Bug corrigido

O POST inseria `NULL` na coluna `PATRIMONIO`. O GET fazia `decimal.Parse(reader[4].ToString())` — quando NULL, `decimal.Parse("")` lançava `FormatException`, crasheando a API para todos os fundos.

**Indo além:** a coluna `PATRIMONIO` foi removida da tabela `FUNDO` e substituída por duas novas tabelas: `Tb_Movimentacao_Fundo` (registro individual de cada aporte/resgate) e `Tb_Posicao_Fundo` (snapshot diário do patrimônio). Isso normaliza a origem do dado, introduz rastreabilidade histórica e elimina o bug na raiz.

### 3. Frontend Angular

Aplicação web em Angular 15 (Nebular/ngx-admin) que consome todos os endpoints da API: autenticação, cadastro de fundos, movimentação patrimonial e consulta de posições.

---

## Modelo de Dados

```mermaid
erDiagram
    Tb_Tipo_Fundo ||--o{ Tb_Fundo : "1:N"
    Tb_Fundo ||--o{ Tb_Movimentacao_Fundo : "1:N"
    Tb_Fundo ||--o{ Tb_Posicao_Fundo : "1:N"

    Tb_Tipo_Fundo {
        int Id PK
        varchar Nm_Tipo_Fundo
    }

    Tb_Fundo {
        int Id PK
        varchar Cd_Fundo UK
        varchar Nm_Fundo
        varchar Nr_Cnpj UK
        int Id_Tipo_Fundo FK
    }

    Tb_Movimentacao_Fundo {
        int Id PK
        int Id_Fundo FK
        datetime Dt_Movimentacao
        decimal Vlr_Movimentacao
    }

    Tb_Posicao_Fundo {
        int Id PK
        int Id_Fundo FK
        datetime Dt_Posicao
        decimal Vlr_Patrimonio
        timestamp Row_Version
    }

    Tb_Feature_Flag {
        int Id PK
        varchar Ds_Chave UK
        bit Fl_Habilitado
        varchar Ds_Descricao
        nvarchar Json_Config
        datetime Dt_Criacao
        datetime Dt_Atualizacao
    }
```

---

## Arquitetura

```
CaseItau_Backend/
├── src/
│   ├── CaseItau.API           -> Controllers, Middlewares, DI, Swagger
│   ├── CaseItau.Application   -> Services, DTOs, Validators, AutoMapper
│   ├── CaseItau.Domain        -> Entidades, Interfaces, Exceções
│   └── CaseItau.Infra         -> EF Core, Repositórios, Migrations
└── tests/
    └── CaseItau.Tests         -> 56 testes (xUnit, Moq, FluentAssertions)

CaseItau_FrontEnd/             -> Angular 15 (Nebular/ngx-admin)
```

![Clean Architecture](https://substackcdn.com/image/fetch/f_auto,q_auto:good,fl_progressive:steep/https%3A%2F%2Fsubstack-post-media.s3.amazonaws.com%2Fpublic%2Fimages%2F163415ba-cbed-4f04-8539-3bc1c3a6fef3_1938x1246.png)

---

## Stack

| Backend | Frontend |
|---|---|
| .NET 10 (LTS), SQL Server 2022, EF Core 10 | Angular 15, Nebular/ngx-admin |
| JWT Bearer + AES-256-CBC, FluentValidation, AutoMapper | HttpClient com interceptors JWT |
| Redis + Polly (Retry + Circuit Breaker) | |
| Serilog (Console + AWS CloudWatch) | |
| Swagger, Docker, GitHub Actions, Rate Limiting | |
| Health Checks, Idempotência, Feature Flags, RowVersion | |

---

## Como Executar

### Com Docker (recomendado)

```bash
docker compose up --build
```

| Serviço | URL |
|---|---|
| Swagger UI | http://localhost:5000/swagger |
| Frontend | http://localhost:4200 |
| Health Check | http://localhost:5000/health |

O banco é criado automaticamente via Migrations com seed de tipos de fundo e fundos de exemplo.

### Sem Docker

```bash
docker compose up sqlserver redis -d

cd CaseItau_Backend && dotnet run --project src/CaseItau.API

cd CaseItau_FrontEnd && npm install && npm start
```

---

## Endpoints

Credenciais: `admin` / `admin123`

| Método | Rota | Descrição | Auth |
|---|---|---|---|
| POST | `/api/auth/login` | Retorna token JWT criptografado | Público |
| GET | `/api/fundo` | Lista fundos (suporta `?page=1&pageSize=20`) | JWT |
| GET | `/api/fundo/{codigo}` | Detalhes de um fundo | JWT |
| POST | `/api/fundo` | Cadastra fundo | JWT |
| PUT | `/api/fundo/{codigo}` | Edita fundo | JWT |
| DELETE | `/api/fundo/{codigo}` | Exclui fundo | JWT |
| POST | `/api/movimentacao/{codigoFundo}` | Registra aporte/resgate | JWT |
| GET | `/api/movimentacao/{codigoFundo}` | Histórico de movimentações | JWT |
| GET | `/api/movimentacao/{codigoFundo}/evolucao-patrimonial` | Evolução patrimonial diária | JWT |
| GET | `/api/tipofundo` | Lista tipos de fundo | JWT |
| GET | `/api/featureflag` | Lista feature flags | JWT |
| PUT | `/api/featureflag/{chave}/toggle?habilitado=true` | Toggle de flag | JWT |
| GET | `/health` | Status SQL Server + Redis | Público |

---

## Testes

```bash
cd CaseItau_Backend && dotnet test
```

56 testes unitários: 37 services, 16 validators, 3 mappings.

---

## Screenshots

**Login**

<img width="800" height="600" alt="Login" src="https://github.com/user-attachments/assets/7ae397d4-7aa5-4649-86a3-f7a15ed2b8ed" />

**CRUD de Fundos**

<img width="800" height="600" alt="CRUD de Fundos" src="https://github.com/user-attachments/assets/ff17b269-31c0-42a3-967f-c6d167ba9f29" />

**Movimentação**

<img width="800" height="600" alt="Movimentação" src="https://github.com/user-attachments/assets/b00b0139-849e-44a0-8d1d-a09d99819d11" />

**Posição Patrimonial**

<img width="800" height="600" alt="Posição Patrimonial" src="https://github.com/user-attachments/assets/00c7705c-6108-400f-8776-ad55583fc2ec" />
