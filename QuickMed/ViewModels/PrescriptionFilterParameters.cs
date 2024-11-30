namespace QuickMed.ViewModels
{
    public class PrescriptionFilterParameters
    {
        public string? Mobile { get; set; }
        public string? PrescriptionCode { get; set; }
        public string? DxTempId { get; set; }
        public string? BrandId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }

}
