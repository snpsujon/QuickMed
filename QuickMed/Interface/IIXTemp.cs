using QuickMed.DB;

namespace QuickMed.Interface
{
    public interface IIXTemp : IBase
    {
        Task<dynamic> GetCCTempData();
        Task<dynamic> DeleteAsync(Guid id);
        Task<dynamic> SaveAsync(TblIXTemplate data);
        Task<dynamic> SaveTemplateDetails(List<TblIXDetails> data);

    }
}
