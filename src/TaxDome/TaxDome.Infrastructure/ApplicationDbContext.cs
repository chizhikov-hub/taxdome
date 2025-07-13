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
        
        modelBuilder.Entity<DocumentAction>().HasData(
            new DocumentAction(new Guid("7AC3BF0E-5B3D-47DE-B0A0-68AC68DFBBA2"), "Pending Signature"),
            new DocumentAction(new Guid("9FB03BCF-D42E-497D-A756-2AB2CF13C6B7"), "Approved"),
            new DocumentAction(new Guid("7D3B3075-E252-473C-9FD1-A1E7213DC75D"), "Retry"),
            new DocumentAction(new Guid("3BE186F1-17A4-42D9-AA8B-CFEBA27EECD6"), "Pending Approval"),
            new DocumentAction(new Guid("7D76BE70-9B0A-4942-9A89-3A05C99CD907"), "Job Processing"),
            new DocumentAction(new Guid("5C0E250A-FD43-4FD4-A5EC-00575B08EDF6"), "Invoice Linked"),
            new DocumentAction(new Guid("FF7B2E8B-93DD-471D-8DAF-0EE8C35F4371"), "Job Linked")
        );
        
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