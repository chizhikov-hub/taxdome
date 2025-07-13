using TaxDome.Domain.Entities;

namespace TaxDome.Domain.Repositories;

public interface IDocumentActionRepository
{
    Task<IReadOnlyCollection<DocumentAction>> GetByIdsAsync(IEnumerable<Guid> ids);
}