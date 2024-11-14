using QuickMed.DB;

namespace QuickMed.Interface
{
    public interface IDXTemp : IBase
    {
        Task<dynamic> GetCCTempData();
        Task<dynamic> SaveCCTemplate(TblDXTemplate data);
        Task<dynamic> UpdateCCTemplate(TblDXTemplate data);
        Task<dynamic> DeleteAsync(Guid id);


    }
}
