using QuickMed.DB;

namespace QuickMed.Interface
{
    public interface ICCTemp : IBase
    {
        Task<dynamic> GetCCTempData();
        Task<dynamic> SaveCCTemplate(TblCCTemplate data);
        Task<dynamic> UpdateCCTemplate(TblCCTemplate data);
        Task<dynamic> DeleteAsync(Guid id);  // New method for deleting


    }
}
