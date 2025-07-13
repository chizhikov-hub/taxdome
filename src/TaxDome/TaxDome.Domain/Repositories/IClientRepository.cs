using TaxDome.Domain.Entities;

namespace TaxDome.Domain.Repositories;

public interface IClientRepository
{
    Task<Client> GetByIdAsync(Guid id);
    Task<Client> CreateAsync(Guid id, string name);
}