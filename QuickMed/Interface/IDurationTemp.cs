using QuickMed.DB;

namespace QuickMed.Interface
{
    public interface IDurationTemp : IBase
    {
        Task<dynamic> GetAsync();
        Task<dynamic> SaveAsync(TblDuration data);
        Task<dynamic> UpdateAsync(TblDuration data);
        Task<dynamic> DeleteAsync(Guid id);
    }
}
