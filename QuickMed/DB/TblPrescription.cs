using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickMed.DB
{
    public class TblPrescription
    {
        [PrimaryKey]
        public Guid Id { get; set; } 
        public string? LicenseKey { get; set; }  // Foreign key reference to TblDoctor
        public Guid? PatientId { get; set; }  // Foreign key reference if needed
        public string? Dx { get; set; }
        public DateTime? PrescriptionDate { get; set; }
        public string? CC { get; set; }  // Chief Complaint
        public string? OE { get; set; }  // Other Examination findings
        public string? IX { get; set; }  // Investigations
        public string? Advice { get; set; }  // Advice for the patient
        public string? PrescriptionDetails { get; set; }  // Advice for the patient
        public decimal? NumberAfter { get; set; }  // Numeric value, perhaps dosage or quantity
        public bool? ISDayAfter { get; set; }  // Indicates if the prescription is for the day after
        public bool? ISMonthAfter { get; set; }  // Indicates if the prescription is for the month after


        public string? CreatedBy { get; set; } = string.Empty;
        public DateTime? CreatedAt { get; set; } = DateTime.Now; // Use UTC time for consistency
        public string? UpdatedBy { get; set; } = string.Empty;
        public DateTime? UpdatedAt { get; set; } = DateTime.Now;
        public string? PrescriptionCode { get; set; }

        public bool IsSynced { get; set; } = false; // Default to false for new records
    }

}
