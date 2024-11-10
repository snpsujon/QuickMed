namespace QuickMed.Interface
{
    public interface ILicense
    {
        Task<dynamic> ValidateLicense(string LicenseNo);
    }
}
