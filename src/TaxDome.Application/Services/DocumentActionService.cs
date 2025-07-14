using TaxDome.Application.DTOs;
using TaxDome.Domain.Repositories;

namespace TaxDome.Application.Services;

public class DocumentActionService(IDocumentActionRepository documentActionRepository)
{
    public async Task<IReadOnlyCollection<DocumentActionDto>> GetAllDocumentActionsAsync(CancellationToken cancellationToken)
    {
        var documentActions = await documentActionRepository.GetAllAsync(cancellationToken);
        return documentActions.Select(d => new DocumentActionDto(d.Id, d.Name)).ToList();
    }
}