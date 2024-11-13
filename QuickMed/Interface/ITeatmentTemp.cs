namespace QuickMed.Interface
{
    public interface ITeatmentTemp : IBase
    {
        Task<dynamic> ValidateLicense(string LicenseNo);
    }
}
