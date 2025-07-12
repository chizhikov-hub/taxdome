using TaxDome.Domain.Enums;

namespace TaxDome.Domain.Entities;

public class Document
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public string Type { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public string CreatedBy { get; private set; }
    public DocumentStatus Status { get; private set; }

    private Document() { } // For EF Core

    public Document(string name, string type, string createdBy)
    {
        Id = Guid.NewGuid();
        Name = name;
        Type = type;
        CreatedAt = DateTime.UtcNow;
        CreatedBy = createdBy;
        Status = DocumentStatus.Active;
    }
}