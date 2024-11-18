
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using QuickMed.DB;

namespace QuickMed.Services
{
    public class DataSyncService
    {
        private readonly ApplicationDbContext _localDbContext;
        private readonly ILogger<DataSyncService> _logger;
        private readonly string _sqlServerConnectionString =
            "Server=203.26.151.51;User Id=sa;Password=Asdf019lkjh;Database=QuickDB;Persist Security Info=true;TrustServerCertificate=true;Trusted_Connection=false;";
        private readonly System.Timers.Timer _timer;

        public DataSyncService(ApplicationDbContext localDbContext, ILogger<DataSyncService> logger)
        {
            _localDbContext = localDbContext;
            _logger = logger;

            // Initialize a Timer with full namespace to resolve ambiguity
            _timer = new System.Timers.Timer
            {
                Interval = TimeSpan.FromMinutes(5).TotalMilliseconds,
                //Interval = TimeSpan.FromSeconds(20).TotalMilliseconds,
                AutoReset = true
            };
            _timer.Elapsed += OnTimerElapsed;
        }

        public void Start()
        {
            _logger.LogInformation("DataSyncService started.");
            _timer.Start();
        }

        private async void OnTimerElapsed(object sender, System.Timers.ElapsedEventArgs e)

        {
            _logger.LogInformation("Checking for internet connection...");
            if (await IsConnectedToInternet())
            {
                _logger.LogInformation("Internet is available, starting sync...");

                _ = Task.Run(() => SyncTableDataAsync<TblPatient>());
                _ = Task.Run(() => SyncTableDataAsync<DrugDosage>());
                _ = Task.Run(() => SyncTableDataAsync<DrugGeneric>());
                _ = Task.Run(() => SyncTableDataAsync<DrugManufacturer>());
                _ = Task.Run(() => SyncTableDataAsync<DrugType>());
                _ = Task.Run(() => SyncTableDataAsync<DrugMedicine>());
                _ = Task.Run(() => SyncTableDataAsync<TblPrescription>());
            }
            else
            {
                _logger.LogInformation("No internet connection, skipping sync.");
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

        private async Task SyncTableDataAsync<TEntity>() where TEntity : class, new()
        {
            try
            {
                var tblName = typeof(TEntity);
                var unsyncedRecords = await _localDbContext._dbConnection.Table<TEntity>().ToListAsync();
                var filteredRecords = unsyncedRecords
                    .Where(record => (bool)(record.GetType().GetProperty("IsSynced")?.GetValue(record) ?? true) == false)
                    .ToList();

                if (filteredRecords.Any())
                {
                    using var sqlConnection = new SqlConnection(_sqlServerConnectionString);
                    foreach (var record in filteredRecords)
                    {
                        var sqlQuery = GenerateInsertOrUpdateQuery(record);
                        await sqlConnection.ExecuteAsync(sqlQuery, record);

                        record.GetType().GetProperty("IsSynced")?.SetValue(record, true);
                        await _localDbContext.UpdateAsync(record);
                    }
                    _logger.LogInformation($"{filteredRecords.Count} records synced for {typeof(TEntity).Name}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error syncing data for {typeof(TEntity).Name}: {ex.Message}");
            }
        }

        private string GenerateInsertOrUpdateQuery<TEntity>(TEntity entity)
        {
            // Get the type of the entity and properties, excluding Id and IsSynced
            var entityType = typeof(TEntity);
            var properties = entityType.GetProperties()
                .Where(p => p.Name != "IsSynced" && p.Name != "Id") // Exclude IsSynced and Id
                .ToList();

            // Generate column names and parameters for both the INSERT and UPDATE statements
            var updateSetClause = string.Join(", ", properties.Select(p => $"Target.{p.Name} = Source.{p.Name}"));
            var insertColumns = string.Join(", ", properties.Select(p => $"{p.Name}"));
            var insertValues = string.Join(", ", properties.Select(p => $"Source.{p.Name}"));

            // Create the list of columns and parameters for the USING clause in SQL syntax
            var sourceColumns = string.Join(", ", properties.Select(p => $"@{p.Name} AS {p.Name}"));

            // Construct the SQL MERGE statement
            var sqlQuery = $@"
        MERGE INTO {entityType.Name} AS Target
        USING (SELECT @Id AS Id, {sourceColumns}) AS Source
        ON Target.Id = Source.Id
        WHEN MATCHED THEN
            UPDATE SET {updateSetClause}, Target.IsSynced = 1
        WHEN NOT MATCHED THEN
            INSERT (Id, {insertColumns}, IsSynced)
            VALUES (Source.Id, {insertValues}, 1);
    ";

            return sqlQuery;
        }


        public void Stop()
        {
            _logger.LogInformation("DataSyncService stopping...");
            _timer.Stop();
        }


        public async Task SyncSqlToLiteAsync<TEntity>(string createdByCondition) where TEntity : class, new()
        {
            try
            {
                using var sqlConnection = new SqlConnection(_sqlServerConnectionString);

                // Get the table name from the entity type
                var tableName = typeof(TEntity).Name;

                // Prepare a SQL query to get records based on the CreatedBy condition
                var query = $"SELECT * FROM {tableName} WHERE CreatedBy = @CreatedBy";

                // Execute the query with the condition
                var records = await sqlConnection.QueryAsync<TEntity>(query, new { CreatedBy = createdByCondition });

                // Retrieve all existing records in the local database to perform comparison in memory
                var localRecords = await _localDbContext._dbConnection.Table<TEntity>().ToListAsync();

                // Process each record and save it locally
                foreach (var record in records)
                {
                    // Get the record's ID
                    var recordId = record.GetType().GetProperty("Id")?.GetValue(record);

                    // Check if a matching record exists in the local database by comparing IDs
                    var existingRecord = localRecords.FirstOrDefault(localRecord =>
                        localRecord.GetType().GetProperty("Id")?.GetValue(localRecord).Equals(recordId) == true);

                    if (existingRecord == null)
                    {
                        // Insert new record data if no matching record was found
                        await _localDbContext._dbConnection.InsertAsync(record);
                    }
                    else
                    {
                        // Update existing record data if needed
                        await _localDbContext._dbConnection.UpdateAsync(record);
                    }
                }

                _logger.LogInformation($"Synced {records.Count()} records of {tableName} with CreatedBy = '{createdByCondition}' from SQL Server to local SQLite.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error syncing data for {typeof(TEntity).Name}: {ex.Message}");
            }
        }



    }
}

