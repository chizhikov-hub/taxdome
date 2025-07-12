using Microsoft.EntityFrameworkCore;
using TaxDome.Domain.Entities;
using TaxDome.Domain.Repositories;

namespace TaxDome.Infrastructure.Repositories;

public class DocumentRepository(ApplicationDbContext dbContext) : IDocumentRepository
{
    public async Task<IReadOnlyCollection<Document>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await dbContext.Set<Document>()
            .ToListAsync(cancellationToken);
    }

    public async Task<Document> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await dbContext.Set<Document>()
            .FirstOrDefaultAsync(d => d.Id == id, cancellationToken);
    }

    public async Task AddAsync(Document document, CancellationToken cancellationToken)
    {
        await dbContext.Set<Document>().AddAsync(document, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(Document document, CancellationToken cancellationToken)
    {
        dbContext.Set<Document>().Update(document);
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}