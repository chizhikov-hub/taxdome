using TaxDome.Domain.Entities;

namespace TaxDome.Domain.Repositories;

public interface IDocumentRepository
{
    Task<IReadOnlyCollection<Document>> GetAllAsync(CancellationToken cancellationToken);
    Task<Document?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task AddAsync(Document document, CancellationToken cancellationToken);
    Task UpdateAsync(Document document, CancellationToken cancellationToken);
}