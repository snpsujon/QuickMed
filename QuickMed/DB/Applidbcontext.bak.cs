//using SQLite;

//namespace QuickMed.DB
//{
//    public class ApplicationDbContext
//    {
//        public SQLiteAsyncConnection _dbConnection;

//        public readonly static string nameSpace = "QuickMed.DB.";

//        //public const string DataBaseFileName = "QuickMed.DB.db3";

//        //public static string databasePath = Path.Combine(FileSystem.AppDataDirectory, DataBaseFileName);

//        public const SQLite.SQLiteOpenFlags Flags =
//            SQLite.SQLiteOpenFlags.ReadWrite |
//            SQLite.SQLiteOpenFlags.Create |
//            SQLite.SQLiteOpenFlags.SharedCache;

//        //public ApplicationDbContext()
//        //{
//        //    if (_dbConnection is null)
//        //    {
//        //        string nameSpace = "QuickMed.DB.";
//        //        string DataBaseFileName = "QuickMed.DB.db3";
//        //        string databasePath = Path.Combine(FileSystem.AppDataDirectory, DataBaseFileName);

//        //        _dbConnection = new SQLiteAsyncConnection(databasePath, Flags);
//        //        _dbConnection.CreateTableAsync<Patient>();
//        //    }
//        //}

//        public ApplicationDbContext()
//        {
//            if (_dbConnection is null)
//            {
//                // Define the path on the D drive for the database file
//                string DataBaseFileName = "QuickMed.DB.db3";
//                string databaseDirectory = @"D:\QuickMedDb"; // Folder on D drive

//                // Ensure the directory exists
//                if (!Directory.Exists(databaseDirectory))
//                {
//                    Directory.CreateDirectory(databaseDirectory);
//                }

//                string databasePath = Path.Combine(databaseDirectory, DataBaseFileName);

//                // Initialize the SQLite connection with the specified path
//                _dbConnection = new SQLiteAsyncConnection(databasePath, Flags);

//                // Create the Patient table if it does not exist


//                _dbConnection.CreateTableAsync<TblPatient>().Wait();
//                _dbConnection.CreateTableAsync<TblTreatmentTemplate>().Wait();
//                _dbConnection.CreateTableAsync<TblBrand>().Wait();
//                _dbConnection.CreateTableAsync<TblGeneric>().Wait();
//                _dbConnection.CreateTableAsync<TblCompany>().Wait();
//                _dbConnection.CreateTableAsync<TblPrescription>().Wait();
//                _dbConnection.CreateTableAsync<TblPrescriptionDetails>().Wait();
//                _dbConnection.CreateTableAsync<TblDoctor>().Wait();
//                _dbConnection.CreateTableAsync<TblDose>().Wait();
//                _dbConnection.CreateTableAsync<TblDuration>().Wait();
//                _dbConnection.CreateTableAsync<TblLicenseInfo>().Wait();

//                insertSeedDoseData();
//                insertSeedDurationData();
//            }
//        }

//        public async Task CloseConnectionAsync()
//        {
//            if (_dbConnection != null)
//            {
//                await _dbConnection.CloseAsync(); // Close the active connection
//                _dbConnection = null;
//            }
//        }

//        public void ReinitializeConnection()
//        {
//            if (_dbConnection == null)
//            {
//                string DataBaseFileName = "QuickMed.DB.db3";
//                string databaseDirectory = @"D:\QuickMedDb";
//                string databasePath = Path.Combine(databaseDirectory, DataBaseFileName);

//                _dbConnection = new SQLiteAsyncConnection(databasePath, Flags);
//            }
//        }

//        public async Task<int> CreateAsync<TEntity>(TEntity entity) where TEntity : class
//        {
//            try
//            {
//                return await _dbConnection.InsertAsync(entity);
//            }
//            catch (Exception e)
//            {
//                Console.WriteLine(e);
//                throw;
//            }


//        }
//        public async Task<IEnumerable<T>> ExecuteSqlQueryAsync<T>(string sql) where T : class, new()
//        {
//            try
//            {
//                return await _dbConnection.QueryAsync<T>(sql);
//            }
//            catch (Exception ex)
//            {

//                throw;
//            }
//            // Ensures that T has a parameterless constructor

//        }


//        public async Task<int> UpdateAsync<TEntity>(TEntity entity) where TEntity : class
//        {
//            return await _dbConnection.UpdateAsync(entity);
//        }
//        public async Task<int> DeleteAsync<TEntity>(TEntity entity) where TEntity : class
//        {
//            try
//            {
//                return await _dbConnection.DeleteAsync(entity);
//            }
//            catch (Exception e)
//            {
//                Console.WriteLine(e);
//                throw;
//            }


//        }

//        public async Task<int> AddOrUpdateAsync<TEntity>(TEntity entity) where TEntity : class
//        {
//            return await _dbConnection.InsertOrReplaceAsync(entity);
//        }

//        public async Task<List<T>> GetTableRowsAsync<T>(string tableName) where T : class
//        {
//            try
//            {
//                object[] obj = new object[] { };
//                TableMapping map = new TableMapping(Type.GetType(nameSpace + tableName));
//                string query = "SELECT * FROM [" + tableName + "]"; // Fixed syntax

//                // Await the asynchronous query
//                var result = await _dbConnection.QueryAsync(map, query, obj);

//                // Cast the result to List<T>
//                return result.Cast<T>().ToList();
//            }
//            catch (Exception e)
//            {
//                Console.WriteLine(e);
//                throw;
//            }
//        }


//        public object GetTableRow(string tableName, string column, string value)
//        {
//            object[] obj = new object[] { };
//            TableMapping map = new TableMapping(Type.GetType(nameSpace + tableName));
//            string query = "SELECT * FROM " + tableName + " WHERE " + column + "='" + value + "'";

//            return _dbConnection.QueryAsync(map, query, obj).Result.FirstOrDefault();
//        }


//        private async void insertSeedDoseData()
//        {
//            try
//            {
//                var count = await _dbConnection.Table<TblDose>().CountAsync();

//                // Insert data only if the table is empty
//                if (count == 0)
//                {
//                    // Define initial data
//                    var initialDoses = new List<TblDose>
//        {

//            new TblDose { Id = Guid.NewGuid(), Name = "১০ দিন (ব্যথা হলে ভরা পেটে)" },
//            new TblDose { Id = Guid.NewGuid(), Name = "১টি ট্যাবলেট রাতে - খালি পেটে - ০৫ দিন ১টি ট্যাবলেট প্রতিদিন সন্ধ্যায় - ১৪ দিন ১+১+১+১ - খাবার পর - ০৫ দিন" },
//            new TblDose { Id = Guid.NewGuid(), Name = "০+০+১" },
//            new TblDose { Id = Guid.NewGuid(), Name = "০+১+০" },
//            new TblDose { Id = Guid.NewGuid(), Name = "(0+0+1)" },
//            new TblDose { Id = Guid.NewGuid(), Name = "(0+1+0)" },
//            new TblDose { Id = Guid.NewGuid(), Name = "0+1/2+0" },
//            new TblDose { Id = Guid.NewGuid(), Name = "0+0+1/2" },
//            new TblDose { Id = Guid.NewGuid(), Name = "১ টি ট্যাবলেট রাতে খাবেন" },
//            new TblDose { Id = Guid.NewGuid(), Name = "১ ফোটা করে দুই নাকের ছিদ্রে দিবেন ৩বার" },
//            new TblDose { Id = Guid.NewGuid(), Name = "১চামচ+০+১চামচ" },
//            new TblDose { Id = Guid.NewGuid(), Name = "1 চামচ প্রথম দিন রাত্রে এবং ৭ দিন পর রাত্রে 1 চামচ খাবে" },
//            new TblDose { Id = Guid.NewGuid(), Name = "০১ টি পায়খানার রাস্তায় দেবেন" },
//            new TblDose { Id = Guid.NewGuid(), Name = "১টি ট্যাবলেট সকালে খাবেন" },
//            new TblDose { Id = Guid.NewGuid(), Name = "১টি ট্যাবলেট ঘুমানোর আগে খাবেন" },
//            new TblDose { Id = Guid.NewGuid(), Name = "১ চামচ" },
//            new TblDose { Id = Guid.NewGuid(), Name = "1 Vial I/V 6 hourly (Four times daily)" },
//            new TblDose { Id = Guid.NewGuid(), Name = "1 Vial I/V 8 hourly (Three times daily)" },
//            new TblDose { Id = Guid.NewGuid(), Name = "1 Vial I/V Once Daily" },
//            new TblDose { Id = Guid.NewGuid(), Name = "1 Vial I/V Once Daily very slowly" },
//            new TblDose { Id = Guid.NewGuid(), Name = "১+০+০" },
//            new TblDose { Id = Guid.NewGuid(), Name = "১+০+১" },
//            new TblDose { Id = Guid.NewGuid(), Name = "১টি ট্যাবলেট সকালে ও ১টি ট্যাবলেট রাতে খাবেন" },
//            new TblDose { Id = Guid.NewGuid(), Name = "১+১+১" },
//            new TblDose { Id = Guid.NewGuid(), Name = "১+১+১+১" },
//            new TblDose { Id = Guid.NewGuid(), Name = "হাফ চামুচ দিনে ১ বার" },
//            new TblDose { Id = Guid.NewGuid(), Name = "হাফ চামুচ দিনে ২ বার" }

//        };

//                    // Insert the initial data into the table
//                    await _dbConnection.InsertAllAsync(initialDoses);
//                }
//            }

//            catch (Exception ex)
//            {

//                throw;
//            }

//        }

//        private async void insertSeedDurationData()
//        {
//            try
//            {
//                var count = await _dbConnection.Table<TblDuration>().CountAsync();

//                // Insert data only if the table is empty
//                if (count == 0)
//                {
//                    // Define initial data
//                    var initialDoses = new List<TblDuration>
//{

//                    new TblDuration { Id = Guid.NewGuid(), Name = "০১ দিন" },
//                    new TblDuration { Id = Guid.NewGuid(), Name = "১০ দিন" },
//                    new TblDuration { Id = Guid.NewGuid(), Name = "১৪ দিন" },
//                    new TblDuration { Id = Guid.NewGuid(), Name = "১৫ দিন" },
//                    new TblDuration { Id = Guid.NewGuid(), Name = "১ সপ্তাহ" },
//                    new TblDuration { Id = Guid.NewGuid(), Name = "১ মাস" },
//                    new TblDuration { Id = Guid.NewGuid(), Name = "২ সপ্তাহ" },
//                    new TblDuration { Id = Guid.NewGuid(), Name = "২ মাস" },
//                    new TblDuration { Id = Guid.NewGuid(), Name = "২১ দিন" },
//                    new TblDuration { Id = Guid.NewGuid(), Name = "২৮ দিন" },
//                    new TblDuration { Id = Guid.NewGuid(), Name = "৩ দিন" },
//                    new TblDuration { Id = Guid.NewGuid(), Name = "৩+৩+৩" },
//                    new TblDuration { Id = Guid.NewGuid(), Name = "০৩ দিন" },
//                    new TblDuration { Id = Guid.NewGuid(), Name = "৩ সপ্তাহ" },
//                    new TblDuration { Id = Guid.NewGuid(), Name = "৩ মাস" },
//                    new TblDuration { Id = Guid.NewGuid(), Name = "30" },
//                    new TblDuration { Id = Guid.NewGuid(), Name = "৪ সপ্তাহ" },
//                    new TblDuration { Id = Guid.NewGuid(), Name = "৪ মাস" },
//                    new TblDuration { Id = Guid.NewGuid(), Name = "০৫ দিন" },
//                    new TblDuration { Id = Guid.NewGuid(), Name = "০৬ দিন" },
//                    new TblDuration { Id = Guid.NewGuid(), Name = "৬ সপ্তাহ" },
//                    new TblDuration { Id = Guid.NewGuid(), Name = "০৭ দিন" },
//                    new TblDuration { Id = Guid.NewGuid(), Name = "৭ দিন" },
//                    new TblDuration { Id = Guid.NewGuid(), Name = "১ম এবং ৭ম দিন" },
//                    new TblDuration { Id = Guid.NewGuid(), Name = "রাতে ঘুমানোর ১ ঘণ্টা আগে" },
//                    new TblDuration { Id = Guid.NewGuid(), Name = "চলবে।" },
//                    new TblDuration { Id = Guid.NewGuid(), Name = "চলবে" },
//                    new TblDuration { Id = Guid.NewGuid(), Name = "পরবর্তী নির্দেশনা না দেওয়া পর্যন্ত চলবে।" }


//};

//                    // Insert the initial data into the table
//                    await _dbConnection.InsertAllAsync(initialDoses);
//                }
//            }

//            catch (Exception ex)
//            {

//                throw;
//            }

//        }
//    }
//}
