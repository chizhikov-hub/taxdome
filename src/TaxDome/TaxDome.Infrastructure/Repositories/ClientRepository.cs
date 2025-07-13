using Microsoft.EntityFrameworkCore;
using TaxDome.Domain.Entities;
using TaxDome.Domain.Repositories;

namespace TaxDome.Infrastructure.Repositories;

public class ClientRepository(ApplicationDbContext dbContext) : IClientRepository
{
    public async Task<Client> GetByIdAsync(Guid id)
    {
        return await dbContext.Set<Client>().FirstOrDefaultAsync(d => d.Id == id);
    }

    public async Task<Client> CreateAsync(Guid id, string name)
    {
        var newClient = new Client(id, name);

        await dbContext.Set<Client>().AddAsync(newClient);
        await dbContext.SaveChangesAsync();

        return newClient;
    }
}