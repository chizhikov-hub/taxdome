using TaxDome.Domain.Entities;
using TaxDome.Domain.Repositories;

namespace TaxDome.Infrastructure.Repositories;

public class DocumentRepositoryStub : IDocumentRepository
{
    public async Task<IReadOnlyCollection<Document>> GetAllAsync(CancellationToken cancellationToken)
    {
        await Task.Delay(3000, cancellationToken);

        var result = new List<Document>();
        for (int i = 0; i < 1000; i++)
        {
            result.Add(new Document($"Tax_Return_2024_{i}.pdf",     2411725, "Jane Cooper",      "Shared with Client", ["Pending Signature", "Job Linked"],                      ["Approved", "Invoice Linked"],    DateTime.Today));
            result.Add(new Document($"Invoice_March_2024_{i}.pdf",  345703,  "Jane Cooper",      "Shared with Client", ["Approved", "Job Linked", "Invoice Linked"],             ["Pending Signature"],             DateTime.Today));
            result.Add(new Document($"Contract_Amendment_{i}.pdf",  1258292, "Esther Howard",    "Shared with Client", [],                                                       ["Retry"],                         DateTime.Today));
            result.Add(new Document($"Financial_Summary_{i}.xslx",  1258292, "Esther Howard",    "Private",            ["Pending Approval", "Job Processing", "Invoice Linked"], ["Pending Signature"],             DateTime.Today.AddDays(-1)));
            result.Add(new Document($"Bank_Statements_Q1_{i}.xslx", 363111,  "Leslie Alexander", "Private",            [],                                                       ["Pending Signature", "Approved"], DateTime.Today.AddDays(-1)));
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