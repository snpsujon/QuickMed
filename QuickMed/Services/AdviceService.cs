using QuickMed.DB;
using QuickMed.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace QuickMed.Services
{
    public class AdviceService : BaseServices, IAdvice
    {
        public AdviceService(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<dynamic> GetAdviceMasterData()
        {
            try
            {            
                return await _context.GetTableRowsAsync<TblAdviceMaster>("TblAdviceMaster");
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task<dynamic> GetAdviceTemplateData()
        {
            try
            {
                return true;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task<dynamic> SaveAdviceTemplate(TblAdviceTemplate data)
        {
            try
            {
                var saveTemplate = await _context.CreateAsync<TblAdviceTemplate>(data);
                return true;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task<dynamic> SaveAdviceTemplateDetails(List<TblAdviceTemplateDetails> data)
        {
            try
            {
               var saveDetails= await _context.CreateMultipleAsync<TblAdviceTemplateDetails>(data);
                return true;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
