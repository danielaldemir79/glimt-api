using Glimt.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Glimt.Api.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options)
    : DbContext(options)
{
    // Representerar tabellen med minnen i databasen
    public DbSet<MemoryEntry> MemoryEntries { get; set; }
}