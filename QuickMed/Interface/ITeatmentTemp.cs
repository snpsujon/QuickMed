using QuickMed.DB;

namespace QuickMed.Interface
{
    public interface ITeatmentTemp : IBase
    {
        Task<dynamic> GetAdviceDataById(string AdviceID);
        Task<dynamic> GetIXDataById(string Id);
        Task<dynamic> GetNoteDataById(string Id);
        Task<dynamic> SaveTreatmentTemp(TblTreatmentTemplate tblTreatmentTemplate);
        Task<dynamic> UpdateTreatmentTemp(TblTreatmentTemplate tblTreatmentTemplate);
        Task<dynamic> SaveTreatmentTempDetails(List<TblTreatmentTempDetails> data);
        Task<dynamic> GetBrandsSameGenaric(string brandid);
        Task<dynamic> DeleteTreatmentTemp(string TempId);
        Task<dynamic> DeleteAllDetails(string TempId);
    }
}
