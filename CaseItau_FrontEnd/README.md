# SABR 3.0 - Front-End

Sistema Front-End do SABR 3.0, baseado em Angular 15 com Nebular UI.

## Stack

- **Angular** 15.2.10
- **Nebular** 11.0.1
- **AG Grid** 31.3.4
- **Node.js** 18.20.0 (obrigatorio)

---

## 1. Pre-requisitos

| Ferramenta | Versao |
|---|---|
| Git | mais recente |
| Node.js | **18.20.0** (via NVM) |
| Angular CLI | 15.2.10 |

> **IMPORTANTE:** Angular 15.2.x exige Node 18 (>= 18.10.0). Node 20+ nao e compativel com todas as dependencias.

---

## 2. Instalar Git

Baixe em https://git-scm.com/ e valide:

```bash
git --version
```

Configure sua identidade:

```bash
git config --global user.name "Seu Nome"
git config --global user.email "seu.email@empresa.com"
```

---

## 3. Instalar NVM + Node 18

### Windows (nvm-windows)

Baixe o instalador em https://github.com/coreybutler/nvm-windows/releases (`nvm-setup.exe`).

> Desinstale versoes previas do Node antes de usar o nvm-windows para evitar conflito de PATH.

```bash
nvm install 18.20.0
nvm use 18.20.0
node -v   # deve mostrar v18.20.0
```

---

## 4. Clonar e instalar

```bash
git clone <url-do-repositorio>
cd SABR-FrontEnd
```

Confirme o Node correto:

```bash
node -v   # v18.20.0
```

Instale as dependencias:

```bash
npm install --legacy-peer-deps
```

---

## 5. Rodar

```bash
npm start
```

Acesse: http://localhost:4200

---

## 6. Build de producao

```bash
ng build --configuration production
```

---

## 7. Troubleshooting

### npm install falha com conflito de dependencias

Use `--legacy-peer-deps`:

```bash
npm install --legacy-peer-deps
```

### node-sass + Python (ENOENT / EPERM)

O `node-sass` e incompativel com Node 18+. Substitua no `package.json`:

```json
// remover: "node-sass": "^4.14.1"
// adicionar: "sass": "^1.69.0"
```

Depois limpe e reinstale:

```bash
rm -rf node_modules package-lock.json
npm cache clean --force
npm install --legacy-peer-deps
```

### @types/ws - Type 'Server' is not generic

```bash
npm install @types/ws@8.5.4 --legacy-peer-deps
```

### nvm nao troca a versao do Node

- Verifique se nao existe Node instalado manualmente (conflito de PATH).
- Execute o terminal como Administrador.

---

## 8. Estrutura do projeto

```
src/
├── app/
│   ├── @core/                          # Nucleo da aplicacao
│   │   ├── guards/                     # Guards de rota (AuthGuard, RoleGuard)
│   │   ├── mock/                       # Servicos de dados mock
│   │   ├── models/                     # Models (ag-grid, form-field)
│   │   ├── pipes/                      # Pipes (translate)
│   │   ├── services/                   # Services (auth, menu, notification, translation)
│   │   ├── strategies/                 # Estrategias de autenticacao
│   │   └── utils/                      # Utilitarios (validators, helpers)
│   │
│   ├── @theme/                         # Tema e layout
│   │   ├── components/                 # Header, Footer
│   │   ├── layouts/                    # Layout principal (one-column)
│   │   └── styles/                     # SCSS global e overrides
│   │
│   ├── shared/                         # Componentes reutilizaveis
│   │   └── components/
│   │       ├── ag-grid-table/          # Tabela CRUD generica com AG Grid
│   │       └── shared-dialogs/         # Dialog generico (form/delete/export)
│   │
│   └── pages/
│       ├── auth/                       # Login, recuperar senha
│       ├── admin/                      # Paginas do perfil admin
│       │   ├── dashboard/              # Dashboard com KPIs
│       │   ├── registrations/          # Cadastros
│       │   │   ├── channels/           # Canais
│       │   │   ├── clients/            # Clientes
│       │   │   ├── integrations/       # Integracoes
│       │   │   ├── products/           # Produtos
│       │   │   └── users/              # Usuarios
│       │   ├── orders/                 # Pedidos
│       │   ├── financial/              # Financeiro
│       │   │   └── recharge-requests/  # Solicitacoes de recarga
│       │   ├── access-control/         # Direito de acesso
│       │   │   ├── groups/             # Grupos
│       │   │   └── features/           # Funcionalidades
│       │   └── profile/                # Perfil do admin
│       ├── client/                     # Paginas do perfil cliente
│       │   ├── dashboard/              # Dashboard do cliente
│       │   ├── my-products/            # Meus produtos
│       │   ├── orders/                 # Pedidos
│       │   ├── financial/              # Financeiro
│       │   ├── integrations/           # Integracoes
│       │   └── placeholder/            # Placeholder para paginas em dev
│       └── miscellaneous/              # 404 e afins
│
├── assets/
│   ├── i18n/                           # Traducoes (pt-BR.json)
│   └── images/                         # Imagens (logos, canais, login)
│
└── environments/                       # Configs por ambiente (dev/prod)
```
