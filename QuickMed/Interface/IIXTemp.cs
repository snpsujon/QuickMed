using QuickMed.DB;

namespace QuickMed.Interface
{
    public interface IIXTemp : IBase
    {
        Task<dynamic> GetCCTempData();
        Task<dynamic> GetAsync();
        Task<dynamic> DeleteAsync(Guid id);
        Task<dynamic> DeleteDetailsAsync(Guid id);
        Task<dynamic> SaveAsync(TblIXTemplate data);
        Task<dynamic> UpdateAsync(TblIXTemplate data);
        Task<dynamic> GetDataById(string id);
        Task<dynamic> SaveTemplateDetails(List<TblIXDetails> data);

    }
}
