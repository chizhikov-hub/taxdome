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
            entity.Property(d => d.FileName).IsRequired().HasMaxLength(255);
            entity.Property(d => d.FileSize).IsRequired();
            entity.Property(d => d.Date).IsRequired();
            entity.Property(d => d.Client).IsRequired().HasMaxLength(100);
            entity.Property(d => d.Folder).IsRequired().HasMaxLength(100);
        });
    }
}