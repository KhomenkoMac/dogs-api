using codebridge.api.Data.entities;
using Microsoft.EntityFrameworkCore;

namespace codebridge.api.Data;

public class AppDbContext: DbContext
{
    public AppDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<Dog> Dogs { get; set; } = null!;
}
