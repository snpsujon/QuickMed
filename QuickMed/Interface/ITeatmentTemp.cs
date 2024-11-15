using QuickMed.DB;

namespace QuickMed.Interface
{
    public interface ITeatmentTemp : IBase
    {
        Task<dynamic> GetAdviceDataById(string AdviceID);
        Task<dynamic> SaveTreatmentTemp(TblTreatmentTemplate tblTreatmentTemplate);
        Task<dynamic> SaveTreatmentTempDetails(List<TblTreatmentTempDetails> data);
        Task<dynamic> DeleteTreatmentTemp(string TempId);
    }
}
