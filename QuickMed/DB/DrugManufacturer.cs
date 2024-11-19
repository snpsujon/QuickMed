using SQLite;

namespace QuickMed.DB
{
    public class DrugManufacturer
    {
        [PrimaryKey]
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public bool IsSynced { get; set; } = false;
        public string? CreatedBy { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow; // Use UTC time for consistency
        public string? UpdatedBy { get; set; } = string.Empty;
        public DateTime? UpdatedAt { get; set; }
    }
}
