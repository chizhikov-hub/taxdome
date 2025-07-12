using Microsoft.EntityFrameworkCore;
using TaxDome.Domain.Entities;

namespace TaxDome.Infrastructure;

public class ApplicationDbContext : DbContext
{
    public DbSet<Document> Documents { get; set; } = null!;

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Здесь вы можете настроить сущность Document, если нужно, например:
        modelBuilder.Entity<Document>(entity =>
        {
            entity.HasKey(d => d.Id);
            entity.Property(d => d.Name).IsRequired().HasMaxLength(255);
            entity.Property(d => d.Type).IsRequired().HasMaxLength(100);
            entity.Property(d => d.CreatedBy).IsRequired().HasMaxLength(100);
        });
    }
}