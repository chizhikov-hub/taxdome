namespace TaxDome.Domain.Entities;

public class Client
{
    public Guid Id { get; private set; }
    public string Name { get; private set; } = null!;
    
    public virtual List<Document> Documents { get; private set; } = [];
    
    //For EF Core
    private Client() { }
    
    public Client(string name)
    {
        Id = Guid.NewGuid();
        Name = name;
    }
    
    public Client(Guid id, string name)
    {
        Id = id;
        Name = name;
    }
}