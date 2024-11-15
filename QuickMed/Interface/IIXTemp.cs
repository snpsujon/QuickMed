namespace QuickMed.Interface
{
    public interface IIXTemp : IBase
    {
        Task<dynamic> GetCCTempData();
        Task<dynamic> DeleteAsync(Guid id);


    }
}
