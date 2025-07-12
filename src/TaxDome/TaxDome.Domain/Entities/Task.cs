namespace TaxDome.Domain.Entities
{
    public class Task
    {
        public Guid Id { get; private set; }
        public string Title { get; private set; }
        public string Description { get; private set; }
        public Enums.TaskStatus Status { get; private set; }
        public DateTime DueDate { get; private set; }

        private Task() { }

        public static Task Create(string title, string description, DateTime dueDate)
        {
            return new Task
            {
                Id = Guid.NewGuid(),
                Title = title,
                Description = description,
                Status = Enums.TaskStatus.New,
                DueDate = dueDate
            };
        }

        public void MarkAsCompleted()
        {
            Status = Enums.TaskStatus.Completed;
        }
    }
}