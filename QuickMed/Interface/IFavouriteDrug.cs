using QuickMed.DB;

namespace QuickMed.Interface
{
    public interface IFavouriteDrug : IBase
    {
        Task<dynamic> GetAsync();
        Task<dynamic> SaveFavouriteDrugTemp(TblFavouriteDrugTemplate TblFavouriteDrugTemplate);
    }
}
