using SQLite;

namespace QuickMed.DB
{
    public class TblPatientReport
    {

        [PrimaryKey]
        public Guid Id { get; set; }
        public Guid? PatientId { get; set; }
        public Guid? PresId { get; set; }
        public string? ReportName { get; set; }

        public DateTime? ReportDate { get; set; }
        public string? Result { get; set; }
        public string? Unit { get; set; }
    }
    public class TblPatientReportTemp
    {

        [PrimaryKey]
        public Guid Id { get; set; }
        public Guid? PatientId { get; set; }
        public Guid? PresId { get; set; }
        public string? ReportName { get; set; }

        public DateTime? ReportDate { get; set; }
        public string? Result { get; set; }
        public string? Unit { get; set; }
    }
}
