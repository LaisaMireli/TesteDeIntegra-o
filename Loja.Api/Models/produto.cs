namespace Loja.Api.Models;

public class Produto
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public decimal Preco { get; set; }
    public int Estoque { get; set; }

    // Função utilitária (Regra de Negócio) para testarmos com Unit Test depois
    public void AdicionarEstoque(int quantidade)
    {
        if (quantidade <= 0) throw new ArgumentException("Quantidade deve ser positiva");
        Estoque += quantidade;
    }
}