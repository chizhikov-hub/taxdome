using TaxDome.Application.DTOs;
using TaxDome.Domain.Entities;
using TaxDome.Domain.Repositories;

namespace TaxDome.Application.Services;

public class DocumentService(IDocumentRepository documentRepository, IClientRepository clientRepository, IFolderRepository folderRepository, IDocumentActionRepository actionRepository)
{
    public async Task<IReadOnlyCollection<DocumentDto>> GetAllDocumentsAsync(CancellationToken cancellationToken)
    {
        var documents = await documentRepository.GetAllAsync(cancellationToken);
        return documents.Select(d => new DocumentDto
        {
            Id = d.Id,
            Document = d.FileName,
            FileSize = d.FileSize,
            Date = d.Date,
            Client = new ClientDto(d.Client.Id, d.Client.Name),            
            Folder = new FolderDto(d.Folder.Id, d.Folder.Name),
            AppliedActions = d.AppliedActions.Select(a => new DocumentActionDto(a.Id, a.Name)).ToList(),
            AvailableActions = d.AvailableActions.Select(a => new DocumentActionDto(a.Id, a.Name)).ToList()
        }).ToList();
    }

    public async Task AddAsync(DocumentDto documentDto, CancellationToken cancellationToken)
    {
        var document = new Document(
            documentDto.Document,
            documentDto.FileSize,
            await GetClientById(documentDto.Client.Id) ?? await CreateClient(documentDto.Client.Id, documentDto.Client.Name),
            await GetFolderById(documentDto.Folder.Id) ?? await Createfolder(documentDto.Folder.Id, documentDto.Folder.Name),
            await GetDocumentActionsByIds(documentDto.AppliedActions.Select(x => x.Id)),
            await GetDocumentActionsByIds(documentDto.AvailableActions.Select(x => x.Id)),
            documentDto.Date
        );
    
        await documentRepository.AddAsync(document, cancellationToken);
    }
    
    private Task<Client> GetClientById(Guid clientId)
    {
        return clientRepository.GetByIdAsync(clientId);
    }
    
    private Task<Client> CreateClient(Guid id, string name)
    {
        return clientRepository.CreateAsync(id, name);
    }

    private Task<Folder> GetFolderById(Guid folderId)
    {
        return folderRepository.GetByIdAsync(folderId);
    }
    
    private Task<Folder> Createfolder(Guid id, string name)
    {
        return folderRepository.CreateAsync(id, name);
    }

    private async Task<List<DocumentAction>> GetDocumentActionsByIds(IEnumerable<Guid> actionIds)
    {        
        return (await actionRepository.GetByIdsAsync(actionIds)).ToList();
    }
}