using QuickMed.ViewModels;

namespace QuickMed.Interface
{
    public interface IDrug : IBase
    {
        Task<DrugDbVM> GetDataByIdAsync(Guid? Id);

    }
}
