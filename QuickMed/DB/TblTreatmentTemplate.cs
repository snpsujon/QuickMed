using SQLite;

namespace QuickMed.DB
{
    public class TblTreatmentTemplate
    {
        [PrimaryKey]
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? CreatedBy { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow; // Use UTC time for consistency
        public string? UpdatedBy { get; set; } = string.Empty;
        public DateTime? UpdatedAt { get; set; }
        public bool IsSynced { get; set; } = false; // Default to false for new records
        public decimal? NumberAfter { get; set; }  // Numeric value, perhaps dosage or quantity
        public bool? ISDayAfter { get; set; }  // Indicates if the prescription is for the day after
        public bool? ISMonthAfter { get; set; }

        public Guid AdviceId { get; set; }



        //public string? PrescriptionDetails { get; set; } = string.Empty;

    }
}
