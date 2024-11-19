using SQLite;

namespace QuickMed.DB
{
    public class DrugMedicine
    {
        [PrimaryKey]
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public Guid? TypeId { get; set; }
        public Guid? DosageId { get; set; }
        public Guid? GenericId { get; set; }
        public Guid? ManufacturerId { get; set; }
        public string? Strength { get; set; }
        public string? PackageContainer { get; set; }
        public string? PackageSize { get; set; }
        public bool IsSynced { get; set; } = false;
        public string? CreatedBy { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow; // Use UTC time for consistency
        public string? UpdatedBy { get; set; } = string.Empty;
        public DateTime? UpdatedAt { get; set; }
    }
}
