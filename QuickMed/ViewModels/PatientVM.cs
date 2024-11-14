namespace QuickMed.ViewModels
{
    public class PatientVM
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Age { get; set; }
        public int? Gender { get; set; }
        public int? GenderName { get; set; }
        public string? Address { get; set; }
        public string? Mobile { get; set; }
        public string? Code { get; set; }
        public decimal? Weight { get; set; }
        public DateTime AdmissionDate { get; set; } = DateTime.Now; // Use UTC time for consistency
        public string? CreatedBy { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.Now; // Use UTC time for consistency
        public string? UpdatedBy { get; set; } = string.Empty;
        public DateTime? UpdatedAt { get; set; } = DateTime.Now;

        public bool IsSynced { get; set; } = false;

        public string Dx { get; set; }

        public DateTime PrescriptionDate { get; set; } = DateTime.Now;
    }
}
