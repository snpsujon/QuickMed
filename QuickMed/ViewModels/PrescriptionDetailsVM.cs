using QuickMed.DB;

namespace QuickMed.ViewModels
{
    public class PrescriptionDetailsVM
    {
        public TblPrescription tblPrescription { get; set; }
        public TblPatient tblPatient { get; set; }
        public List<TblAdviceTemplateDetails> tblAdviceTemplateDetails { get; set; }
        public List<TblPres_Cc> tblPres_Ccs { get; set; }
        public TblPres_Ho tblPres_Ho { get; set; }
        public List<TblPres_MH> tblPres_MHs { get; set; }
        public List<TblPres_OE> tblPres_OEs { get; set; }
        public List<TblPres_DX> tblPres_DXes { get; set; }
        public List<TblIXDetails> ixDetails { get; set; }
        public List<TblNotesTempDetails> noteDetails { get; set; }
        public List<TblPatientReport> tblPatientReports { get; set; }
        public List<TblPrescriptionDetails> rxDetails { get; set; }


    }
}
