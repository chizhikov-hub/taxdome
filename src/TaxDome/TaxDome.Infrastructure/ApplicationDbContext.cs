using Microsoft.EntityFrameworkCore;
using TaxDome.Domain.Entities;

namespace TaxDome.Infrastructure;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<Document> Documents => Set<Document>();
    public DbSet<Client> Clients => Set<Client>();
    public DbSet<Folder> Folders => Set<Folder>();
    public DbSet<DocumentAction> DocumentActions => Set<DocumentAction>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Client>(builder =>
        {
            builder.HasKey(d => d.Id);
            
            builder.Property(d => d.Name)
                .IsRequired()
                .HasMaxLength(255);
            
            builder.HasMany<Document>()
                .WithOne(d => d.Client)
                .HasForeignKey(d => d.ClientId)
                .OnDelete(DeleteBehavior.Restrict);
        });
        
        modelBuilder.Entity<Folder>(builder =>
        {
            builder.HasKey(d => d.Id);
            
            builder.Property(d => d.Name)
                .IsRequired()
                .HasMaxLength(255);
            
            builder.HasMany<Document>()
                .WithOne(d => d.Folder)
                .HasForeignKey(d => d.FolderId)
                .OnDelete(DeleteBehavior.Restrict);
        });
        
        modelBuilder.Entity<DocumentAction>(builder =>
        {
            builder.HasKey(d => d.Id);
            
            builder.Property(d => d.Name)
                .IsRequired()
                .HasMaxLength(255);
            
            builder.HasMany(a => a.AppliedToDocuments)
                .WithMany(d => d.AppliedActions)
                .UsingEntity("DocumentAppliedActions");

            builder.HasMany(a => a.AvailableForDocuments)
                .WithMany(d => d.AvailableActions)
                .UsingEntity("DocumentAvailableActions");
        });
        
        modelBuilder.Entity<Document>(builder =>
        {
            builder.HasKey(d => d.Id);
            
            builder.Property(d => d.FileName)
                .IsRequired()
                .HasMaxLength(255);
            
            builder.Property(d => d.FileSize)
                .IsRequired();
            
            builder.Property(d => d.Date)
                .IsRequired();
            
            builder.HasOne(d => d.Client)
                .WithMany()
                .HasForeignKey(d => d.ClientId)
                .OnDelete(DeleteBehavior.Restrict);
            
            builder.HasOne(d => d.Folder)
                .WithMany()
                .HasForeignKey(d => d.FolderId)
                .OnDelete(DeleteBehavior.Restrict);
            
            builder.HasMany(d => d.AppliedActions)
                .WithMany(a => a.AppliedToDocuments)
                .UsingEntity("DocumentAppliedActions");

            builder.HasMany(d => d.AvailableActions)
                .WithMany(a => a.AvailableForDocuments)
                .UsingEntity("DocumentAvailableActions");
        });
    }
}