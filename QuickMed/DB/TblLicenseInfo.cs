using SQLite;

namespace QuickMed.DB
{
    public class TblLicenseInfo
    {
        [PrimaryKey]
        public Guid Id { get; set; }
        public string LicenseKey { get; set; }
        public DateTime ExpirationDate { get; set; }

    }
    public class LicenseApiResponse
    {
        public LicenseData Data { get; set; }
    }

    public class LicenseData
    {
        public int IsValid { get; set; }
        public DateTime ExpirationDate { get; set; }
        public TblDoctor Doctor { get; set; }
    }
}