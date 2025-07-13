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
            Id = d.Id,
            Document = d.FileName,
            FileSize = d.FileSize,
            Date = d.Date,
            Client = d.Client,
            Folder = d.Folder,
            AppliedActions = d.AppliedActions,
            AvailableActions = d.AvailableActions
        }).ToList();
    }

    public Task AddAsync(Document document, CancellationToken cancellationToken)
    {
        return documentRepository.AddAsync(document, cancellationToken);
    }
}