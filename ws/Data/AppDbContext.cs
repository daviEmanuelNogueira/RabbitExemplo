using Microsoft.EntityFrameworkCore;
using ws.Models;

namespace ws.Data;

public class AppDbContext : DbContext
{
    public DbSet<Notification>? Notifications { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Server=db,1433;Database=TESTE_DB;User Id=sa;Password=24241224@Senha;Trusted_Connection=False; TrustServerCertificate=True");
    }
}
