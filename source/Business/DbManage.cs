using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Microsoft.Data.SqlClient;
using DatabaseManagement.Models;
using DatabaseManagement.Utility;

namespace DatabaseManagement.Business
{
    internal class DbManage
    {
        #region Variables
        private readonly string _connectionString;
        private readonly int _commandTimeout;
        private readonly string _sqlScriptsPath;
        private readonly bool _autoCreateStoredProcedures;
        private readonly string _serverName;

        #endregion

        #region Constructor

        public DbManage(string? serverName)
        {
            _serverName = string.IsNullOrEmpty(serverName) == false ? serverName : AppSettings.GetDefaultServerName();

            var serverConfig = AppSettings.GetServerConfiguration(_serverName);

            _connectionString = serverConfig.BuildConnectionString();
            _commandTimeout = serverConfig.CommandTimeout;
            _autoCreateStoredProcedures = serverConfig.AutoCreateStoredProcedures;
            _sqlScriptsPath = AppDomain.CurrentDomain.BaseDirectory;
        }

        public DbManage(ServerConfiguration configuration)
        {
            _serverName = configuration.ServerName;
            _connectionString = configuration.BuildConnectionString();
            _commandTimeout = configuration.CommandTimeout;
            _autoCreateStoredProcedures = configuration.AutoCreateStoredProcedures;
            _sqlScriptsPath = AppDomain.CurrentDomain.BaseDirectory;
        }

        #endregion

        #region Public
        public string ServerName => _serverName;
        public string SqlScriptsPath => _sqlScriptsPath;
        public int CommandTimeout => _commandTimeout;
        public bool AutoCreateStoredProcedures => _autoCreateStoredProcedures;

        // Database Management
        public void CreateDatabase(DatabaseProcedure.Create config)
        {
            ExecuteStoredProcedure(config);
        }
        public void AlterDatabase(DatabaseProcedure.Alter config)
        {
            ExecuteStoredProcedure(config);
        }
        public void DropDatabase(DatabaseProcedure.Drop config)
        {
            ExecuteStoredProcedure(config);
        }
        public DataTable InformationDatabase(DatabaseProcedure.Information config)
        {
            return ExecuteQuery(config);
        }


        // Table Management
        public void CreateTable(TableProcedure.Create config)
        {
            ExecuteStoredProcedure(config);
        }
        public void AlterTable(TableProcedure.Alter config)
        {
            ExecuteStoredProcedure(config);
        }
        public void DropTable(TableProcedure.Drop config)
        {
            ExecuteStoredProcedure(config);
        }
        public DataTable InformationTable(TableProcedure.Information config)
        {
            return ExecuteQuery(config);
        }


        // Column Management
        public void CreateColumn(ColumnProcedure.Create config)
        {
            ExecuteStoredProcedure(config);
        }
        public void AlterColumn(ColumnProcedure.Alter config)
        {
            ExecuteStoredProcedure(config);
        }
        public void DropColumn(ColumnProcedure.Drop config)
        {
            ExecuteStoredProcedure(config);
        }
        public DataTable InformationColumn(ColumnProcedure.Information config)
        {
            return ExecuteQuery(config);
        }


        // Index Management
        public void CreateIndex(IndexProcedure.Create config)
        {
            ExecuteStoredProcedure(config);
        }
        public void AlterIndex(IndexProcedure.Alter config)
        {
            ExecuteStoredProcedure(config);
        }
        public void DropIndex(IndexProcedure.Drop config)
        {
            ExecuteStoredProcedure(config);
        }
        public DataTable InformationIndex(IndexProcedure.Information config)
        {
            return ExecuteQuery(config);
        }


        // Foreign Key Management
        public void CreateForeignKey(ForeignKeyProcedure.Create config)
        {
            ExecuteStoredProcedure(config);
        }
        public void AlterForeignKey(ForeignKeyProcedure.Alter config)
        {
            ExecuteStoredProcedure(config);
        }
        public void DropForeignKey(ForeignKeyProcedure.Drop config)
        {
            ExecuteStoredProcedure(config);
        }
        public DataTable InformationForeignKey(ForeignKeyProcedure.Information config)
        {
            return ExecuteQuery(config);
        }


        // Backup & Restore
        public void Backup(BackupProcedure config)
        {
            ExecuteStoredProcedure(config);
        }
        public void Restore(RestoreProcedure config)
        {
            ExecuteStoredProcedure(config);
        }


        // Get Data
        public List<string> GetDatabases()
        {
            var databases = new List<string>();

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand("SELECT name FROM sys.databases WHERE database_id > 4 ORDER BY name", connection);
                command.CommandTimeout = _commandTimeout;

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        databases.Add(reader.GetString(0));
                    }
                }
            }

            return databases;
        }
        public List<string> GetCollations()
        {
            var collations = new List<string>();

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand("SELECT name FROM sys.fn_helpcollations() ORDER BY name", connection);
                command.CommandTimeout = _commandTimeout;

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read()) collations.Add(reader.GetString(0));
                }
            }

            return collations;
        }
        public List<string> GetSchemas(string databaseName)
        {
            var schemas = new List<string>();

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                connection.ChangeDatabase(databaseName);

                var command = new SqlCommand("SELECT name FROM sys.schemas WHERE principal_id = 1 ORDER BY name;", connection);

                command.CommandTimeout = _commandTimeout;

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        schemas.Add(reader.GetString(0));
                    }
                }
            }

            return schemas;
        }
        public List<string> GetTables(string databaseName, string schemaName)
        {
            var tables = new List<string>();

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                connection.ChangeDatabase(databaseName);

                var command = new SqlCommand(@"
                SELECT t.name
                FROM sys.tables t
                INNER JOIN sys.schemas s ON t.schema_id = s.schema_id
                WHERE s.name = @SchemaName
                ORDER BY t.name;", connection);

                command.Parameters.AddWithValue("@SchemaName", schemaName);
                command.CommandTimeout = _commandTimeout;

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        tables.Add(reader.GetString(0));
                    }
                }
            }

            return tables;
        }
        public List<string> GetIndexNames(string databaseName, string schemaName, string tableName)
        {
            var indexNames = new List<string>();

            string query = @"
            USE [{0}];
            SELECT i.name
            FROM sys.indexes i
            INNER JOIN sys.objects o ON i.object_id = o.object_id
            INNER JOIN sys.schemas s ON o.schema_id = s.schema_id
            WHERE o.type = 'U'
              AND s.name = @SchemaName
              AND o.name = @TableName
              AND i.name IS NOT NULL
              AND i.is_primary_key = 0
            ORDER BY i.name";

            query = string.Format(query, databaseName);

            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@SchemaName", schemaName);
                command.Parameters.AddWithValue("@TableName", tableName);

                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        indexNames.Add(reader.GetString(0));
                    }
                }
            }

            return indexNames;
        }
        public List<string> GetColumns(string databaseName, string schemaName, string tableName)
        {
            var columns = new List<string>();

            string query = $@"
            USE [{databaseName}];
            SELECT c.name
            FROM sys.columns c
            INNER JOIN sys.objects o ON c.object_id = o.object_id
            INNER JOIN sys.schemas s ON o.schema_id = s.schema_id
            WHERE o.type = 'U'
              AND s.name = @SchemaName
              AND o.name = @TableName
            ORDER BY c.column_id";

            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.CommandTimeout = _commandTimeout;
                    cmd.Parameters.AddWithValue("@SchemaName", schemaName);
                    cmd.Parameters.AddWithValue("@TableName", tableName);

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            columns.Add(reader.GetString(0));
                        }
                    }
                }
            }

            return columns;
        }
        public List<string> GetDataTypes()
        {
            var dataTypes = new List<string>();

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                var command = new SqlCommand(@"
            SELECT name
            FROM sys.types
            WHERE is_user_defined = 0
            GROUP BY name
            ORDER BY name;",
                    connection);

                command.CommandTimeout = _commandTimeout;

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                        dataTypes.Add(reader.GetString(0));
                }
            }

            return dataTypes;
        }
        public List<string> GetFileGroups()
        {
            var fileGroups = new List<string>();

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                var command = new SqlCommand("SELECT name FROM sys.filegroups ORDER BY name;", connection);

                command.CommandTimeout = _commandTimeout;

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                        fileGroups.Add(reader.GetString(0));
                }
            }

            return fileGroups;
        }
        public List<string> GetIndexTypes()
        {
            var indexTypes = new List<string>
            {
                "NONCLUSTERED",
                "CLUSTERED",
                "UNIQUE CLUSTERED",
                "UNIQUE NONCLUSTERED",
                "COLUMNSTORE",
                "NONCLUSTERED COLUMNSTORE"
            };

            return indexTypes;
        }
        public List<string> GetReferentialActions()
        {
            return new List<string>
            {
                "NO ACTION",
                "CASCADE",
                "SET NULL",
                "SET DEFAULT"
            };
        }

        #endregion

        #region Private
        private bool StoredProcedureExists(string procedureName)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    conn.Open();
                    string query = @"
                        SELECT COUNT(*) 
                        FROM sys.procedures 
                        WHERE name = @ProcName";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@ProcName", procedureName);
                        int count = (int)cmd.ExecuteScalar();
                        return count > 0;
                    }
                }
            }
            catch
            {
                return false;
            }
        }
        private void ExecuteSqlFile(string sqlFileName)
        {
            string sqlFilePath = Path.Combine(_sqlScriptsPath, sqlFileName);

            if (!File.Exists(sqlFilePath))
                throw new FileNotFoundException($"SQL file not found: {sqlFilePath}");

            string sqlScript = File.ReadAllText(sqlFilePath);

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                var batches = System.Text.RegularExpressions.Regex.Split(
                    sqlScript,
                    @"^\s*GO\s*$",
                    System.Text.RegularExpressions.RegexOptions.IgnoreCase |
                    System.Text.RegularExpressions.RegexOptions.Multiline);

                foreach (string batch in batches)
                {
                    string trimmedBatch = batch.Trim();

                    if (string.IsNullOrWhiteSpace(trimmedBatch))
                        continue;

                    using (SqlCommand cmd = new SqlCommand(trimmedBatch, conn))
                    {
                        cmd.CommandTimeout = _commandTimeout;
                        cmd.ExecuteNonQuery();
                    }
                }
            }
        }

        public void ExecuteStoredProcedure(IProcedure procedure)
        {
            procedure.Validate();

            ExecuteStoredProcedure(
                procedure.GetProcedureName(),
                $"{procedure.GetProcedurePath()}\\{procedure.GetProcedureName()}.sql",
                cmd => procedure.AddParameters(cmd)
            );
        }
        private void ExecuteStoredProcedure(string procedureName, string sqlFileName, Action<SqlCommand> addParameters)
        {
            if (_autoCreateStoredProcedures && !StoredProcedureExists(procedureName))
            {
                ExecuteSqlFile(sqlFileName);
            }

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(procedureName, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = _commandTimeout;

                    addParameters?.Invoke(cmd);

                    cmd.ExecuteNonQuery();
                }
            }
        }
        public DataTable ExecuteQuery(IProcedure procedure)
        {
            if (_autoCreateStoredProcedures && !StoredProcedureExists(procedure.GetProcedureName()))
            {
                ExecuteSqlFile($"{procedure.GetProcedurePath()}\\{procedure.GetProcedureName()}.sql");
            }

            procedure.Validate();

            using var connection = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand(procedure.GetProcedureName(), connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            procedure.AddParameters(cmd);

            using var adapter = new SqlDataAdapter(cmd);
            var dataTable = new DataTable();

            connection.Open();
            adapter.Fill(dataTable);

            return dataTable;
        }
        #endregion
    }
}
