using TaxDome.Domain.Enums;

namespace TaxDome.Application.DTOs;

public class DocumentDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public string CreatedBy { get; set; } = string.Empty;
    public DocumentStatus Status { get; set; }
}