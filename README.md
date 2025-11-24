# 🧠 Troca Comigo API – Plataforma de Mapeamento de Competências para o Futuro do Trabalho

Backend em **.NET 8 + Oracle + Observabilidade** para gerenciar usuários, habilidades e trilhas de aprendizado focadas no **Futuro do Trabalho**.

> Projeto desenvolvido para a **Global Solution 2025/2 – Advanced Business Development with .NET (FIAP)**  
> Tema: **O Futuro do Trabalho**

---

## 📌 Visão Geral

A **Troca Comigo API** é uma plataforma que ajuda pessoas e organizações a:

- Mapear **habilidades atuais** e **competências do futuro**
- Relacionar **usuários ↔ habilidades**
- Criar **trilhas de aprendizado** alinhadas ao mercado
- Apoiar **requalificação profissional (reskilling)** e **upskilling**
- Preparar talentos para carreiras impactadas por **IA, automação, green jobs, trabalho híbrido** etc.

A API foi construída com foco em:

- **Boas práticas REST**
- **Paginação**
- **HATEOAS**
- **Status codes adequados**
- **Banco Oracle + EF Core 7**
- **HealthChecks, Logging, Tracing (OpenTelemetry)**
- **Versionamento de API (v1, v2)**  
- **Containerização (Docker)**

---

## 👥 Integrantes

- Guilherme Felipe – RM558282  
- Pablo Lopes – RM556834  
- Vinicius Leopoldino - RM557047

> *(Preencher com os dados reais do grupo.)*

---

## 🏗 Arquitetura da Solução

### 🧩 Camadas do Projeto

Estrutura de pastas:

```text
TrocaComigo.Api
│
├── Application
│   ├── Interface          → Contratos de serviços (ports)
│   └── Service            → Implementação das regras de negócio
│
├── Doc
│   └── Samples            → Exemplos, documentação extra
│
├── Domain
│   └── Entities           → Entidades de domínio (POCOs)
│
├── Infrastructure
│   ├── Data               → DbContext, Mapeamentos, Repositórios
│   ├── HealthCheck        → Configurações de HealthChecks
│   └── IoC                → Injeção de dependência (registrar services/repos)
│
├── Presentation
│   ├── Controllers        → Endpoints HTTP (API REST)
│   ├── Dtos               → Objetos de transporte (entrada/saída)
│   └── Hateoas            → Montagem de links HATEOAS
│
├── appsettings.json       → Connection string, configs
├── Dockerfile             → Container da API
└── Program.cs             → Configuração do pipeline / middlewares

🔐 Principais Tecnologias

.NET 8 (ASP.NET Core Web API)

Oracle Database (ORCL FIAP)

Entity Framework Core 7 + Oracle.EntityFrameworkCore

API Versioning (/api/v1, /api/v2)

Swagger / OpenAPI

Serilog (console + arquivo)

HealthChecks

OpenTelemetry (Tracing)

AutoMapper

JWT (opcional) – se ativado no projeto

Docker (Linux container)

🧬 Modelo de Domínio
Entidades principais

Usuario

Id (int)

Nome (string)

Email (string)

AreaAtuacao (string)

Senioridade (string) – Ex.: Junior, Pleno, Senior

UsuarioHabilidades (N:N com Habilidade)

Habilidade

Id (int)

Nome (string) – Ex.: Machine Learning, Comunicação, Liderança

Categoria (string) – Ex.: Tech, Soft Skill, IA, Green Jobs

Descricao (string?)

UsuarioHabilidades (N:N com Usuario)

TrilhaHabilidades (N:N com TrilhaAprendizado)

UsuarioHabilidade (tabela de junção N:N)

UsuarioId

HabilidadeId

Nivel (1 a 5)

AnosExperiencia (int)

TrilhaAprendizado

Id (int)

Titulo (string)

Descricao (string)

AreaFoco (string) – Ex.: IA & Automação, Liderança Digital

TrilhaHabilidades (N:N com Habilidade)

TrilhaHabilidade (tabela de junção N:N)

TrilhaAprendizadoId

HabilidadeId

⚙️ Configuração do Ambiente
1️⃣ Pré-requisitos

.NET SDK 8.0+

Docker Desktop (para rodar via container, opcional)

Oracle Database FIAP (ORCL)

SQL Developer / DBeaver (para acessar o banco)

Conta Oracle para acessar o feed de pacotes Oracle.EntityFrameworkCore (se necessário)

2️⃣ appsettings.json

Exemplo (NÃO colocar senha real no repositório público):

{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*",
  "ConnectionStrings": {
    "DefaultConnection": "User Id=RMXXXXX;Password=SUA_SENHA;Data Source=oracle.fiap.com.br:1521/ORCL;"
  }
}


Trocar RMXXXXX e SUA_SENHA pelos dados do usuário Oracle FIAP.

3️⃣ Program.cs (Resumo das principais configurações)

DbContext Oracle com EF Core

Serilog

API Versioning

Swagger

HealthChecks

OpenTelemetry

CORS

AutoMapper

(O código já está implementado no projeto, aqui é só a visão geral.)

🗄️ Migrations & Banco de Dados (Oracle)
1️⃣ Instalar tool do EF (se ainda não tiver)

No terminal:

dotnet tool install --global dotnet-ef
# ou, para atualizar:
dotnet tool update --global dotnet-ef

2️⃣ Criar a migration inicial

Na pasta do projeto (onde está o .csproj):

dotnet ef migrations add InitialCreate

3️⃣ Gerar script SQL para Oracle
dotnet ef migrations script -o SkillTrack_Oracle.sql

4️⃣ Executar no SQL Developer

Abrir o arquivo SkillTrack_Oracle.sql

Conectar no schema do RM (ex.: RM556834)

Executar o script
Isso criará as tabelas:

USUARIOS

HABILIDADES

TRILHAS

USUARIOS_HABILIDADES

TRILHAS_HABILIDADES

SEQUENCES respectivas

🏃 Como Rodar o Projeto (Local)
Rodar direto (sem Docker)
dotnet restore
dotnet build
dotnet run


Por padrão, a API sobe em algo como:

https://localhost:7060

http://localhost:5060

A porta exata depende do launchSettings.json.

Swagger UI

Acessar:

https://localhost:7060/swagger

🐳 Rodando via Docker (Opcional)
1️⃣ Build da imagem

Na pasta do projeto:

docker build -t troca-comigo-api .

2️⃣ Rodar o container
docker run -d -p 8080:8080 --name troca-comigo-api troca-comigo-api


API disponível em http://localhost:8080/swagger (ajustar porta conforme Dockerfile)

🌐 Versionamento da API

A API utiliza versionamento via segmento de URL:

GET /api/v1/usuarios

GET /api/v1/habilidades

GET /api/v1/trilhas

Futuras versões podem ser expostas como:

GET /api/v2/usuarios

📚 Endpoints Principais (v1)

(nomes podem ser ajustados conforme controllers reais)

👤 Usuários

Base: /api/v1/usuarios

Método	Rota	Descrição
GET	/api/v1/usuarios	Lista paginada de usuários
GET	/api/v1/usuarios/{id}	Detalhes de um usuário
POST	/api/v1/usuarios	Cria um novo usuário
PUT	/api/v1/usuarios/{id}	Atualiza um usuário
DELETE	/api/v1/usuarios/{id}	Remove um usuário
🧩 Habilidades

Base: /api/v1/habilidades

Método	Rota	Descrição
GET	/api/v1/habilidades	Lista paginada de habilidades
GET	/api/v1/habilidades/{id}	Detalhes da habilidade
POST	/api/v1/habilidades	Cria uma nova habilidade
PUT	/api/v1/habilidades/{id}	Atualiza uma habilidade
DELETE	/api/v1/habilidades/{id}	Remove uma habilidade
📚 Trilhas de Aprendizado

Base: /api/v1/trilhas

Método	Rota	Descrição
GET	/api/v1/trilhas	Lista paginada de trilhas
GET	/api/v1/trilhas/{id}	Detalhes da trilha
POST	/api/v1/trilhas	Cria uma nova trilha
PUT	/api/v1/trilhas/{id}	Atualiza uma trilha
DELETE	/api/v1/trilhas/{id}	Remove uma trilha
🔗 Associações

POST /api/v1/usuarios/{usuarioId}/habilidades
Associa habilidades a um usuário.

POST /api/v1/trilhas/{trilhaId}/habilidades
Associa habilidades a uma trilha.

📄 Exemplo de JSON – Usuário
➕ Criar Usuário (POST /api/v1/usuarios)
{
  "nome": "Ana Silva",
  "email": "ana.silva@example.com",
  "areaAtuacao": "Tecnologia",
  "senioridade": "Pleno"
}

🔁 Resposta (201 CREATED + HATEOAS)
{
  "id": 1,
  "nome": "Ana Silva",
  "email": "ana.silva@example.com",
  "areaAtuacao": "Tecnologia",
  "senioridade": "Pleno",
  "links": [
    {
      "rel": "self",
      "href": "https://localhost:7060/api/v1/usuarios/1",
      "method": "GET"
    },
    {
      "rel": "update",
      "href": "https://localhost:7060/api/v1/usuarios/1",
      "method": "PUT"
    },
    {
      "rel": "delete",
      "href": "https://localhost:7060/api/v1/usuarios/1",
      "method": "DELETE"
    },
    {
      "rel": "add-habilidade",
      "href": "https://localhost:7060/api/v1/usuarios/1/habilidades",
      "method": "POST"
    }
  ]
}

📄 Exemplo de JSON – Habilidade
➕ Criar Habilidade (POST /api/v1/habilidades)
{
  "nome": "Machine Learning",
  "categoria": "Tech",
  "descricao": "Modelagem e treinamento de modelos preditivos."
}

📄 Exemplo de JSON – Trilha de Aprendizado
➕ Criar Trilha (POST /api/v1/trilhas)
{
  "titulo": "Trilha IA & Automação",
  "descricao": "Trilha focada em preparar profissionais para trabalhar com IA e automação.",
  "areaFoco": "Inteligência Artificial"
}

📄 Exemplo de Associação Usuário ↔ Habilidades
➕ Associar Habilidades ao Usuário (POST /api/v1/usuarios/1/habilidades)
{
  "habilidades": [
    {
      "habilidadeId": 1,
      "nivel": 4,
      "anosExperiencia": 2
    },
    {
      "habilidadeId": 2,
      "nivel": 3,
      "anosExperiencia": 1
    }
  ]
}

📑 Paginação

A listagem suporta parâmetros:

page (padrão: 1)

pageSize (padrão: 10)

Exemplo de request
GET /api/v1/usuarios?page=1&pageSize=5

Exemplo de response paginado
{
  "page": 1,
  "pageSize": 5,
  "totalItems": 23,
  "totalPages": 5,
  "items": [
    {
      "id": 1,
      "nome": "Ana Silva",
      "email": "ana.silva@example.com",
      "links": [...]
    }
  ],
  "links": [
    {
      "rel": "self",
      "href": "https://localhost:7060/api/v1/usuarios?page=1&pageSize=5",
      "method": "GET"
    },
    {
      "rel": "next",
      "href": "https://localhost:7060/api/v1/usuarios?page=2&pageSize=5",
      "method": "GET"
    }
  ]
}

⚠️ Tratamento de Erros (Exemplo)
Exemplo – Usuário não encontrado
GET /api/v1/usuarios/999

{
  "statusCode": 404,
  "error": "NotFound",
  "message": "Usuário não encontrado.",
  "traceId": "00-6d7b22b6c6df0165145c69d14c86f5df-7ad9e76e81e3e2d4-00"
}


O traceId pode ser correlacionado com os logs do Serilog e o tracing do OpenTelemetry.

🩺 HealthCheck

Endpoint:

GET /health


Retorna o status básico da aplicação (UP/DOWN).
Pode ser estendido para validar conexão com Oracle.

📊 Observabilidade (Logging + Tracing)

Serilog grava logs:

Console

Arquivo: logs/skilltrack-log.txt (ou nome definido)

OpenTelemetry:

Coleta traces das requisições HTTP

Pode ser integrado a Zipkin/Jaeger no futuro

🔒 Autenticação & Segurança (Opcional)

A solução suporta configuração de JWT Bearer.
Se ativado:

Endpoints sensíveis exigem Authorization: Bearer <token>

Usuários seriam autenticados e autorizados com base em roles/claims.

(Essa parte é opcional na GS, mas suportada na arquitetura.)

🎥 Vídeo de Demonstração

(Preencher quando o vídeo estiver pronto.)

Link do vídeo da API (.NET):
[Em construção]

✅ Como esta API atende os requisitos da GS (.NET)

Boas Práticas REST (30 pts)

Verbos HTTP corretos (GET, POST, PUT, DELETE)

Status codes adequados (200, 201, 400, 404, 500…)

Paginação implementada

HATEOAS nas respostas

Monitoramento e Observabilidade (15 pts)

HealthCheck endpoint (/health)

Serilog configurado

OpenTelemetry configurado

Versionamento da API (10 pts)

Rota /api/v1/...

Suporte a novas versões (/api/v2/...)

Documentado no README

Integração e Persistência (30 pts)

Banco Oracle ORCL FIAP

EF Core 7 + Oracle.EntityFrameworkCore

Migrations + script SQL

Relacionamentos N:N

Testes automatizados (15 pts)

Testes com xUnit (controllers e services)

Podem ser executados via dotnet test

Casos cobrindo cenários de sucesso/erro/paginação

📎 Próximos Passos / Melhorias Futuras

Implementar módulo de recomendações inteligentes (ML.NET ou integração com serviço externo de IA).

Adicionar autenticação via JWT com roles Aluno / Mentor / Empresa.

Dashboard web ou mobile consumindo essa API.

Métricas mais detalhadas com Prometheus/Grafana.
