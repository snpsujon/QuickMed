using SQLite;

namespace QuickMed.DB
{
    public class TblTreatmentTempDetails
    {
        [PrimaryKey]
        public Guid Id { get; set; }
        public string TreatmentTempId { get; set; }
        public string BrandId { get; set; }
        public string DoseId { get; set; }
        public string DurationId { get; set; }
        public string InstructionId { get; set; }
        public string? CreatedBy { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.Now; // Use UTC time for consistency
        public string? UpdatedBy { get; set; } = string.Empty;
        public DateTime? UpdatedAt { get; set; } = DateTime.Now;
        public bool IsSynced { get; set; } = false;
    }
}
