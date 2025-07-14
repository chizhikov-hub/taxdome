namespace TaxDome.Domain.Entities;

public class Folder
{
    public Guid Id { get; private set; }
    public string Name { get; private set; } = null!;
    
    public virtual List<Document> Documents { get; private set; } = [];
    
    //For EF Core
    private Folder() { }
    
    public Folder(string name)
    {
        Id = Guid.NewGuid();
        Name = name;
    }
    
    public Folder(Guid id, string name)
    {
        Id = id;
        Name = name;
    }
}