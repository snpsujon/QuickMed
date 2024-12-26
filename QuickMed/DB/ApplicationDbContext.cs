using Dapper;
using SQLite;

namespace QuickMed.DB
{
    public class ApplicationDbContext
    {

        public SQLiteAsyncConnection _dbConnection;
        public readonly static string nameSpace = "QuickMed.DB.";
        public const SQLite.SQLiteOpenFlags Flags = SQLite.SQLiteOpenFlags.ReadWrite | SQLite.SQLiteOpenFlags.Create | SQLite.SQLiteOpenFlags.SharedCache;

        private string DataBaseFileName = "QuickMed.DB.db3";
        private string databaseDirectory = Path.Combine(Microsoft.Maui.Storage.FileSystem.AppDataDirectory, "QuickMedDb");


        //public ApplicationDbContext()
        //{
        //    InitializeDatabaseConnection().ConfigureAwait(false).GetAwaiter().GetResult(); // To run it synchronously
        //}

        //public ApplicationDbContext()
        //{
        //    // Empty constructor; database initialization moved to InitializeAsync method.
        //}
        public async Task InitializeAsync()
        {
            await InitializeDatabaseConnection(); // Ensure async initialization completes
        }


        public ApplicationDbContext()
        {
            InitializeDatabaseConnection();
        }



        private async Task InitializeDatabaseConnection()
        {
            if (_dbConnection == null)
            {
                string databasePath = GetDatabasePath();
                _dbConnection = new SQLiteAsyncConnection(databasePath, Flags);
                await InitializeDatabase().ConfigureAwait(false);
            }
        }

        //private async Task InitializeDatabaseConnection()
        //{
        //    if (_dbConnection == null)
        //    {
        //        string databasePath = GetDatabasePath();
        //        _dbConnection = new SQLiteAsyncConnection(databasePath, Flags);
        //        await InitializeDatabase(); 
        //    }
        //}

        public string GetDatabasePath()
        {
            if (!Directory.Exists(databaseDirectory))
            {
                Directory.CreateDirectory(databaseDirectory);
            }

            return Path.Combine(databaseDirectory, DataBaseFileName);
        }


        private async Task InitializeDatabase()
        {
            await _dbConnection.CreateTableAsync<TblPatient>().ConfigureAwait(false);
            await _dbConnection.CreateTableAsync<TblTreatmentTemplate>().ConfigureAwait(false);
            await _dbConnection.CreateTableAsync<TblPrescription>().ConfigureAwait(false);
            await _dbConnection.CreateTableAsync<TblPrescriptionDetails>().ConfigureAwait(false);
            await _dbConnection.CreateTableAsync<TblDoctor>().ConfigureAwait(false);
            await _dbConnection.CreateTableAsync<TblDose>().ConfigureAwait(false);
            await _dbConnection.CreateTableAsync<TblDuration>().ConfigureAwait(false);
            await _dbConnection.CreateTableAsync<TblLicenseInfo>().ConfigureAwait(false);
            await _dbConnection.CreateTableAsync<TblAdviceMaster>().ConfigureAwait(false);
            await _dbConnection.CreateTableAsync<TblAdviceTemplate>().ConfigureAwait(false);
            await _dbConnection.CreateTableAsync<TblAdviceTemplateDetails>().ConfigureAwait(false);
            await _dbConnection.CreateTableAsync<TblCCTemplate>().ConfigureAwait(false);
            await _dbConnection.CreateTableAsync<TblDXTemplate>().ConfigureAwait(false);
            await _dbConnection.CreateTableAsync<TblInstruction>().ConfigureAwait(false);
            await _dbConnection.CreateTableAsync<TblNotesTemplate>().ConfigureAwait(false);
            await _dbConnection.CreateTableAsync<TblNotesTempDetails>().ConfigureAwait(false);
            await _dbConnection.CreateTableAsync<TblTreatmentTempDetails>().ConfigureAwait(false);
            await _dbConnection.CreateTableAsync<TblMixTemplate>().ConfigureAwait(false);
            await _dbConnection.CreateTableAsync<TblFavouriteDrugTemplate>().ConfigureAwait(false);
            await _dbConnection.CreateTableAsync<DrugType>().ConfigureAwait(false);
            await _dbConnection.CreateTableAsync<DrugDosage>().ConfigureAwait(false);
            await _dbConnection.CreateTableAsync<DrugGeneric>().ConfigureAwait(false);
            await _dbConnection.CreateTableAsync<DrugManufacturer>().ConfigureAwait(false);
            await _dbConnection.CreateTableAsync<DrugMedicine>().ConfigureAwait(false);
            await _dbConnection.CreateTableAsync<TblIXDetails>().ConfigureAwait(false);
            await _dbConnection.CreateTableAsync<TblIXTemplate>().ConfigureAwait(false);
            await _dbConnection.CreateTableAsync<TblMixTempDetails>().ConfigureAwait(false);

            await _dbConnection.CreateTableAsync<TblPerceptionTemplate>().ConfigureAwait(false);
            await _dbConnection.CreateTableAsync<TblBrandTemplate>().ConfigureAwait(false);
            await _dbConnection.CreateTableAsync<TblDoseTemplate>().ConfigureAwait(false);
            await _dbConnection.CreateTableAsync<TblInstructionTemplate>().ConfigureAwait(false);
            await _dbConnection.CreateTableAsync<TblDurationTemplate>().ConfigureAwait(false);
            await _dbConnection.CreateTableAsync<TblAdviceTemplate>().ConfigureAwait(false);
            await _dbConnection.CreateTableAsync<TblRefferTemplate>().ConfigureAwait(false);
            await _dbConnection.CreateTableAsync<TblPres_Cc>().ConfigureAwait(false);
            await _dbConnection.CreateTableAsync<TblPres_DH>().ConfigureAwait(false);
            await _dbConnection.CreateTableAsync<TblPres_DX>().ConfigureAwait(false);
            await _dbConnection.CreateTableAsync<TblPres_Ho>().ConfigureAwait(false);
            await _dbConnection.CreateTableAsync<TblPres_MH>().ConfigureAwait(false);
            await _dbConnection.CreateTableAsync<TblPres_Note>().ConfigureAwait(false);
            await _dbConnection.CreateTableAsync<TblPres_OE>().ConfigureAwait(false);
            await _dbConnection.CreateTableAsync<TblPatientReportTemp>().ConfigureAwait(false);
            await _dbConnection.CreateTableAsync<TblPatientReport>().ConfigureAwait(false);
            await _dbConnection.CreateTableAsync<TblPres_CcTemp>().ConfigureAwait(false);
            await _dbConnection.CreateTableAsync<TblPres_DHTemp>().ConfigureAwait(false);
            await _dbConnection.CreateTableAsync<TblPres_DXTemp>().ConfigureAwait(false);
            await _dbConnection.CreateTableAsync<TblPres_HoTemp>().ConfigureAwait(false);
            await _dbConnection.CreateTableAsync<TblPres_MHTemp>().ConfigureAwait(false);
            await _dbConnection.CreateTableAsync<TblPres_NoteTemp>().ConfigureAwait(false);
            await _dbConnection.CreateTableAsync<TblPres_OETemp>().ConfigureAwait(false);
            await _dbConnection.CreateTableAsync<TblDurationNumber>().ConfigureAwait(false);

            // Await the seed data methods to ensure they complete before continuing
            await insertSeedDoseData().ConfigureAwait(false);
            await insertSeedDurationData().ConfigureAwait(false);
            await insertSeedAdviceMasterData().ConfigureAwait(false);
            await insertSeedInstructiomData().ConfigureAwait(false);
            await insertSeedTableDurationNumber().ConfigureAwait(false);
        }


        public async Task CloseConnectionAsync()
        {
            if (_dbConnection != null)
            {
                await _dbConnection.CloseAsync();
                _dbConnection = null;
            }
        }
        public void ReinitializeConnection()
        {
            if (_dbConnection == null)
            {
                string databasePath = GetDatabasePath();
                _dbConnection = new SQLiteAsyncConnection(databasePath, Flags);
                InitializeDatabase().Wait(); // Optionally re-create tables or validate schema
            }
        }


        public async Task<int> CreateAsync<TEntity>(TEntity entity) where TEntity : class
        {
            try
            {
                return await _dbConnection.InsertAsync(entity);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        public async Task<int> CreateMultipleAsync<TEntity>(List<TEntity> entity) where TEntity : class
        {
            try
            {
                return await _dbConnection.InsertAllAsync(entity);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<IEnumerable<T>> ExecuteSqlQueryAsync<T>(string sql) where T : class, new()
        {
            try
            {
                return await _dbConnection.QueryAsync<T>(sql);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<IEnumerable<T>> ExecuteSqlWithParamQueryAsync<T>(string sql, DynamicParameters parameters) where T : class, new()
        {
            try
            {
                return await _dbConnection.QueryAsync<T>(sql, parameters);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error executing SQL query: {ex.Message}");
                throw;
            }
        }





        public async Task<T> ExecuteSqlQueryFirstorDefultAsync<T>(string sql) where T : class, new()
        {
            try
            {
                var result = await _dbConnection.QueryAsync<T>(sql);
                return result.FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<bool> DeleteTableAsync(string tableName)
        {
            try
            {
                var Sql = "DELETE FROM " + tableName;
                var result = await ExecuteSqlQueryAsync<TblDoctor>(Sql);
                return true;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<bool> DeleteTableRowAsync(string tableName, string DeleteColumn, string DeleteValue)
        {
            try
            {
                var Sql = "DELETE FROM " + tableName + " Where " + DeleteColumn + " = '" + DeleteValue + "';";
                var result = await ExecuteSqlQueryAsync<TblDoctor>(Sql);
                return true;
            }
            catch (Exception ex)
            {
                throw;
            }
        }




        public async Task<int> UpdateAsync<TEntity>(TEntity entity) where TEntity : class
        {
            return await _dbConnection.UpdateAsync(entity);
        }

        public async Task<int> DeleteAsync<TEntity>(TEntity entity) where TEntity : class
        {
            try
            {
                return await _dbConnection.DeleteAsync(entity);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<int> AddOrUpdateAsync<TEntity>(TEntity entity) where TEntity : class
        {
            return await _dbConnection.InsertOrReplaceAsync(entity);
        }

        public async Task<List<T>> GetTableRowsAsync<T>(string tableName) where T : class
        {
            try
            {
                //await InitializeAsync();

                //await Task.Run(async () => await InitializeAsync().ConfigureAwait(false));

                object[] obj = new object[] { };
                TableMapping map = new TableMapping(Type.GetType(nameSpace + tableName));
                string query = "SELECT * FROM [" + tableName + "]";

                var result = await _dbConnection.QueryAsync(map, query, obj);
                return result.Cast<T>().ToList();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<object> GetTableRowAsync(string tableName, string column, string value)
        {
            try
            {
                // Assuming `TableMapping` and `_dbConnection` are set up properly
                object[] obj = new object[] { };
                TableMapping map = new TableMapping(Type.GetType(nameSpace + tableName));

                // Use a named parameter instead of anonymous object
                var parameters = new { value };

                // Async query execution
                string query = $"SELECT * FROM {tableName} WHERE {column} = {value}";  // Use parameterized query to avoid SQL injection
                var result = await _dbConnection.QueryAsync(map, query, parameters);

                return result.FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }










        private async Task insertSeedDoseData()
        {
            // Implement seeding logic for TblDose
            try
            {
                var count = await _dbConnection.Table<TblDose>().CountAsync();

                // Insert data only if the table is empty
                if (count == 0)
                {
                    // Define initial data
                    var initialDoses = new List<TblDose>
                    {

                        new TblDose { Id = Guid.NewGuid(), Name = "দিনে ২ বার মুখ ধুবেন" },
                        new TblDose { Id = Guid.NewGuid(), Name = "দিনে ২ বার সারা শরীরে ব্যবহার করবেন" },
                        new TblDose { Id = Guid.NewGuid(), Name = "দিনে ২ বার মুখে ব্যবহার করবেন" },
                        new TblDose { Id = Guid.NewGuid(), Name = "প্রয়োজন অনুযায়ী ঠোঁটে লাগাবেন" },
                        new TblDose { Id = Guid.NewGuid(), Name = "রোদে / চুলার পাশে যাবার ১০ মিনিট আগে ব্যবহার করবেন" },
                        new TblDose { Id = Guid.NewGuid(), Name = "আক্রান্ত স্থানে দিনে ২ বার ব্যবহার করবেন" },
                        new TblDose { Id = Guid.NewGuid(), Name = "সপ্তাহে ৩ দিন গোসলের সময় ব্যবহার করবেন" },
                        new TblDose { Id = Guid.NewGuid(), Name = "সকালে মুখে ব্যবহার করবেন" },
                        new TblDose { Id = Guid.NewGuid(), Name = "রাতে মুখে ব্যবহার করবেন" },
                        new TblDose { Id = Guid.NewGuid(), Name = "দিনে ২ বার সারা শরীরে / আক্রান্ত স্থানে ব্যবহার করবেন" },
                        new TblDose { Id = Guid.NewGuid(), Name = "দিনে ৩-৪ বার আক্রান্ত স্থানে ধুবেন" },
                        new TblDose { Id = Guid.NewGuid(), Name = "গোসলের সময় সারা গায়ে মেখে ৫ মিনিট পর ধুয়ে ফেলবেন" },
                        new TblDose { Id = Guid.NewGuid(), Name = "গোসলে ব্যবহার করবেন" },
                        new TblDose { Id = Guid.NewGuid(), Name = "গোসলের সময় সারা গায়ে মেখে ৩ মিনিট পর ধুয়ে ফেলবেন" },
                        new TblDose { Id = Guid.NewGuid(), Name = "প্রতিদিন সকালে (একই সময়ে) ১ পাম্প করে উভয় বাহুতে ব্যবহার করবেন" },
                        new TblDose { Id = Guid.NewGuid(), Name = "সন্ধ্যায় পরিমান মতো জেল আক্রান্ত স্থানে মেখে ২ ঘন্টা পর ধুয়ে ফেলবেন" },
                        new TblDose { Id = Guid.NewGuid(), Name = "আক্রান্ত স্থানে দিনে ২ বার ব্যবহার করবেন" },
                        new TblDose { Id = Guid.NewGuid(), Name = "সকাল ও রাতে মাথায় তালুতে (আক্রান্ত স্থানে) ৭ টি করে স্প্রে করবেন" },
                        new TblDose { Id = Guid.NewGuid(), Name = "১০ দিন (ব্যথা হলে ভরা পেটে)" },
                        new TblDose { Id = Guid.NewGuid(), Name = "০+০+১" },
                        new TblDose { Id = Guid.NewGuid(), Name = "০+১+০" },
                        new TblDose { Id = Guid.NewGuid(), Name = "(0+0+1)" },
                        new TblDose { Id = Guid.NewGuid(), Name = "(0+1+0)" },
                        new TblDose { Id = Guid.NewGuid(), Name = "0+1/2+0" },
                        new TblDose { Id = Guid.NewGuid(), Name = "0+0+1/2" },
                        new TblDose { Id = Guid.NewGuid(), Name = "১ ফোটা করে দুই নাকের ছিদ্রে দিবেন ৩বার" },
                        new TblDose { Id = Guid.NewGuid(), Name = "1 চামচ প্রথম দিন রাত্রে এবং ৭ দিন পর রাত্রে 1 চামচ খাবে" },
                        new TblDose { Id = Guid.NewGuid(), Name = "০১ টি পায়খানার রাস্তায় দেবেন" },
                        new TblDose { Id = Guid.NewGuid(), Name = "১টি ট্যাবলেট সকালে খাবেন" },
                        new TblDose { Id = Guid.NewGuid(), Name = "১টি ট্যাবলেট ঘুমানোর আগে খাবেন" },
                        new TblDose { Id = Guid.NewGuid(), Name = "১ চামচ" },
                        new TblDose { Id = Guid.NewGuid(), Name = "১+০+০" },
                        new TblDose { Id = Guid.NewGuid(), Name = "১+০+১" },
                        new TblDose { Id = Guid.NewGuid(), Name = "১+১+১" },
                        new TblDose { Id = Guid.NewGuid(), Name = "১+১+১+১" },
                        new TblDose { Id = Guid.NewGuid(), Name = "হাফ চামুচ দিনে ১ বার" },
                        new TblDose { Id = Guid.NewGuid(), Name = "হাফ চামুচ দিনে ২ বার" }

                    };

                    // Insert the initial data into the table
                    await _dbConnection.InsertAllAsync(initialDoses);
                }
            }

            catch (Exception ex)
            {

                throw;
            }

        }

        private async Task insertSeedInstructiomData()
        {
            // Implement seeding logic for TblDose
            try
            {
                var count = await _dbConnection.Table<TblInstruction>().CountAsync();

                // Insert data only if the table is empty
                if (count == 0)
                {
                    // Define initial data
                    var initialDoses = new List<TblInstruction>
                    {

                        new TblInstruction { Id = Guid.NewGuid(), Name = "খাবার আগে" },
                        new TblInstruction { Id = Guid.NewGuid(), Name = "খাবার পরে" },
                        new TblInstruction { Id = Guid.NewGuid(), Name = "গোসলের আগে" },
                        new TblInstruction { Id = Guid.NewGuid(), Name = "গোসলের পরে" },
                        new TblInstruction { Id = Guid.NewGuid(), Name = "গোসলের সময়" },




                    };

                    // Insert the initial data into the table
                    await _dbConnection.InsertAllAsync(initialDoses);
                }
            }

            catch (Exception ex)
            {

                throw;
            }

        }
        private async Task insertSeedTableDurationNumber()
        {
            // Implement seeding logic for TblDose
            try
            {
                var count = await _dbConnection.Table<TblDurationNumber>().CountAsync();

                // Insert data only if the table is empty
                if (count == 0)
                {
                    var initialDoses = new List<TblDurationNumber>();
                    for (int i = 1; i < 32; i++)
                    {
                        var tblDurationNumber = new TblDurationNumber { Id = Guid.NewGuid(), Name = i.ToString() };
                        initialDoses.Add(tblDurationNumber);

                    }

                    // Insert the initial data into the table
                    await _dbConnection.InsertAllAsync(initialDoses);
                }
            }

            catch (Exception ex)
            {

                throw;
            }

        }

        private async Task insertSeedDurationData()
        {
            // Implement seeding logic for TblDuration

            try
            {
                var count = await _dbConnection.Table<TblDuration>().CountAsync();

                // Insert data only if the table is empty
                if (count == 0)
                {
                    // Define initial data
                    var initialDoses = new List<TblDuration>
            {

                                new TblDuration { Id = Guid.NewGuid(), Name = "০১ দিন" },
                                new TblDuration { Id = Guid.NewGuid(), Name = "১০ দিন" },
                                new TblDuration { Id = Guid.NewGuid(), Name = "১৪ দিন" },
                                new TblDuration { Id = Guid.NewGuid(), Name = "১৫ দিন" },
                                new TblDuration { Id = Guid.NewGuid(), Name = "১ সপ্তাহ" },
                                new TblDuration { Id = Guid.NewGuid(), Name = "১ মাস" },
                                new TblDuration { Id = Guid.NewGuid(), Name = "২ সপ্তাহ" },
                                new TblDuration { Id = Guid.NewGuid(), Name = "২ মাস" },
                                new TblDuration { Id = Guid.NewGuid(), Name = "২১ দিন" },
                                new TblDuration { Id = Guid.NewGuid(), Name = "২৮ দিন" },
                                new TblDuration { Id = Guid.NewGuid(), Name = "৩ দিন" },
                                new TblDuration { Id = Guid.NewGuid(), Name = "৩+৩+৩" },
                                new TblDuration { Id = Guid.NewGuid(), Name = "০৩ দিন" },
                                new TblDuration { Id = Guid.NewGuid(), Name = "৩ সপ্তাহ" },
                                new TblDuration { Id = Guid.NewGuid(), Name = "৩ মাস" },
                                new TblDuration { Id = Guid.NewGuid(), Name = "30" },
                                new TblDuration { Id = Guid.NewGuid(), Name = "৪ সপ্তাহ" },
                                new TblDuration { Id = Guid.NewGuid(), Name = "৪ মাস" },
                                new TblDuration { Id = Guid.NewGuid(), Name = "০৫ দিন" },
                                new TblDuration { Id = Guid.NewGuid(), Name = "০৬ দিন" },
                                new TblDuration { Id = Guid.NewGuid(), Name = "৬ সপ্তাহ" },
                                new TblDuration { Id = Guid.NewGuid(), Name = "০৭ দিন" },
                                new TblDuration { Id = Guid.NewGuid(), Name = "৭ দিন" },
                                new TblDuration { Id = Guid.NewGuid(), Name = "১ম এবং ৭ম দিন" },
                                new TblDuration { Id = Guid.NewGuid(), Name = "রাতে ঘুমানোর ১ ঘণ্টা আগে" },
                                new TblDuration { Id = Guid.NewGuid(), Name = "চলবে।" },
                                new TblDuration { Id = Guid.NewGuid(), Name = "চলবে" },
                                new TblDuration { Id = Guid.NewGuid(), Name = "পরবর্তী নির্দেশনা না দেওয়া পর্যন্ত চলবে।" }


            };

                    // Insert the initial data into the table
                    await _dbConnection.InsertAllAsync(initialDoses);
                }
            }

            catch (Exception ex)
            {

                throw;
            }
        }
        public async Task<bool> IsConnectedToInternet()
        {
            //return NetworkInterface.GetIsNetworkAvailable();
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.Timeout = TimeSpan.FromSeconds(5); // Set a timeout for the request
                    HttpResponseMessage response = await client.GetAsync("https://www.google.com");
                    return response.IsSuccessStatusCode;
                }
            }
            catch
            {
                return false; // If there’s any exception, return false
            }

        }
        private async Task insertSeedAdviceMasterData()
        {
            // Implement seeding logic for TblDuration

            try
            {
                var count = await _dbConnection.Table<TblAdviceMaster>().CountAsync();

                // Insert data only if the table is empty
                if (count == 0)
                {
                    // Define initial data
                    var initialDoses = new List<TblAdviceMaster>
                    {
                        new TblAdviceMaster { AdviceName = "নিয়মিত ঔষধ খাবেন।" },
                        new TblAdviceMaster { AdviceName = "পরবর্তী সাক্ষাতে প্রেসক্রিপশন ও এই ফাইলটি সাথে আনবেন।" },
                        new TblAdviceMaster { AdviceName = "জ্বর ১০২ এর বেশি হলে ১টি Napa Suppository (500 mg) পায়ুপথের মাধ্যমে দেবেন।" },
                        new TblAdviceMaster { AdviceName = "ভিটামিন সি যুক্ত ফল বেশি পরিমাণে খাবেন।" },
                        new TblAdviceMaster { AdviceName = "প্রতিদিন অন্তত ৮ গ্লাস পানি পান করবেন।" },
                        new TblAdviceMaster { AdviceName = "চাপের-লেখা উত্তম চিকিৎসকের পরামর্শ ছাড়া বন্ধ করবেন না।" },
                        new TblAdviceMaster { AdviceName = "ধুমপান ও তামাকজাত দ্রব্য ব্যবহার নিষেধ।" },
                        new TblAdviceMaster { AdviceName = "অতিরিক্ত কায়িক পরিশ্রম নিষেধ।" },
                        new TblAdviceMaster { AdviceName = "পান্তা ভাত খাওয়া নিষেধ।" },
                        new TblAdviceMaster { AdviceName = "পাতে আলু, চিপস, ফ্রাইড রাইস ও উচ্চ ক্যালোরিযুক্ত খাবার কম খাবেন।" },
                        new TblAdviceMaster { AdviceName = "প্রতিদিন সকালের ১৫ মিনিটের সময় ৫ মিনিট করে হাঁটবেন।" },
                        new TblAdviceMaster { AdviceName = "সপ্তাহে ২ বার ব্লাড প্রেসার পরীক্ষা করবেন।" }
                    };

                    // Insert the initial data into the table
                    await _dbConnection.InsertAllAsync(initialDoses);
                }
            }

            catch (Exception ex)
            {

                throw;
            }
        }



    }



}
