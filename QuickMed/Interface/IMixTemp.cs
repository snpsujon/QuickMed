using QuickMed.DB;

namespace QuickMed.Interface
{
    public interface IMixTemp : IBase
    {
        Task<dynamic> GetAsync();
        Task<dynamic> GetAllMedicine();
        Task<dynamic> SaveMixTemp(TblMixTemplate tblTreatmentTemplate);
        Task<dynamic> SaveMixTempDetails(List<TblMixTempDetails> data);
        Task<dynamic> GetMasterDataById(string Id);
        Task<dynamic> GetDetailsDataById(string Id);
        Task<dynamic> DeleteAsync(Guid id);

    }
}
