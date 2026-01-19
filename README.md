# Loja Completa - API com Testes de Integração

API desenvolvida em .NET 9 para gerenciamento de produtos, focada em qualidade de código e arquitetura de testes automatizados.

## Tecnologias Utilizadas

* **Linguagem:** C# (.NET 9)
* **Framework:** ASP.NET Core Web API
* **Banco de Dados:** SQLite (Produção e Testes)
* **ORM:** Entity Framework Core
* **Testes:** xUnit, FluentAssertions, Microsoft.Mvc.Testing (Integration)
* **CI/CD:** GitHub Actions

## Arquitetura e Estrutura

O projeto foi dividido para garantir a separação de responsabilidades e testabilidade:

* `Loja.Api`: Contém a regra de negócios, Controllers e acesso a dados.
* `Loja.Tests`: Projeto dedicado à qualidade, contendo:
    * **Unit Tests:** Validação de regras de domínio (ex: Estoque negativo).
    * **Integration Tests:** Testes E2E (End-to-End) usando `WebApplicationFactory` e banco em memória.

## Como Rodar o Projeto Localmente

### Pré-requisitos
* .NET SDK 9.0 instalado.

### Passo a Passo
1. Clone o repositório:
   ```bash
   git clone https://github.com/LaisaMireli/TesteDeIntegra-o.git
   Entre na pasta:

2. Entre na pasta

cd LojaCompleta

3. Execute a API:

dotnet run --project Loja.Api

(A API estará rodando em: http://localhost:5125)
