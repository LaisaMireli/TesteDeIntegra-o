using Microsoft.AspNetCore.Hosting; // <--- Essencial para o IWebHostBuilder
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Data.Common;
using Loja.Api.Data;
using Loja.Api; // <--- Essencial para achar o Program

namespace Loja.Tests.Setup;

public class LojaApiFactory : WebApplicationFactory<Program>
{
    // O segredo está aqui: override void ConfigureWebHost(IWebHostBuilder builder)
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            // 1. Remove o contexto do banco original (SQL Server ou SQLite de prod)
            var dbContextDescriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(DbContextOptions<LojaContext>));
            
            if (dbContextDescriptor != null) 
                services.Remove(dbContextDescriptor);

            // 2. Remove a conexão de banco original
            var dbConnectionDescriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(DbConnection));

            if (dbConnectionDescriptor != null) 
                services.Remove(dbConnectionDescriptor);

            // 3. Cria a nova conexão SQLite em Memória (Singleton para durar o teste todo)
            services.AddSingleton<DbConnection>(container =>
            {
                var connection = new SqliteConnection("DataSource=:memory:");
                connection.Open();
                return connection;
            });

            // 4. Adiciona o contexto usando a conexão em memória
            services.AddDbContext<LojaContext>((container, options) =>
            {
                var connection = container.GetRequiredService<DbConnection>();
                options.UseSqlite(connection);
            });
        });
    }
}