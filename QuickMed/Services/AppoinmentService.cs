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
                var query = @"
            SELECT 
                Id,
                Code,
                Weight,
                Name,
                Age,
                Address,
                Mobile,
                AdmissionDate,
                CASE 
                    WHEN Gender = 1 THEN 'M'
                    WHEN Gender = 2 THEN 'F'
                    ELSE 'O'
                END AS GenderName,
                Gender
            FROM 
                TblPatient";

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
                    Code = await AutoGenCode(),
                    HeightInch = data.HeightInch

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

        private async Task<string> AutoGenCode()
        {
            var result = await GetAsync();
            // Ensure the result is enumerable
            var patients = result as IEnumerable<dynamic>;
            string generatedCode;

            if (patients != null)
            {
                // Take the last item or default
                var lastPatient = patients.LastOrDefault();

                // Initialize lastCode to a default value
                int lastCode = 0;
                if (lastPatient != null && int.TryParse(lastPatient.Code.ToString(), out lastCode))
                {
                    generatedCode = (lastCode + 1).ToString();
                }
                else
                {
                    generatedCode = DateTime.Now.Year + "0001";
                }
            }
            else
            {
                generatedCode = DateTime.Now.Year + "0001";
            }

            return generatedCode;
        }





    }
}
