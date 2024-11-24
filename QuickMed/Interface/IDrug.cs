namespace QuickMed.Interface
{
    public interface IDrug : IBase
    {
        Task<dynamic> GetDataByIdAsync();
    }
}
