namespace QuickMed.Interface
{
    public interface IDashboard
    {
        //Task<dynamic> GetMostUsedDXAsync();
        Task<dynamic> GetTotalDashboardData();
        Task<dynamic> GetTotalDxDashboardData();
        Task<dynamic> GetBrandDashboardData();
        //Task<dynamic> GetMostUsedMedicineAsync();
    }
}
