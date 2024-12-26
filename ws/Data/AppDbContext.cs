using Microsoft.EntityFrameworkCore;
using ws.Models;

namespace ws.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    public DbSet<Notification>? Notifications { get; set; }

}
