namespace QuickMed.Interface
{
    public interface IMixTemp : IBase
    {
        Task<dynamic> GetAsync();
        Task<dynamic> GetAllMedicine();
    }
}
