using SQLite;

namespace QuickMed.DB
{
    public class TblPrescription
    {
        [PrimaryKey]
        public Guid Id { get; set; }
        public string? LicenseKey { get; set; }  // Foreign key reference to TblDoctor
        public Guid? PatientId { get; set; }  // Foreign key reference if needed
        public DateTime? PrescriptionDate { get; set; }
        public string? PrescriptionCode { get; set; }
        public bool? ISMonthAfter { get; set; }  // Indicates if the prescription is for the month after
        public DateTime? NextMeetingDate { get; set; }
        public int? NextMeetingValue { get; set; }
        public int? Payment { get; set; }
        public string? RefferedBy { get; set; }
        public string? RefferedTo { get; set; }
        public Guid? AdviceId { get; set; }
        public Guid? NoteId { get; set; }
        public Guid? IxId { get; set; }
        public int? Height { get; set; }
        public int? weight { get; set; }
        public string? CreatedBy { get; set; } = string.Empty;
        public DateTime? CreatedAt { get; set; } = DateTime.Now; // Use UTC time for consistency
        public string? UpdatedBy { get; set; } = string.Empty;
        public DateTime? UpdatedAt { get; set; } = DateTime.Now;
        public bool IsSynced { get; set; } = false; // Default to false for new records
    }

}
