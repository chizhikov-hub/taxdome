using TaxDome.Domain.Enums;

namespace TaxDome.Application.DTOs;

public record DocumentDto
{
    public DateTime Date { get; init; }
    public string Document { get; init; }
    public long FileSize { get; init; }
    public string Client { get; init; }
    public string Folder { get; init; }
    public List<string> AppliedActions { get; init; }
    public List<string> AvailableActions { get; init; }
}