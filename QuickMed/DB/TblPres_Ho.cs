using SQLite;

namespace QuickMed.DB
{
    public class TblPres_Ho
    {
        [PrimaryKey]
        public Guid? Id { get; set; }
        public Guid? Pres_ID { get; set; }
        public bool? HTN { get; set; }
        public bool? DM { get; set; }
        public bool? Asthma { get; set; }
        public bool? COPD { get; set; }
        public bool? IHD { get; set; }
        public bool? CKD { get; set; }
        public bool? CVD { get; set; }
        public bool? CLD { get; set; }
        public bool? Smoking { get; set; }
        public bool? TobaccoChewing { get; set; }
        public bool? Malignancy { get; set; }
        public bool? Allergy { get; set; }
        public bool? DrugAbuse { get; set; }
        public bool? Depression { get; set; }
        public string? FreeText { get; set; }
    }
}
