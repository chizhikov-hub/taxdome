using Microsoft.EntityFrameworkCore;
using TaxDome.Domain.Entities;
using TaxDome.Domain.Repositories;

namespace TaxDome.Infrastructure.Repositories;

public class FolderRepository(ApplicationDbContext dbContext) : IFolderRepository
{
    public async Task<Folder> GetByIdAsync(Guid id)
    {
        return await dbContext.Set<Folder>().FirstOrDefaultAsync(d => d.Id == id);
    }

    public async Task<Folder> CreateAsync(Guid id, string name)
    {
        var newFolder = new Folder(id, name);

        await dbContext.Set<Folder>().AddAsync(newFolder);
        await dbContext.SaveChangesAsync();

        return newFolder;
    }
}