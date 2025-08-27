#  ExchangeRate API

**API REST moderna para consumo de cotações de moedas**, utilizando a [AwesomeAPI](https://docs.awesomeapi.com.br/api-de-moedas) como fonte de dados, com persistência em banco de dados e atualização automática via **worker em .NET 8**.

Projetada com **arquitetura limpa (Clean Architecture)**, segurança com **JWT** e banco de dados **PostgreSQL**, esta solução é ideal para aplicações que precisam de dados de câmbio atualizados em tempo real.

---

##  Funcionalidades

-  Consulta e armazenamento automático da cotação do **Dólar (USD)** a cada 1 minuto.
-  Persistência dos dados no banco de dados.
-  Autenticação e autorização via **JWT**.
-  Documentação da API via **Swagger (OpenAPI)**.
-  Arquitetura baseada em **Clean Architecture** para alta manutenibilidade e testabilidade.
-  Tecnologia moderna com **.NET 8**.

---

## 🛠️ Tecnologias Utilizadas

| Tecnologia        | Descrição |
|------------------|---------|
| **.NET 8**       | Plataforma principal para desenvolvimento da API e do worker. |
| **PostgreSQL**   | Banco de dados relacional recomendado (configurável para outros). |
| **JWT**          | Autenticação segura com tokens. |
| **Swagger**      | Documentação interativa da API. |

---

## 📦 Pré-requisitos

Antes de executar o projeto, certifique-se de ter instalado:

- ✅ [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- ✅ Um IDE compatível com .NET (recomendado: **Visual Studio 2022** ou **Visual Studio Code**)
- ✅ **PostgreSQL** (ou outro banco compatível com Dapper)
- ✅ [PostgreSQL Client](https://www.pgadmin.org/) (opcional, para visualização do banco)

---

## 🚀 Como Rodar o Projeto

### 1. Clonar o repositório

git clone https://github.com/seu-usuario/ExchangeRate.API.git

### 2. Configurar as variáveis de ambiente

Crie um arquivo `.env` dentro dos seguintes diretórios:

- `ExchangeRate.API/.env`
- `ExchangeRate.Worker.CurrencyUSD/.env`

Com o seguinte conteúdo:

```env
URL_AWESOME_API=https://economia.awesomeapi.com.br/json
CONNECTION_STRING=Host=localhost;Database=exchangerate_db;Username=postgres;Password=sua_senha
JWT_AUDIENCE=ExchangeRateAPI
JWT_ISSUER=ExchangeRateAPI
JWT_SECRET_KEY=sua_chave_secreta_muito_forte_aqui_com_no_minimo_32_caracteres
```

# Estrutura do Projeto

O projeto segue uma arquitetura **limpa e modular**, separando responsabilidades em camadas distintas:

## 📦 API
Responsável por expor endpoints e lidar com solicitações HTTP.

- **Controller**: Controladores de API, responsáveis por receber requisições e retornar respostas.

## 📦 Application
Camada de aplicação, responsável por lógica de integração, DTOs e serviços.

- **API**: Serviços para consumir APIs externas.  
- **DTO**: Objetos de Transferência de Dados usados para comunicar entre camadas.  
- **Interface**: Contratos para os serviços da aplicação.  
- **Service**: Contém lógica de negócios de alto nível e orquestra chamadas aos repositórios.  
- **Settings**: Configurações relacionadas ao projeto.

## 📦 Domain
Camada de domínio, responsável pelas regras de negócio essenciais.

- **Entity**: Entidades de domínio.  
- **Interface**: Contratos para os repositórios de domínio.

## 📦 Infra.Data
Camada de persistência, responsável pelo acesso ao banco de dados.

- **Context**: Orquestra o acesso às tabelas e gerencia a conexão com o banco de dados.  
- **Repository**: Contém a lógica de consulta às tabelas do banco.

## 📦 Infra.Ioc
Responsável por gerenciar a injeção de dependências do projeto.

## 📦 Test Service
Gerencia os testes unitários da camada de Service.

## 📦 Worker.CurrencyUSD
Serviço específico para consumir e persistir dados da API **AwesomeAPI**.

---


