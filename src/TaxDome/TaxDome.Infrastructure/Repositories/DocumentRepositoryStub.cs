using TaxDome.Domain.Entities;
using TaxDome.Domain.Repositories;

namespace TaxDome.Infrastructure.Repositories;

public class DocumentRepositoryStub : IDocumentRepository
{
    private Client JaneCooper = new Client("Jane Cooper");
    private Client EstherHoward = new Client("Esther Howard");
    private Client LeslieAlexander = new Client("Leslie Alexander");
    
    private Folder SharedWithClient = new Folder("Shared with Client");
    private Folder Private = new Folder("Private");
    
    private DocumentAction PendingSignature = new DocumentAction("Pending Signature");
    private DocumentAction Approved = new DocumentAction("Approved");
    private DocumentAction Retry = new DocumentAction("Retry");
    private DocumentAction PendingApproval = new DocumentAction("Pending Approval");
    private DocumentAction JobProcessing = new DocumentAction("Job Processing");
    private DocumentAction InvoiceLinked = new DocumentAction("Invoice Linked");
    private DocumentAction JobLinked = new DocumentAction("Job Linked");
    
    public async Task<IReadOnlyCollection<Document>> GetAllAsync(CancellationToken cancellationToken)
    {
        await Task.Delay(3000, cancellationToken);

        var result = new List<Document>();
        for (int i = 0; i < 1000; i++)
        {
            result.Add(new Document($"Tax_Return_2024_{i}.pdf",     2411725, JaneCooper,      SharedWithClient, [PendingSignature, JobLinked],                   [Approved, InvoiceLinked],    DateTime.Today));
            result.Add(new Document($"Invoice_March_2024_{i}.pdf",  345703,  JaneCooper,      SharedWithClient, [Approved, JobLinked, InvoiceLinked],            [PendingSignature],           DateTime.Today));
            result.Add(new Document($"Contract_Amendment_{i}.pdf",  1258292, EstherHoward,    SharedWithClient, [],                                              [Retry],                      DateTime.Today));
            result.Add(new Document($"Financial_Summary_{i}.xslx",  1258292, EstherHoward,    Private,          [PendingApproval, JobProcessing, InvoiceLinked], [PendingSignature],           DateTime.Today.AddDays(-1)));
            result.Add(new Document($"Bank_Statements_Q1_{i}.xslx", 363111,  LeslieAlexander, Private,          [],                                              [PendingSignature, Approved], DateTime.Today.AddDays(-1)));
        }

        return result;
    }

    public Task<Document> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task AddAsync(Document document, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task UpdateAsync(Document document, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}