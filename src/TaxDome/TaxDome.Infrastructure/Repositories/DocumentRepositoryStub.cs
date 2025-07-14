using TaxDome.Domain.Entities;
using TaxDome.Domain.Repositories;

namespace TaxDome.Infrastructure.Repositories;

public class DocumentRepositoryStub : IDocumentRepository
{
    private Client JaneCooper = new Client(new Guid("A3CAC9C5-C924-444F-A4C0-38823E3C9BD6"), "Jane Cooper");
    private Client EstherHoward = new Client(new Guid("9BAAB71D-8A1C-413B-93FE-B996EBA47C6D"), "Esther Howard");
    private Client LeslieAlexander = new Client(new Guid("0888D441-F5FD-403F-A43A-FD7FA5D6BCA2"), "Leslie Alexander");
    
    private Folder SharedWithClient = new Folder(new Guid("C5DAC252-7961-4638-BFBE-8291EF99D3C7"), "Shared with Client");
    private Folder Private = new Folder(new Guid("F796EF33-5B93-40FD-9EB5-D34668843F45"), "Private");
    
    private DocumentAction PendingSignature = new DocumentAction(new Guid("7AC3BF0E-5B3D-47DE-B0A0-68AC68DFBBA2"), "Pending Signature");
    private DocumentAction Approved = new DocumentAction(new Guid("9FB03BCF-D42E-497D-A756-2AB2CF13C6B7"), "Approved");
    private DocumentAction Retry = new DocumentAction(new Guid("7D3B3075-E252-473C-9FD1-A1E7213DC75D"), "Retry");
    private DocumentAction PendingApproval = new DocumentAction(new Guid("3BE186F1-17A4-42D9-AA8B-CFEBA27EECD6"), "Pending Approval");
    private DocumentAction JobProcessing = new DocumentAction(new Guid("7D76BE70-9B0A-4942-9A89-3A05C99CD907"), "Job Processing");
    private DocumentAction InvoiceLinked = new DocumentAction(new Guid("5C0E250A-FD43-4FD4-A5EC-00575B08EDF6"), "Invoice Linked");
    private DocumentAction JobLinked = new DocumentAction(new Guid("FF7B2E8B-93DD-471D-8DAF-0EE8C35F4371"), "Job Linked");
    
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