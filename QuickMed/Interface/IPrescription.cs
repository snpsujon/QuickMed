using QuickMed.DB;
using QuickMed.ViewModels;

namespace QuickMed.Interface
{
    public interface IPrescription
    {
        Task<dynamic> GetAll();
        Task<dynamic> DeleteAsync(Guid id);
        Task<FavouriteDrugTempVM> GetFavDrugbyId(Guid id);

        Task<List<FavouriteDrugTempVM>> TblTreatmentTempDetails(Guid id);
        Task<dynamic> GetCCList();
        Task<dynamic> GetDXList();
        Task<dynamic> GetDurationsList();


        Task<dynamic> SavePresCC(List<TblPres_Cc> datas);
        Task<dynamic> SavePresMH(List<TblPres_MH> datas);
        Task<dynamic> SavePresOE(List<TblPres_OE> datas);
        Task<dynamic> SavePresDH(List<TblPres_DH> datas);
        Task<dynamic> SavePresDX(List<TblPres_DX> datas);
        Task<dynamic> SavePatientReport(List<TblPatientReport> datas);
        Task<dynamic> SavePrescriptionDetails(List<TblPrescriptionDetails> datas);
        Task<dynamic> SavePresHO(TblPres_Ho data);
        Task<dynamic> SavePrescription(TblPrescription data);
        Task<dynamic> GetPorPResult(string input, bool isMobile);


    }
}
