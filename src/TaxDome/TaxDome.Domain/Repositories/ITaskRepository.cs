namespace TaxDome.Domain.Repositories;

public interface ITaskRepository
{
    Task<Task> GetByIdAsync(Guid id);
    Task<IEnumerable<Task>> GetAllAsync();
    Task AddAsync(Task task);
    Task UpdateAsync(Task task);
    Task DeleteAsync(Guid id);
}