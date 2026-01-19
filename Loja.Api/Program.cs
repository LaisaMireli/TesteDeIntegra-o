using Loja.Api.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Configuração do SQLite para rodar no Linux/Windows
builder.Services.AddDbContext<LojaContext>(opts => 
    opts.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllers();

var app = builder.Build();

app.MapControllers();

app.Run();

// 🚨 ESSENCIAL: Permite que o projeto de testes enxergue essa classe
public partial class Program { }