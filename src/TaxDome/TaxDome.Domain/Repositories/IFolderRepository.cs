using TaxDome.Domain.Entities;

namespace TaxDome.Domain.Repositories;

public interface IFolderRepository
{
    Task<Folder> GetByIdAsync(Guid id);
    Task<Folder> CreateAsync(Guid id, string name);
}