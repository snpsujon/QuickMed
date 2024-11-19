namespace QuickMed.Interface
{
    public interface IPrescription
    {
        Task<dynamic> GetAll();
        Task<dynamic> DeleteAsync(Guid id);
    }
}
