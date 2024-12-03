namespace QuickMed.ViewModels
{
    public class PrescriptionVM
    {
        public Guid Id { get; set; } // Adjust to Guid if required
        public string PrescriptionCode { get; set; }
        public DateTime? PrescriptionDate { get; set; }
        public int PatientId { get; set; } // Adjust to Guid if required
        public string PatientName { get; set; }
        public string MobileNumber { get; set; }
        public string Address { get; set; }
        public string Plan { get; set; }
        public string Dx { get; set; }
    }
}
