using QuickMed.DB;

namespace QuickMed.ViewModels
{
    public class PerviewPresVM
    {
        public Guid? Id { get; set; }
        public string? Name { get; set; }
        public string? Age { get; set; }
        public string? Gender { get; set; }
        public string? Address { get; set; }
        public string? Mobile { get; set; }
        public string? Code { get; set; }
        public Guid? PatientId { get; set; }
        public DateTime AdmissionDate { get; set; } = DateTime.Now;
        public Guid? PrescriptionId { get; set; }
        public string? LicenseKey { get; set; }
        public string? Dx { get; set; }
        public DateTime? PrescriptionDate { get; set; } = DateTime.Now;
        public string? CC { get; set; }
        public string? OE { get; set; }
        public string? IX { get; set; }
        public string? Advice { get; set; }
        public string? PrescriptionDetails { get; set; }
        public decimal? NumberAfter { get; set; }
        public bool? ISDayAfter { get; set; }
        public bool? ISMonthAfter { get; set; }
        public bool IsSynced { get; set; } = false;
        public TblDoctor TblDoctor { get; set; } = new TblDoctor();
        public string? GenderS { get; set; }
        public string? PrescriptionCode { get; set; }
    }

}