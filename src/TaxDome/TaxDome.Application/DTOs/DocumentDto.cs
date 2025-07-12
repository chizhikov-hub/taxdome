using TaxDome.Domain.Enums;

namespace TaxDome.Application.DTOs;

public class DocumentDto
{
    public class DocumentItem
    {
        public DateTime Date { get; set; }
        public string Document { get; set; }
        public string Client { get; set; }
        public string Folder { get; set; }
        public List<string> AppliedActions { get; set; }
        public List<string> AvailableActions { get; set; }
    
        public string DateGroup
        {
            get
            {
                if (Date.Date == DateTime.Today)
                    return "Сегодня";
                if (Date.Date == DateTime.Today.AddDays(-1))
                    return "Вчера";
                return Date.ToString("yyyy-MM-dd");
            }
        }
    }

    
    // public Guid Id { get; set; }
    // public string Name { get; set; } = string.Empty;
    // public string Type { get; set; } = string.Empty;
    // public DateTime CreatedAt { get; set; }
    // public string CreatedBy { get; set; } = string.Empty;
    // public DocumentStatus Status { get; set; }
}