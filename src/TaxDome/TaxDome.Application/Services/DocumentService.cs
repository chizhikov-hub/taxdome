using TaxDome.Application.DTOs;
using TaxDome.Domain.Repositories;

namespace TaxDome.Application.Services;

public class DocumentService
{
    private readonly IDocumentRepository _documentRepository;

    public DocumentService(IDocumentRepository documentRepository)
    {
        _documentRepository = documentRepository;
    }

    public async Task<IReadOnlyCollection<DocumentDto>> GetAllDocumentsAsync(CancellationToken cancellationToken)
    {
        var documents = await _documentRepository.GetAllAsync(cancellationToken);
        return documents.Select(d => new DocumentDto
        {
            Id = d.Id,
            Name = d.Name,
            Type = d.Type,
            CreatedAt = d.CreatedAt,
            CreatedBy = d.CreatedBy,
            Status = d.Status
        }).ToList();
    }
}