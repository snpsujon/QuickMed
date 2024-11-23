namespace QuickMed.DB
{
    public class TblPrescriptionDetails
    {
        public Guid Id { get; set; }
        public Guid? Brand { get; set; } // Name of the brand
        public Guid? Dose { get; set; } // Dosage information
        public Guid? Duration { get; set; } // Duration of the prescription
        public Guid? Instruction { get; set; } // Instructions for taking the medication
        public Guid? PrescriptionMasterId { get; set; } // Foreign key reference to TblPrescription
        public string? CreatedBy { get; set; } // User who created the record
        public DateTime CreatedAt { get; set; } = DateTime.Now; // Timestamp for creation
        public string? UpdatedBy { get; set; } // User who updated the record
        public DateTime? UpdatedAt { get; set; } // Timestamp for last update
        public bool IsSynced { get; set; } = false; // 0 = Unsynced, 1 = Synced
    }
}
