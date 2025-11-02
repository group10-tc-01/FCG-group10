# **FCG - Free Clean Games Platform - API Documentation**

## **Contexto do Projeto**

FCG (Free Clean Games) é uma plataforma de gerenciamento de jogos digitais desenvolvida em .NET 8, seguindo os princípios de Clean Architecture, SOLID e Domain-Driven Design (DDD). A API REST fornece funcionalidades para gerenciamento de usuários, jogos, promoções e bibliotecas pessoais de jogos.

## **Arquitetura**

### **Stack Tecnológico**
- **.NET 8** (C#)
- **Entity Framework Core** (ORM)
- **MediatR** (CQRS Pattern)
- **JWT Authentication** (Bearer Token)
- **Docker** (Containerização)
- **Swagger/OpenAPI** (Documentação)

### **Estrutura do Projeto**
```
FCG/
├── FCG.Domain/          # Entidades, Value Objects, Eventos de Domínio
├── FCG.Application/     # Casos de Uso (Use Cases), Handlers
├── FCG.Infrastructure/  # Persistência, Migrations, Serviços Externos
├── FCG.WebApi/          # Controllers, Middlewares, Configurações
├── FCG.Messages/        # Mensagens de Recursos (i18n)
└── tests/               # Testes Unitários, Integrados e Funcionais
```

## **Modelo de Domínio**

### **Entidades Principais**

#### **1. User (Usuário)**
- Id (Guid)
- Name (Value Object)
- Email (Value Object)
- Password (Hash - Value Object)
- Role (Enum: Admin = 0, User = 1)
- Library (Relacionamento 1:1)
- Wallet (Relacionamento 1:1)
- RefreshTokens (Coleção)

#### **2. Game (Jogo)**
- Id (Guid)
- Name (Value Object)
- Description (string)
- Price (Value Object - decimal)
- Category (string)
- Promotions (Coleção)
- LibraryGames (Relacionamento N:N)

#### **3. Promotion (Promoção)**
- Id (Guid)
- GameId (Guid)
- Discount (Value Object)
- StartDate (DateTime)
- EndDate (DateTime)
- Game (Relacionamento)

#### **4. Library (Biblioteca)**
- Id (Guid)
- UserId (Guid)
- LibraryGames (Coleção de jogos do usuário)

#### **5. Wallet (Carteira)**
- Id (Guid)
- UserId (Guid)
- Balance (decimal) - Saldo inicial de R$ 10,00

---

## **ROTAS DA API - ESPECIFICAÇÃO COMPLETA**

### **Base URL**
```
https://api.fcg.com/api/v1
```

### **Health Check**
```http
GET /health
```
**Status:** 200 OK (Healthy) | 503 Service Unavailable (Unhealthy)

---

## **1. AUTENTICAÇÃO (AuthController)**

### **1.1. Login**
```http
POST /api/v1/auth/login
Content-Type: application/json
```

**Request Body:**
```json
{
  "email": "user@example.com",
  "password": "Password123!"
}
```

**Response (200 OK):**
```json
{
  "success": true,
  "data": {
    "accessToken": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
    "refreshToken": "refresh_token_string",
    "expiresInMinutes": 60
  }
}
```

**Possíveis Erros:**
- `400 Bad Request` - Email/senha inválidos

---

### **1.2. Refresh Token**
```http
POST /api/v1/auth/refresh-token
Content-Type: application/json
```

**Request Body:**
```json
{
  "refreshToken": "refresh_token_string"
}
```

**Response (200 OK):**
```json
{
  "success": true,
  "data": {
    "accessToken": "new_access_token",
    "refreshToken": "new_refresh_token",
    "expiresInMinutes": 60
  }
}
```

**Possíveis Erros:**
- `400 Bad Request` - Token inválido
- `401 Unauthorized` - Token expirado

---

### **1.3. Logout**
```http
POST /api/v1/auth/logout
Authorization: Bearer {accessToken}
```

**Response (200 OK):**
```json
{
  "success": true,
  "data": {
    "message": "Logout realizado com sucesso"
  }
}
```

**Possíveis Erros:**
- `401 Unauthorized` - Token inválido

---

## **2. USUÁRIOS (UsersController)**

### **2.1. Registrar Usuário**
```http
POST /api/v1/users/register
Content-Type: application/json
```

**Request Body:**
```json
{
  "name": "John Doe",
  "email": "john.doe@example.com",
  "password": "SecurePassword123!"
}
```

**Response (201 Created):**
```json
{
  "success": true,
  "data": {
    "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    "name": "John Doe",
    "email": "john.doe@example.com"
  }
}
```

**Regras de Negócio:**
- Ao registrar, automaticamente:
  - Uma **Library** (biblioteca) é criada para o usuário
  - Uma **Wallet** (carteira) é criada com saldo inicial de R$ 10,00
  - O usuário recebe o role **User** por padrão

**Possíveis Erros:**
- `400 Bad Request` - Dados inválidos
- `409 Conflict` - Email já cadastrado

---

### **2.2. Atualizar Usuário**
```http
PUT /api/v1/users/{id}
Authorization: Bearer {accessToken}
Content-Type: application/json
```

**Request Body:**
```json
{
  "password": "NewSecurePassword123!"
}
```

**Response (200 OK):**
```json
{
  "success": true,
  "data": {
    "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    "message": "Usuário atualizado com sucesso"
  }
}
```

**Autenticação:** Requer usuário autenticado (`[AuthenticatedUser]`)

**Possíveis Erros:**
- `400 Bad Request` - Dados inválidos
- `401 Unauthorized` - Não autenticado
- `404 Not Found` - Usuário não encontrado

---

### **2.3. Atualizar Role do Usuário (Admin)**
```http
PATCH /api/v1/users/admin/update-role
Authorization: Bearer {adminToken}
Content-Type: application/json
```

**Request Body:**
```json
{
  "userId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "role": 0
}
```

**Roles:**
- `0` = Admin
- `1` = User

**Response (200 OK):**
```json
{
  "success": true,
  "data": {
    "userId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    "newRole": "Admin",
    "message": "Role atualizada com sucesso"
  }
}
```

**Autenticação:** Requer administrador (`[AuthenticatedAdmin]`)

**Possíveis Erros:**
- `400 Bad Request` - Dados inválidos
- `403 Forbidden` - Sem permissão de admin
- `404 Not Found` - Usuário não encontrado

---

## **3. ADMINISTRAÇÃO (AdminController)**

### **3.1. Listar Todos os Usuários (Paginado)**
```http
GET /api/v1/admin/users?PageNumber=1&PageSize=10
Authorization: Bearer {adminToken}
```

**Query Parameters:**
- `PageNumber` (int) - Número da página (padrão: 1)
- `PageSize` (int) - Tamanho da página (padrão: 10)

**Response (200 OK):**
```json
{
  "success": true,
  "data": {
    "items": [
      {
        "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        "name": "John Doe",
        "email": "john.doe@example.com",
        "role": "User",
        "createdAt": "2025-10-29T10:00:00Z"
      }
    ],
    "pageNumber": 1,
    "pageSize": 10,
    "totalPages": 5,
    "totalCount": 50,
    "hasPreviousPage": false,
    "hasNextPage": true
  }
}
```

**Autenticação:** Requer administrador (`[AuthenticatedAdmin]`)

**Possíveis Erros:**
- `403 Forbidden` - Sem permissão de admin
- `404 Not Found` - Nenhum usuário encontrado

---

### **3.2. Buscar Usuário por ID**
```http
GET /api/v1/admin/users/{id}
Authorization: Bearer {adminToken}
```

**Response (200 OK):**
```json
{
  "success": true,
  "data": {
    "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    "name": "John Doe",
    "email": "john.doe@example.com",
    "role": "User",
    "createdAt": "2025-10-29T10:00:00Z",
    "updatedAt": "2025-10-29T12:00:00Z"
  }
}
```

**Autenticação:** Requer administrador (`[AuthenticatedAdmin]`)

**Possíveis Erros:**
- `403 Forbidden` - Sem permissão de admin
- `404 Not Found` - Usuário não encontrado

---

## **4. JOGOS (GamesController)**

### **4.1. Registrar Jogo**
```http
POST /api/v1/games
Authorization: Bearer {adminToken}
Content-Type: application/json
```

**Request Body:**
```json
{
  "name": "The Legend of Zelda",
  "description": "An epic adventure game",
  "price": 59.99,
  "category": "Adventure"
}
```

**Response (201 Created):**
```json
{
  "success": true,
  "data": {
    "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    "name": "The Legend of Zelda",
    "description": "An epic adventure game",
    "price": 59.99,
    "category": "Adventure"
  }
}
```

**Autenticação:** Requer administrador (`[AuthenticatedAdmin]`)

**Possíveis Erros:**
- `400 Bad Request` - Dados inválidos
- `403 Forbidden` - Sem permissão de admin

---

### **4.2. Listar Todos os Jogos (Paginado)**
```http
GET /api/v1/games?PageNumber=1&PageSize=10&Category=Adventure
Authorization: Bearer {accessToken}
```

**Query Parameters:**
- `PageNumber` (int) - Número da página (padrão: 1)
- `PageSize` (int) - Tamanho da página (padrão: 10)
- `Category` (string, opcional) - Filtrar por categoria

**Response (200 OK):**
```json
{
  "success": true,
  "data": {
    "items": [
      {
        "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        "name": "The Legend of Zelda",
        "description": "An epic adventure game",
        "price": 59.99,
        "category": "Adventure"
      }
    ],
    "pageNumber": 1,
    "pageSize": 10,
    "totalPages": 3,
    "totalCount": 25,
    "hasPreviousPage": false,
    "hasNextPage": true
  }
}
```

**Autenticação:** Requer usuário autenticado (`[AuthenticatedUser]`)

**Possíveis Erros:**
- `401 Unauthorized` - Não autenticado
- `400 Bad Request` - Parâmetros inválidos

---

## **5. PROMOÇÕES (PromotionsController)**

### **5.1. Criar Promoção**
```http
POST /api/v1/promotions
Authorization: Bearer {adminToken}
Content-Type: application/json
```

**Request Body:**
```json
{
  "gameId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "discount": 25.00,
  "startDate": "2025-11-01T00:00:00Z",
  "endDate": "2025-11-30T23:59:59Z"
}
```

**Validações:**
- `discount` deve ser entre 0 e 100 (percentual)
- `endDate` deve ser posterior a `startDate`
- `gameId` deve existir no banco de dados

**Response (201 Created):**
```json
{
  "success": true,
  "data": {
    "id": "7fa85f64-5717-4562-b3fc-2c963f66afa6",
    "gameId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    "discount": 25.00,
    "startDate": "2025-11-01T00:00:00Z",
    "endDate": "2025-11-30T23:59:59Z"
  }
}
```

**Autenticação:** Requer administrador (`[AuthenticatedAdmin]`)

**Possíveis Erros:**
- `400 Bad Request` - Dados inválidos ou datas incorretas
- `403 Forbidden` - Sem permissão de admin
- `404 Not Found` - Jogo não encontrado

---

## **REQUISITOS FUNCIONAIS DO PROJETO**

### **RF01 - Gerenciamento de Usuários**
- ✅ O sistema deve permitir o cadastro de novos usuários
- ✅ Ao cadastrar, o usuário recebe automaticamente uma Library e Wallet
- ✅ A Wallet inicial deve ter saldo de R$ 10,00
- ✅ Usuários podem atualizar sua senha
- ✅ Administradores podem promover/rebaixar usuários

### **RF02 - Autenticação e Autorização**
- ✅ O sistema deve autenticar usuários via email/senha
- ✅ Deve fornecer JWT (Access Token) com expiração
- ✅ Deve fornecer Refresh Token para renovação
- ✅ Deve permitir logout (invalidação de tokens)
- ✅ Dois níveis de acesso: Admin (0) e User (1)

### **RF03 - Gerenciamento de Jogos**
- ✅ Administradores podem cadastrar novos jogos
- ✅ Jogos devem ter: nome, descrição, preço e categoria
- ✅ Usuários autenticados podem listar jogos (paginado)
- ✅ Filtros disponíveis: categoria

### **RF04 - Sistema de Promoções**
- ✅ Administradores podem criar promoções para jogos
- ✅ Promoções têm período de validade (data início/fim)
- ✅ Desconto em percentual (0-100%)
- ✅ Validação de datas (fim após início)

### **RF05 - Biblioteca Pessoal**
- ✅ Todo usuário possui uma Library
- ✅ A Library armazena os jogos adquiridos (LibraryGame)
- ✅ Relacionamento N:N entre Library e Games

### **RF06 - Sistema de Carteira**
- ✅ Todo usuário possui uma Wallet
- ✅ Saldo inicial de R$ 10,00 no cadastro
- ✅ Controle de saldo para compras futuras

### **RF07 - Administração**
- ✅ Administradores podem listar todos os usuários (paginado)
- ✅ Administradores podem buscar usuário específico por ID
- ✅ Administradores podem alterar roles de usuários

---

## **PADRÕES DE RESPOSTA**

### **Sucesso**
```json
{
  "success": true,
  "data": { /* payload específico */ }
}
```

### **Erro**
```json
{
  "success": false,
  "error": {
    "message": "Descrição do erro",
    "code": "ERROR_CODE"
  }
}
```

### **Paginação**
```json
{
  "items": [],
  "pageNumber": 1,
  "pageSize": 10,
  "totalPages": 5,
  "totalCount": 50,
  "hasPreviousPage": false,
  "hasNextPage": true
}
```

---

## **AUTENTICAÇÃO E AUTORIZAÇÃO**

### **Atributos de Segurança**
- `[AuthenticatedUser]` - Requer token válido (Admin ou User)
- `[AuthenticatedAdmin]` - Requer token válido com role Admin

### **Header de Autenticação**
```http
Authorization: Bearer {accessToken}
```

---

## **REGRAS DE NEGÓCIO IMPORTANTES**

### **1. Registro de Usuário**
- Criação automática de Library e Wallet
- Role padrão: User (1)
- Saldo inicial da Wallet: R$ 10,00

### **2. Promoções**
- EndDate deve ser posterior a StartDate
- Desconto entre 0-100%
- Vinculada a um jogo existente

### **3. Roles**
- Admin (0): Acesso total
- User (1): Acesso limitado

### **4. Tokens**
- Access Token: Expira em 60 minutos
- Refresh Token: Para renovação do Access Token

---

## **PRÓXIMOS PASSOS SUGERIDOS PARA O FRONT-END**

### **Fluxos Principais a Implementar**

#### **1. Fluxo de Autenticação**
- Tela de Login
- Tela de Registro
- Renovação automática de token
- Logout

#### **2. Fluxo de Usuário (User)**
- Dashboard principal
- Listagem de jogos (com filtros e paginação)
- Visualização de biblioteca pessoal
- Visualização de saldo da carteira
- Atualização de senha

#### **3. Fluxo de Administrador (Admin)**
- Dashboard administrativo
- Gerenciamento de usuários (listar, buscar, alterar role)
- Cadastro de jogos
- Criação de promoções
- Visualização de estatísticas

#### **4. Componentes Comuns**
- Componente de paginação
- Componente de filtros
- Componente de card de jogo
- Componente de notificações/toasts
- Loading states
- Error handling

---

## **INFORMAÇÕES TÉCNICAS ADICIONAIS**

- **Ambiente de Desenvolvimento:** Swagger disponível em `/swagger`
- **Health Check:** `/health` para monitoramento
- **Content-Type:** `application/json`
- **Charset:** UTF-8
- **CORS:** Configurado para permitir origens específicas
- **Framework:** .NET 8
- **Padrão de Arquitetura:** Clean Architecture + DDD
- **Padrão de Design:** CQRS (MediatR)
- **ORM:** Entity Framework Core

---

## **TABELA DE RESUMO DAS ROTAS**

| Método | Endpoint | Autenticação | Descrição |
|--------|----------|--------------|-----------|
| POST | `/api/v1/auth/login` | Não | Login de usuário |
| POST | `/api/v1/auth/refresh-token` | Não | Renovar access token |
| POST | `/api/v1/auth/logout` | User/Admin | Logout (invalida tokens) |
| POST | `/api/v1/users/register` | Não | Registrar novo usuário |
| PUT | `/api/v1/users/{id}` | User/Admin | Atualizar senha |
| PATCH | `/api/v1/users/admin/update-role` | Admin | Alterar role de usuário |
| GET | `/api/v1/admin/users` | Admin | Listar todos os usuários |
| GET | `/api/v1/admin/users/{id}` | Admin | Buscar usuário por ID |
| POST | `/api/v1/games` | Admin | Cadastrar novo jogo |
| GET | `/api/v1/games` | User/Admin | Listar jogos (paginado) |
| POST | `/api/v1/promotions` | Admin | Criar promoção |
| GET | `/health` | Não | Health check |

---

**Última Atualização:** 29 de outubro de 2025

**Observação:** Este é um sistema em desenvolvimento. Algumas funcionalidades podem estar parcialmente implementadas ou em processo de evolução.
