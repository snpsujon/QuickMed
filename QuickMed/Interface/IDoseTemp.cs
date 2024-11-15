using QuickMed.DB;

namespace QuickMed.Interface
{
    public interface IDoseTemp : IBase
    {
        Task<dynamic> GetAsync();
        Task<dynamic> SaveAsync(TblDose data);
        Task<dynamic> UpdateAsync(TblDose data);
    }
}
