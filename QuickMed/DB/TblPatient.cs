using SQLite;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuickMed.DB
{
    public class TblPatient
    {
        [PrimaryKey]
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Age { get; set; }
        public int? Gender { get; set; }
        public string? Address { get; set; }
        public string? Mobile { get; set; }
        public string? Code { get; set; }
        public DateTime AdmissionDate { get; set; } = DateTime.Now; // Use UTC time for consistency
        public string? CreatedBy { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.Now; // Use UTC time for consistency
        public string? UpdatedBy { get; set; } = string.Empty;
        public DateTime? UpdatedAt { get; set; } = DateTime.Now;

        public bool IsSynced { get; set; } = false;
        [NotMapped]
        public string Dx { get; set; }
        [NotMapped]
        public DateTime PrescriptionDate { get; set; } = DateTime.Now;
    }
}
