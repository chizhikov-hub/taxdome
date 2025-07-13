using TaxDome.Domain.Enums;

namespace TaxDome.Domain.Entities;

public class Document
{
    public Guid Id { get; private set; }
    public DateTime Date { get; private set; }
    public string FileName { get; private set; }
    public long FileSize { get; private set; }
    public string Client { get; private set; }
    public string Folder { get; private set; }
    public List<string> AppliedActions { get; private set; }
    public List<string> AvailableActions { get; private set; }

    private Document() { } 

    public Document(string fileName, long fileSize, string client, string folder, List<string> appliedActions, List<string> availableActions, DateTime? date = null)
    {
        Id = Guid.NewGuid();
        Date = date ?? DateTime.UtcNow;
        FileName = fileName;
        FileSize = fileSize;
        Client = client;
        Folder = folder;
        AppliedActions = appliedActions;
        AvailableActions = availableActions;
    }
}