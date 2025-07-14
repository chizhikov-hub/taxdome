using TaxDome.Application.DTOs;
using TaxDome.Domain.Repositories;

namespace TaxDome.Application.Services;

public class FolderService(IFolderRepository folderRepository)
{
    public async Task<IReadOnlyCollection<FolderDto>> GetAllFoldersAsync(CancellationToken cancellationToken)
    {
        var folders = await folderRepository.GetAllAsync(cancellationToken);
        return folders.Select(d => new FolderDto(d.Id, d.Name)).ToList();
    }
}