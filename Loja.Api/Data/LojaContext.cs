using Loja.Api.Models;
using Microsoft.EntityFrameworkCore; // <--- Importante

namespace Loja.Api.Data;

// 👇 O ERRO ESTÁ AQUI: Faltava o " : DbContext"
public class LojaContext : DbContext 
{
    public LojaContext(DbContextOptions<LojaContext> options) : base(options) { }

    public DbSet<Produto> Produtos { get; set; }
}