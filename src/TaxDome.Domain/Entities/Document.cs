namespace TaxDome.Domain.Entities;

public class Document
{
    public Guid Id { get; private set; }
    public DateTime Date { get; private set; }
    public string FileName { get; private set; } = null!;
    public long FileSize { get; private set; }
    public Guid ClientId { get; private set; }
    public Guid FolderId { get; private set; }
    
    public virtual Client Client { get; private set; } = null!;
    public virtual Folder Folder { get; private set; } = null!;
    public virtual List<DocumentAction> AppliedActions { get; private set; } = [];
    public virtual List<DocumentAction> AvailableActions { get; private set; } = [];

    //For EF Core
    private Document() { } 

    public Document(string fileName, long fileSize, Client client, Folder folder, ICollection<DocumentAction> appliedActions, ICollection<DocumentAction> availableActions, DateTime? date = null)
    {
        Id = Guid.NewGuid();
        Date = date ?? DateTime.UtcNow;
        FileName = fileName;
        FileSize = fileSize;
        ClientId = client.Id;
        Client = client;
        FolderId = folder.Id;
        Folder = folder;
        AppliedActions = appliedActions.ToList();
        AvailableActions = availableActions.ToList();
    }
}