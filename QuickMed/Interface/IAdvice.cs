using QuickMed.DB;

namespace QuickMed.Interface
{
    public interface IAdvice : IBase
    {
        Task<dynamic> GetAdviceMasterData();
        Task<dynamic> GetAdviceTemplateData();
        Task<dynamic> SaveAdviceTemplate(TblAdviceTemplate data);
        Task<dynamic> SaveAdviceTemplateDetails(List<TblAdviceTemplateDetails> data);
        Task<dynamic> DeleteAdviceTemplete(string sql);
        Task<dynamic> DeleteAdviceDetails(string sql);
        Task<dynamic> GetTemplateById(string sql);
        Task<dynamic> GetTemplateDetailsById(string sql);
        Task<dynamic> UpdateAdviceTemplate(TblAdviceTemplate data);
        Task<dynamic> GetOnlyAdviceData();
    }
}
