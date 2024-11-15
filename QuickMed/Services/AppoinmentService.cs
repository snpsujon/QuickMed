using QuickMed.DB;
using QuickMed.Interface;
using QuickMed.ViewModels;

namespace QuickMed.Services
{
    public class AppoinmentService : BaseServices, IAppoinment
    {
        public AppoinmentService(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<dynamic> GetAsync()
        {
            try
            {
                var query = $"Select Id, Name, Age, Address, Mobile, AdmissionDate,\r\nCASE When Gender = 1 THEN 'M' \r\n\tWhen Gender = 2 THEN 'F'\r\n\tElse 'O' \r\nEND as GenderName\r\n From TblPatient";
                var result = await _context.ExecuteSqlQueryAsync<PatientVM>(query);
                return result;

            }
            catch (Exception ex)
            {

                throw;
            }
        }
        public async Task<dynamic> SaveAsync(PatientVM data)
        {
            try
            {
                TblPatient tblData = new()
                {
                    Id = Guid.NewGuid(),
                    Name = data.Name,
                    Age = data.Age,
                    Gender = data.Gender,
                    Address = data.Address,
                    Mobile = data.Mobile,
                    AdmissionDate = data.AdmissionDate,
                    Weight = data.Weight,
                    Code = data.Code
                };
                var saveTemplate = await _context.CreateAsync<TblPatient>(tblData);
                return true;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task<dynamic> UpdateAsync(PatientVM data)
        {
            try
            {
                int rowsAffected = await _context.UpdateAsync(data);
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while updating the template.", ex);
            }
        }
        public async Task<dynamic> DeleteAsync(Guid id)
        {
            try
            {
                var q = $"DELETE FROM TblPatient WHERE Id = '{id}'";

                var deleteResult = await _context.ExecuteSqlQueryFirstorDefultAsync<PatientVM>(q);

                return true;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }


    }
}
