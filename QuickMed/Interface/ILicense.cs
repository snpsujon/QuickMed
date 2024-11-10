namespace QuickMed.Interface
{
    public interface ILicense : IBase
    {
        Task<dynamic> ValidateLicense(string LicenseNo);
    }
}
