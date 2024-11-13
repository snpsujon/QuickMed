using QuickMed.DB;

namespace QuickMed.Interface
{
    public interface ICCTemp : IBase
    {
        Task<dynamic> GetCCTempData();
        Task<dynamic> SaveCCTemplate(TblCCTemplate data);
    }
}
