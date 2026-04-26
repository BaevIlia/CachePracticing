using CachePracticing.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CachePracticing.Infrastructure;

public class ProductsDbContext : DbContext
{
    private readonly IConfiguration _configuration;

    public ProductsDbContext(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public DbSet<Product> Products { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(_configuration.GetConnectionString("ProductsDb"));
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new ProductEntityConfiguration());
    }
}