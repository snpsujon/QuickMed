using SQLite;

namespace QuickMed.DB
{
    public class TblPatient
    {
        [PrimaryKey]
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Age { get; set; }
        public string? Gender { get; set; }
        public string? Address { get; set; }
        public string? Mobile { get; set; }
        public string? Code { get; set; }
        public decimal? Weight { get; set; }
        public decimal? HeightInch { get; set; }
        public DateTime AdmissionDate { get; set; } = DateTime.Now; // Use UTC time for consistency
        public string? CreatedBy { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.Now; // Use UTC time for consistency
        public string? UpdatedBy { get; set; } = string.Empty;
        public DateTime? UpdatedAt { get; set; } = DateTime.Now;

        public bool IsSynced { get; set; } = false;

    }
}
