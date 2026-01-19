using System.Net.Http.Json;
using FluentAssertions;
using Loja.Api.Data;
using Loja.Api.Models;
using Loja.Tests.Setup;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Loja.Tests.Integration;

public class ProdutosIntegrationTests : IClassFixture<LojaApiFactory>
{
    private readonly HttpClient _client;
    private readonly LojaApiFactory _factory;

    public ProdutosIntegrationTests(LojaApiFactory factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Get_DeveRetornarListaDeProdutos()
    {
        // 1. PREPARAR O BANCO (SEED)
        using (var scope = _factory.Services.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<LojaContext>();
            context.Database.EnsureCreated(); // Cria tabela na memória
            
            // Limpa dados anteriores se houver (boa prática)
            context.Produtos.RemoveRange(context.Produtos);
            
            context.Produtos.Add(new Produto { Nome = "Notebook Gamer", Preco = 5000 });
            await context.SaveChangesAsync();
        }

        // 2. EXECUTAR A REQUISIÇÃO (ACT)
        var response = await _client.GetAsync("/api/produtos");

        // 3. VALIDAR (ASSERT)
        response.EnsureSuccessStatusCode(); 
        
        var produtos = await response.Content.ReadFromJsonAsync<List<Produto>>();
        
        produtos.Should().HaveCount(1);
        produtos.First().Nome.Should().Be("Notebook Gamer");
    }
    
    [Fact]
        public async Task Post_DeveCriarNovoProduto_E_Retornar201Created()
        {
            // 1. ARRANGE (Preparar o cenário)
            // Criamos um objeto C# que o teste vai transformar em JSON para enviar
            var novoProduto = new Produto 
            { 
                Nome = "Mouse Ultra Rápido", 
                Preco = 150.00m, 
                Estoque = 10 
            };
    
            // 2. ACT (Ação - O Momento da Verdade)
            // O método PostAsJsonAsync faz a mágica: serializa o objeto para JSON e envia
            var response = await _client.PostAsJsonAsync("/api/produtos", novoProduto);
    
            // 3. ASSERT (Validação)
            // Verifica se a API retornou código 201 (Created)
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);
    
            // Lê o produto que a API devolveu para ver se os dados batem
            var produtoCriado = await response.Content.ReadFromJsonAsync<Produto>();
            
            produtoCriado.Should().NotBeNull();
            produtoCriado!.Id.Should().BeGreaterThan(0); // O banco deve ter gerado um ID
            produtoCriado.Nome.Should().Be("Mouse Ultra Rápido");
        }
    
}