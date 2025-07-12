using TaxDome.Application.DTOs;
using TaxDome.Domain.Entities;
using TaxDome.Domain.Repositories;

namespace TaxDome.Application.Services;

public class DocumentService(IDocumentRepository documentRepository)
{
    public async Task<IReadOnlyCollection<DocumentDto>> GetAllDocumentsAsync(CancellationToken cancellationToken)
    {
        var documents = await documentRepository.GetAllAsync(cancellationToken);
        return documents.Select(d => new DocumentDto
        {
            // Id = d.Id,
            // Name = d.Name,
            // Type = d.Type,
            // CreatedAt = d.CreatedAt,
            // CreatedBy = d.CreatedBy,
            // Status = d.Status
        }).ToList();
    }

    public Task AddAsync(Document document, CancellationToken cancellationToken)
    {
        return documentRepository.AddAsync(document, cancellationToken);
    }
}