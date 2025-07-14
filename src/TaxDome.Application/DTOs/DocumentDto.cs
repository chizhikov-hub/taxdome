using TaxDome.Domain.Enums;

namespace TaxDome.Application.DTOs;

public record DocumentDto
{
    public Guid Id { get; init; }
    public DateTime Date { get; init; }
    public string Document { get; init; }
    public long FileSize { get; init; }
    public ClientDto Client { get; init; }
    public FolderDto Folder { get; init; }
    public List<DocumentActionDto> AppliedActions { get; init; }
    public List<DocumentActionDto> AvailableActions { get; init; }
}