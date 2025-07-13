using Microsoft.EntityFrameworkCore;
using TaxDome.Domain.Entities;
using TaxDome.Domain.Repositories;

namespace TaxDome.Infrastructure.Repositories;

public class DocumentActionRepository(ApplicationDbContext dbContext) : IDocumentActionRepository
{
    public async Task<IReadOnlyCollection<DocumentAction>> GetByIdsAsync(IEnumerable<Guid> ids)
    {     
        return await dbContext.DocumentActions
            .Where(action => ids.Contains(action.Id))
            .ToListAsync();
    }
}