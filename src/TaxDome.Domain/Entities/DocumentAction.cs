namespace TaxDome.Domain.Entities;

public class DocumentAction
{
    public Guid Id { get; private set; }
    public string Name { get; private set; } = null!;
    
    public virtual List<Document> AppliedToDocuments { get; private set; } = [];
    public virtual List<Document> AvailableForDocuments { get; private set; } = [];
    
    //For EF Core
    private DocumentAction() { }
    
    public DocumentAction(string name)
    {
        Id = Guid.NewGuid();
        Name = name;
    }
    
    public DocumentAction(Guid id, string name)
    {
        Id = id;
        Name = name;
    }
}