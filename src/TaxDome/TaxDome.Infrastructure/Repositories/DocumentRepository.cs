using Microsoft.EntityFrameworkCore;
using TaxDome.Domain.Entities;
using TaxDome.Domain.Repositories;

namespace TaxDome.Infrastructure.Repositories;

public class DocumentRepository: IDocumentRepository
{
    private readonly DbContext _dbContext;

    public DocumentRepository(DbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IReadOnlyCollection<Document>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await _dbContext.Set<Document>()
            .ToListAsync(cancellationToken);
    }

    public async Task<Document> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _dbContext.Set<Document>()
            .FirstOrDefaultAsync(d => d.Id == id, cancellationToken);
    }

    public async Task AddAsync(Document document, CancellationToken cancellationToken)
    {
        await _dbContext.Set<Document>().AddAsync(document, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(Document document, CancellationToken cancellationToken)
    {
        _dbContext.Set<Document>().Update(document);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}