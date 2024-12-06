namespace QuickMed.DB
{

    public class TblDoctor
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? DoctorNameBn { get; set; }
        public string? PassedMedicalDegree { get; set; }
        public string? PassedMedicalDegreeBn { get; set; }
        public string? Designation { get; set; }
        public string? DesignationBn { get; set; }
        public string? Department { get; set; }
        public string? DepartmentBn { get; set; }
        public string? Address { get; set; }
        public string? AddressBn { get; set; }
        public string? Mobile { get; set; }
        public string? MobileBn { get; set; }
        public string? ChemberHospital { get; set; }
        public string? ChemberHospitalBn { get; set; }
        public string? ChemberAddress { get; set; }
        public string? ChemberAddressBn { get; set; }
        public string? PhoneNumber { get; set; }
        public string? PhoneBn { get; set; }
        public string? DoctorPhnSL { get; set; }
        public string? DoctorPhnSLBn { get; set; }
        public string? StartTime { get; set; }
        public string? EndTime { get; set; }
        public string? OffDay { get; set; }
        public string? OffDayBn { get; set; }
        public string? OffDayNotes { get; set; }
        public string? OffDayNotesBn { get; set; }
        public string? BmdcRegNo { get; set; }
        public string? LicenseKey { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool? IsSynced { get; set; }
    }

}
