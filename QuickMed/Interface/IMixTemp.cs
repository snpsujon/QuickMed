using QuickMed.DB;

namespace QuickMed.Interface
{
    public interface IMixTemp : IBase
    {
        Task<dynamic> GetAsync();
        Task<dynamic> GetAllMedicine();
        Task<dynamic> SaveMixTemp(TblMixTemplate tblTreatmentTemplate);
        Task<dynamic> SaveMixTempDetails(List<TblMixTempDetails> data);

    }
}
