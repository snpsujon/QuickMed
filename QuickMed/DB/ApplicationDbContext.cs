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


        public ApplicationDbContext()
        {
            InitializeDatabaseConnection();
        }

        private void InitializeDatabaseConnection()
        {
            if (_dbConnection == null)
            {
                string databasePath = GetDatabasePath();
                _dbConnection = new SQLiteAsyncConnection(databasePath, Flags);
                InitializeDatabase();
            }
        }

        public string GetDatabasePath()
        {
            if (!Directory.Exists(databaseDirectory))
            {
                Directory.CreateDirectory(databaseDirectory);
            }

            return Path.Combine(databaseDirectory, DataBaseFileName);
        }


        private async void InitializeDatabase()
        {
            await _dbConnection.CreateTableAsync<TblPatient>();
            await _dbConnection.CreateTableAsync<TblTreatmentTemplate>();
            await _dbConnection.CreateTableAsync<TblBrand>();
            await _dbConnection.CreateTableAsync<TblGeneric>();
            await _dbConnection.CreateTableAsync<TblCompany>();
            await _dbConnection.CreateTableAsync<TblPrescription>();
            await _dbConnection.CreateTableAsync<TblPrescriptionDetails>();
            await _dbConnection.CreateTableAsync<TblDoctor>();
            await _dbConnection.CreateTableAsync<TblDose>();
            await _dbConnection.CreateTableAsync<TblDuration>();
            await _dbConnection.CreateTableAsync<TblLicenseInfo>();

            insertSeedDoseData();
            insertSeedDurationData();
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
                InitializeDatabase(); // Optionally re-create tables or validate schema
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

        public object GetTableRow(string tableName, string column, string value)
        {
            object[] obj = new object[] { };
            TableMapping map = new TableMapping(Type.GetType(nameSpace + tableName));
            string query = "SELECT * FROM " + tableName + " WHERE " + column + "='" + value + "'";

            return _dbConnection.QueryAsync(map, query, obj).Result.FirstOrDefault();
        }

        private async void insertSeedDoseData()
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

        private async void insertSeedDurationData()
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
    }
}
