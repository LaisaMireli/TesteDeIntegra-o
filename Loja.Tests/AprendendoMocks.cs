using FluentAssertions;
using NSubstitute; // <--- A Biblioteca de Mock
using Xunit;

namespace Loja.Tests;

// --- 1. O CONTRATO (A Dependência) ---
// Essa interface representa o acesso ao banco. Na vida real estaria na API.
public interface IEstoqueRepository
{
    int ObterQuantidadeEmEstoque(int produtoId);
}

// --- 2. A REGRA DE NEGÓCIO (O Ator Principal) ---
// Essa classe precisa do repositório para trabalhar.
public class VendaService
{
    private readonly IEstoqueRepository _repositorio;

    public VendaService(IEstoqueRepository repositorio)
    {
        _repositorio = repositorio;
    }

    public string RealizarVenda(int produtoId, int quantidadeSolicitada)
    {
        // Vai no "banco" ver quanto tem
        var quantidadeAtual = _repositorio.ObterQuantidadeEmEstoque(produtoId);

        if (quantidadeAtual < quantidadeSolicitada)
        {
            return "Estoque insuficiente!";
        }

        return "Venda realizada com sucesso!";
    }
}

// --- 3. O TESTE COM MOCK (Onde a mágica acontece) ---
public class AprendendoMocks
{
    [Fact]
    public void Venda_DeveFalhar_QuandoEstoqueForBaixo()
    {
        // 1. ARRANGE (Preparar o Dublê)
        // Criamos o Mock (o dublê) da interface
        var repositorioFalso = Substitute.For<IEstoqueRepository>();

        // ENSINAMOS O DUBLÊ A MENTIR:
        // "Quando alguém perguntar o estoque do produto 1, responda que tem apenas 5."
        repositorioFalso.ObterQuantidadeEmEstoque(1).Returns(5);

        // Criamos o serviço injetando o nosso dublê
        var servico = new VendaService(repositorioFalso);

        // 2. ACT (Ação)
        // Tentamos vender 10 itens (mas o dublê disse que só tem 5)
        var resultado = servico.RealizarVenda(produtoId: 1, quantidadeSolicitada: 10);

        // 3. ASSERT (Validação)
        // A lógica deve ter percebido que 5 < 10 e retornado erro.
        resultado.Should().Be("Estoque insuficiente!");
    }

    [Fact]
    public void Venda_DeveSucesso_QuandoTiverEstoque()
    {
        // 1. ARRANGE
        var repositorioFalso = Substitute.For<IEstoqueRepository>();

        // Agora o dublê diz que tem 100 itens!
        repositorioFalso.ObterQuantidadeEmEstoque(99).Returns(100);

        var servico = new VendaService(repositorioFalso);

        // 2. ACT
        // Tento vender 10 (tem 100, então deve dar certo)
        var resultado = servico.RealizarVenda(produtoId: 99, quantidadeSolicitada: 10);

        // 3. ASSERT
        resultado.Should().Be("Venda realizada com sucesso!");
    }
}