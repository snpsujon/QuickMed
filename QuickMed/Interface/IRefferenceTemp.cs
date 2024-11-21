using QuickMed.DB;

namespace QuickMed.Interface
{
    public interface IRefferenceTemp : IBase
    {
        Task<dynamic> GetAsync();
        Task<dynamic> SaveAsync(TblRefferTemplate data);
        Task<dynamic> UpdateAsync(TblRefferTemplate data);
        Task<dynamic> DeleteAsync(Guid id);
    }
}
