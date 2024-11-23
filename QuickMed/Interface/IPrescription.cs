using QuickMed.ViewModels;

namespace QuickMed.Interface
{
    public interface IPrescription
    {
        Task<dynamic> GetAll();
        Task<dynamic> DeleteAsync(Guid id);
        Task<FavouriteDrugTempVM> GetFavDrugbyId(Guid id);

        Task<List<FavouriteDrugTempVM>> TblTreatmentTempDetails(Guid id);
        Task<dynamic> GetCCList();
        Task<dynamic> GetDXList();
        Task<dynamic> GetDurationsList();


    }
}
