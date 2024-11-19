namespace QuickMed.Interface
{
    public interface IFavouriteDrug : IBase
    {
        Task<dynamic> GetAsync();
    }
}
