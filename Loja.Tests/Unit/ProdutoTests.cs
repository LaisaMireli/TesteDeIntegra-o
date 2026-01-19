using FluentAssertions;
using Loja.Api.Models;
using Xunit;

namespace Loja.Tests.Unit;

public class ProdutoTests
{
    [Fact]
    public void AdicionarEstoque_ComValorNegativo_DeveLancarErro()
    {
        // Arrange
        var produto = new Produto { Estoque = 10 };

        // Act
        Action act = () => produto.AdicionarEstoque(-5);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("Quantidade deve ser positiva");
    }
}