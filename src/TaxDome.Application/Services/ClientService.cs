using TaxDome.Application.DTOs;
using TaxDome.Domain.Repositories;

namespace TaxDome.Application.Services;

public class ClientService(IClientRepository clientRepository)
{
    public async Task<IReadOnlyCollection<ClientDto>> GetAllClientsAsync(CancellationToken cancellationToken)
    {
        var clients = await clientRepository.GetAllAsync(cancellationToken);
        return clients.Select(d => new ClientDto(d.Id, d.Name)).ToList();
    }
}