using Microsoft.AspNetCore.Components;
using QuickMed.DB;

namespace QuickMed.BaseComponent
{
    class License : ComponentBase
    {
        private List<TblLicenseInfo> tblLicenses = new();
        private TblLicenseInfo model = new();
        private TblDoctor doctorModel = new();
        private string StatusMessage = "Please enter your license key.";
        private int isLicenseValid = 0;


    }
}
