using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using DatabaseManagement.Models;

namespace DatabaseManagement.Business
{
    public class DbService
    {
        private readonly DbManage _DatabaseManager;

        public DbService(string serverName)
        {
            _DatabaseManager = new DbManage(serverName);
        }

        public DbService(ServerConfiguration configuration)
        {
            _DatabaseManager = new DbManage(configuration);
        }

        public OperationResult CreateDatabase(DatabaseProcedure.Create config)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(config.DatabaseName))
                    return OperationResult.Failure("Database name cannot be empty");

                if (string.IsNullOrWhiteSpace(config.DataFilePath))
                    return OperationResult.Failure("Data file path cannot be empty");

                if (string.IsNullOrWhiteSpace(config.LogFilePath))
                    config.LogFilePath = null;

                if (string.IsNullOrWhiteSpace(config.Collation))
                    config.Collation = null;

                _DatabaseManager.CreateDatabase(config);
                return OperationResult.Success($"Database '{config.DatabaseName}' created successfully");
            }
            catch (Exception ex)
            {
                return OperationResult.Failure($"Error creating database: {ex.Message}");
            }
        }
        public OperationResult AlterDatabase(DatabaseProcedure.Alter config)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(config.DatabaseName))
                    return OperationResult.Failure("Database name cannot be empty");

                if (string.IsNullOrWhiteSpace(config.Collation))
                    config.Collation = null;

                _DatabaseManager.AlterDatabase(config);
                return OperationResult.Success($"Database '{config.DatabaseName}' altered successfully");
            }
            catch (Exception ex)
            {
                return OperationResult.Failure($"Error altering database: {ex.Message}");
            }
        }
        public OperationResult DropDatabase(DatabaseProcedure.Drop config)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(config.DatabaseName))
                    return OperationResult.Failure("Database name cannot be empty");

                _DatabaseManager.DropDatabase(config);
                return OperationResult.Success($"Database '{config.DatabaseName}' dropped successfully");
            }
            catch (Exception ex)
            {
                return OperationResult.Failure($"Error dropping database: {ex.Message}");
            }
        }
        public (OperationResult operationResult, DataTable? information) GetDatabaseInformation(DatabaseProcedure.Information config)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(config.DatabaseName))
                    return (OperationResult.Failure("Database name cannot be empty"), null);

                var results = _DatabaseManager.InformationDatabase(config);

                if (results == null)
                    return (OperationResult.Failure("No database information found"), null);

                return (OperationResult.Success($"Retrieved information for {results.Columns.Count} database(s)"), results);
            }
            catch (Exception ex)
            {
                return (OperationResult.Failure($"Error retrieving database information: {ex.Message}"), null);
            }
        }

        public OperationResult CreateTable(TableProcedure.Create config)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(config.DatabaseName))
                    return OperationResult.Failure("Database name cannot be empty");

                if (string.IsNullOrWhiteSpace(config.SchemaName))
                    return OperationResult.Failure("Schema name cannot be empty");

                if (string.IsNullOrWhiteSpace(config.TableName))
                    return OperationResult.Failure("Table name cannot be empty");

                if (string.IsNullOrWhiteSpace(config.FileGroup))
                    config.FileGroup = "PRIMARY";

                _DatabaseManager.CreateTable(config);
                return OperationResult.Success($"Table '{config.SchemaName}.{config.TableName}' created successfully");
            }
            catch (Exception ex)
            {
                return OperationResult.Failure($"Error creating table: {ex.Message}");
            }
        }
        public OperationResult AlterTable(TableProcedure.Alter config)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(config.DatabaseName))
                    return OperationResult.Failure("Database name cannot be empty");

                if (string.IsNullOrWhiteSpace(config.SchemaName))
                    return OperationResult.Failure("Schema name cannot be empty");

                if (string.IsNullOrWhiteSpace(config.TableName))
                    return OperationResult.Failure("Table name cannot be empty");

                _DatabaseManager.AlterTable(config);
                return OperationResult.Success($"Table '{config.SchemaName}.{config.TableName}' altered successfully");
            }
            catch (Exception ex)
            {
                return OperationResult.Failure($"Error altering table: {ex.Message}");
            }
        }
        public OperationResult DropTable(TableProcedure.Drop config)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(config.DatabaseName))
                    return OperationResult.Failure("Database name cannot be empty");

                if (string.IsNullOrWhiteSpace(config.SchemaName))
                    return OperationResult.Failure("Schema name cannot be empty");

                if (string.IsNullOrWhiteSpace(config.TableName))
                    return OperationResult.Failure("Table name cannot be empty");

                _DatabaseManager.DropTable(config);
                return OperationResult.Success($"Table '{config.SchemaName}.{config.TableName}' dropped successfully");
            }
            catch (Exception ex)
            {
                return OperationResult.Failure($"Error dropping table: {ex.Message}");
            }
        }
        public (OperationResult operationResult, DataTable? information) GetTableInformation(TableProcedure.Information config)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(config.DatabaseName))
                    return (OperationResult.Failure("Database name cannot be empty"), null);

                if (string.IsNullOrWhiteSpace(config.SchemaName))
                    return (OperationResult.Failure("Schema name cannot be empty"), null);

                if (string.IsNullOrWhiteSpace(config.TableName))
                    return (OperationResult.Failure("Table name cannot be empty"), null);

                var results = _DatabaseManager.InformationTable(config);

                if (results == null)
                    return (OperationResult.Failure("No table information found"), null);

                return (OperationResult.Success($"Retrieved information for {results.Rows.Count} table(s)"), results);
            }
            catch (Exception ex)
            {
                return (OperationResult.Failure($"Error retrieving table information: {ex.Message}"), null);
            }
        }

        public OperationResult CreateColumn(ColumnProcedure.Create config)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(config.DatabaseName))
                    return OperationResult.Failure("Database name cannot be empty");

                if (string.IsNullOrWhiteSpace(config.SchemaName))
                    return OperationResult.Failure("Schema name cannot be empty");

                if (string.IsNullOrWhiteSpace(config.TableName))
                    return OperationResult.Failure("Table name cannot be empty");

                if (string.IsNullOrWhiteSpace(config.ColumnName))
                    return OperationResult.Failure("Column name cannot be empty");

                if (string.IsNullOrWhiteSpace(config.DataType))
                    return OperationResult.Failure("Data type cannot be empty");

                _DatabaseManager.CreateColumn(config);
                return OperationResult.Success($"Column '{config.ColumnName}' created successfully in table '{config.SchemaName}.{config.TableName}'");
            }
            catch (Exception ex)
            {
                return OperationResult.Failure($"Error creating column: {ex.Message}");
            }
        }
        public OperationResult AlterColumn(ColumnProcedure.Alter config)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(config.DatabaseName))
                    return OperationResult.Failure("Database name cannot be empty");

                if (string.IsNullOrWhiteSpace(config.SchemaName))
                    return OperationResult.Failure("Schema name cannot be empty");

                if (string.IsNullOrWhiteSpace(config.TableName))
                    return OperationResult.Failure("Table name cannot be empty");

                if (string.IsNullOrWhiteSpace(config.ColumnName))
                    return OperationResult.Failure("Column name cannot be empty");

                _DatabaseManager.AlterColumn(config);
                return OperationResult.Success($"Column '{config.ColumnName}' altered successfully in table '{config.SchemaName}.{config.TableName}'");
            }
            catch (Exception ex)
            {
                return OperationResult.Failure($"Error altering column: {ex.Message}");
            }
        }
        public OperationResult DropColumn(ColumnProcedure.Drop config)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(config.DatabaseName))
                    return OperationResult.Failure("Database name cannot be empty");

                if (string.IsNullOrWhiteSpace(config.SchemaName))
                    return OperationResult.Failure("Schema name cannot be empty");

                if (string.IsNullOrWhiteSpace(config.TableName))
                    return OperationResult.Failure("Table name cannot be empty");

                if (string.IsNullOrWhiteSpace(config.ColumnName))
                    return OperationResult.Failure("Column name cannot be empty");

                _DatabaseManager.DropColumn(config);
                return OperationResult.Success($"Column '{config.ColumnName}' dropped successfully from table '{config.SchemaName}.{config.TableName}'");
            }
            catch (Exception ex)
            {
                return OperationResult.Failure($"Error dropping column: {ex.Message}");
            }
        }
        public (OperationResult operationResult, DataTable? information) GetColumnInformation(ColumnProcedure.Information config)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(config.DatabaseName))
                    return (OperationResult.Failure("Database name cannot be empty"), null);

                if (string.IsNullOrWhiteSpace(config.SchemaName))
                    return (OperationResult.Failure("Schema name cannot be empty"), null);

                if (string.IsNullOrWhiteSpace(config.TableName))
                    return (OperationResult.Failure("Table name cannot be empty"), null);

                if (string.IsNullOrWhiteSpace(config.ColumnName))
                    return (OperationResult.Failure("Column name cannot be empty"), null);

                var results = _DatabaseManager.InformationColumn(config);

                if (results == null)
                    return (OperationResult.Failure("No column information found"), null);

                return (OperationResult.Success($"Retrieved information for {results.Rows.Count} column(s)"), results);
            }
            catch (Exception ex)
            {
                return (OperationResult.Failure($"Error retrieving column information: {ex.Message}"), null);
            }
        }

        public OperationResult CreateForeignKey(ForeignKeyProcedure.Create config)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(config.DatabaseName))
                    return OperationResult.Failure("Database name cannot be empty");

                if (string.IsNullOrWhiteSpace(config.SchemaName))
                    return OperationResult.Failure("Schema name cannot be empty");

                if (string.IsNullOrWhiteSpace(config.TableName))
                    return OperationResult.Failure("Table name cannot be empty");

                if (string.IsNullOrWhiteSpace(config.ColumnName))
                    return OperationResult.Failure("Column name cannot be empty");

                if (string.IsNullOrWhiteSpace(config.RefSchemaName))
                    return OperationResult.Failure("Referenced schema name cannot be empty");

                if (string.IsNullOrWhiteSpace(config.RefTableName))
                    return OperationResult.Failure("Referenced table name cannot be empty");

                if (string.IsNullOrWhiteSpace(config.RefColumnName))
                    return OperationResult.Failure("Referenced column name cannot be empty");

                _DatabaseManager.CreateForeignKey(config);
                return OperationResult.Success($"Foreign key created successfully on table '{config.SchemaName}.{config.TableName}'");
            }
            catch (Exception ex)
            {
                return OperationResult.Failure($"Error creating foreign key: {ex.Message}");
            }
        }
        public OperationResult AlterForeignKey(ForeignKeyProcedure.Alter config)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(config.DatabaseName))
                    return OperationResult.Failure("Database name cannot be empty");

                if (string.IsNullOrWhiteSpace(config.SchemaName))
                    return OperationResult.Failure("Schema name cannot be empty");

                if (string.IsNullOrWhiteSpace(config.TableName))
                    return OperationResult.Failure("Table name cannot be empty");

                if (string.IsNullOrWhiteSpace(config.ColumnName))
                    return OperationResult.Failure("Column name cannot be empty");

                _DatabaseManager.AlterForeignKey(config);
                return OperationResult.Success($"Foreign key altered successfully on table '{config.SchemaName}.{config.TableName}'");
            }
            catch (Exception ex)
            {
                return OperationResult.Failure($"Error altering foreign key: {ex.Message}");
            }
        }
        public OperationResult DropForeignKey(ForeignKeyProcedure.Drop config)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(config.DatabaseName))
                    return OperationResult.Failure("Database name cannot be empty");

                if (string.IsNullOrWhiteSpace(config.SchemaName))
                    return OperationResult.Failure("Schema name cannot be empty");

                if (string.IsNullOrWhiteSpace(config.TableName))
                    return OperationResult.Failure("Table name cannot be empty");

                _DatabaseManager.DropForeignKey(config);
                return OperationResult.Success($"Foreign key dropped successfully from table '{config.SchemaName}.{config.TableName}'");
            }
            catch (Exception ex)
            {
                return OperationResult.Failure($"Error dropping foreign key: {ex.Message}");
            }
        }
        public (OperationResult operationResult, DataTable? information) GetForeignKeyInformation(ForeignKeyProcedure.Information config)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(config.DatabaseName))
                    return (OperationResult.Failure("Database name cannot be empty"), null);

                if (string.IsNullOrWhiteSpace(config.SchemaName))
                    return (OperationResult.Failure("Schema name cannot be empty"), null);

                if (string.IsNullOrWhiteSpace(config.TableName))
                    return (OperationResult.Failure("Table name cannot be empty"), null);

                if (string.IsNullOrWhiteSpace(config.ColumnName))
                    return (OperationResult.Failure("Column name cannot be empty"), null);

                var results = _DatabaseManager.InformationForeignKey(config);

                if (results == null)
                    return (OperationResult.Failure("No foreign key information found"), null);

                return (OperationResult.Success($"Retrieved information for {results.Rows.Count} foreign key(s)"), results);
            }
            catch (Exception ex)
            {
                return (OperationResult.Failure($"Error retrieving foreign key information: {ex.Message}"), null);
            }
        }

        public OperationResult CreateIndex(IndexProcedure.Create config)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(config.DatabaseName))
                    return OperationResult.Failure("Database name cannot be empty");

                if (string.IsNullOrWhiteSpace(config.SchemaName))
                    return OperationResult.Failure("Schema name cannot be empty");

                if (string.IsNullOrWhiteSpace(config.TableName))
                    return OperationResult.Failure("Table name cannot be empty");

                if (string.IsNullOrWhiteSpace(config.Columns))
                    return OperationResult.Failure("Columns cannot be empty");

                _DatabaseManager.CreateIndex(config);
                return OperationResult.Success($"Index created successfully on table '{config.SchemaName}.{config.TableName}'");
            }
            catch (Exception ex)
            {
                return OperationResult.Failure($"Error creating index: {ex.Message}");
            }
        }
        public OperationResult AlterIndex(IndexProcedure.Alter config)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(config.DatabaseName))
                    return OperationResult.Failure("Database name cannot be empty");

                if (string.IsNullOrWhiteSpace(config.SchemaName))
                    return OperationResult.Failure("Schema name cannot be empty");

                if (string.IsNullOrWhiteSpace(config.TableName))
                    return OperationResult.Failure("Table name cannot be empty");

                if (string.IsNullOrWhiteSpace(config.IndexName))
                    return OperationResult.Failure("Index name cannot be empty");

                _DatabaseManager.AlterIndex(config);
                return OperationResult.Success($"Index '{config.IndexName}' altered successfully on table '{config.SchemaName}.{config.TableName}'");
            }
            catch (Exception ex)
            {
                return OperationResult.Failure($"Error altering index: {ex.Message}");
            }
        }
        public OperationResult DropIndex(IndexProcedure.Drop config)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(config.DatabaseName))
                    return OperationResult.Failure("Database name cannot be empty");

                if (string.IsNullOrWhiteSpace(config.SchemaName))
                    return OperationResult.Failure("Schema name cannot be empty");

                if (string.IsNullOrWhiteSpace(config.TableName))
                    return OperationResult.Failure("Table name cannot be empty");

                if (string.IsNullOrWhiteSpace(config.IndexName))
                    return OperationResult.Failure("Index name cannot be empty");

                _DatabaseManager.DropIndex(config);
                return OperationResult.Success($"Index '{config.IndexName}' dropped successfully from table '{config.SchemaName}.{config.TableName}'");
            }
            catch (Exception ex)
            {
                return OperationResult.Failure($"Error dropping index: {ex.Message}");
            }
        }
        public (OperationResult operationResult, DataTable? information) GetIndexInformation(IndexProcedure.Information config)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(config.DatabaseName))
                    return (OperationResult.Failure("Database name cannot be empty"), null);

                if (string.IsNullOrWhiteSpace(config.SchemaName))
                    return (OperationResult.Failure("Schema name cannot be empty"), null);

                if (string.IsNullOrWhiteSpace(config.TableName))
                    return (OperationResult.Failure("Table name cannot be empty"), null);

                if (string.IsNullOrWhiteSpace(config.IndexName))
                    return (OperationResult.Failure("Index name cannot be empty"), null);

                var results = _DatabaseManager.InformationIndex(config);

                if (results == null)
                    return (OperationResult.Failure("No index information found"), null);

                return (OperationResult.Success($"Retrieved information for {results.Rows.Count} index(es)"), results);
            }
            catch (Exception ex)
            {
                return (OperationResult.Failure($"Error retrieving index information: {ex.Message}"), null);
            }
        }

        public OperationResult BackupDatabase(BackupProcedure config)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(config.DatabaseName))
                    return OperationResult.Failure("Database name cannot be empty");

                if (string.IsNullOrWhiteSpace(config.BackupPath))
                    return OperationResult.Failure("Backup path cannot be empty");

                _DatabaseManager.Backup(config);
                return OperationResult.Success($"Database '{config.DatabaseName}' backed up successfully to '{config.BackupPath}'");
            }
            catch (Exception ex)
            {
                return OperationResult.Failure($"Error: {ex.Message}");
            }
        }
        public OperationResult RestoreDatabase(RestoreProcedure config)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(config.DatabaseName))
                    return OperationResult.Failure("Database name cannot be empty");

                if (string.IsNullOrWhiteSpace(config.RestorePath))
                    return OperationResult.Failure("Restore path cannot be empty");

                _DatabaseManager.Restore(config);
                return OperationResult.Success($"Database '{config.DatabaseName}' restored successfully from '{config.RestorePath}'");
            }
            catch (Exception ex)
            {
                return OperationResult.Failure($"Error: {ex.Message}");
            }
        }

        public (OperationResult operationResult, List<string> databases) GetDatabases()
        {
            try
            {
                var databases = _DatabaseManager.GetDatabases();
                return (OperationResult.Success("Get Databases successfully"), databases);
            }
            catch (Exception ex)
            {
                return (OperationResult.Failure($"Error getting databases: {ex.Message}"), new List<string>());
            }
        }
        public (OperationResult operationResult, List<string> collations) GetCollations()
        {
            try
            {
                var collations = _DatabaseManager.GetCollations();
                return (OperationResult.Success("Get Collations successfully"), collations);
            }
            catch (Exception ex)
            {
                return (OperationResult.Failure($"Error getting collations: {ex.Message}"), new List<string>());
            }
        }
        public (OperationResult operationResult, List<string> schemas) GetSchemas(string databaseName)
        {
            try
            {
                var schemas = _DatabaseManager.GetSchemas(databaseName);
                return (OperationResult.Success("Get Schemas successfully"), schemas);
            }
            catch (Exception ex)
            {
                return (OperationResult.Failure($"Error getting schemas: {ex.Message}"), new List<string>());
            }
        }
        public (OperationResult operationResult, List<string> tables) GetTables(string databaseName, string schemaName)
        {
            try
            {
                var tables = _DatabaseManager.GetTables(databaseName, schemaName);
                return (OperationResult.Success("Get Tables successfully"), tables);
            }
            catch (Exception ex)
            {
                return (OperationResult.Failure($"Error getting tables: {ex.Message}"), new List<string>());
            }
        }
        public (OperationResult operationResult, List<string> indexNames) GetIndexNames(string databaseName, string schemaName, string tableName)
        {
            try
            {
                var indexNames = _DatabaseManager.GetIndexNames(databaseName, schemaName, tableName);
                return (OperationResult.Success("Get IndexNames successfully"), indexNames);
            }
            catch (Exception ex)
            {
                return (OperationResult.Failure($"Error getting indexNames: {ex.Message}"), new List<string>());
            }
        }
        public (OperationResult operationResult, List<string> columns) GetColumns(string databaseName, string schemaName, string tableName)
        {
            try
            {
                var columns = _DatabaseManager.GetColumns(databaseName, schemaName, tableName);
                return (OperationResult.Success("Get Columns successfully"), columns);
            }
            catch (Exception ex)
            {
                return (OperationResult.Failure($"Error getting columns: {ex.Message}"), new List<string>());
            }
        }
        public (OperationResult operationResult, List<string> dataTypes) GetDataTypes()
        {
            try
            {
                var dataTypes = _DatabaseManager.GetDataTypes();
                return (OperationResult.Success("Get DataTypes successfully"), dataTypes);
            }
            catch (Exception ex)
            {
                return (OperationResult.Failure($"Error getting dataTypes: {ex.Message}"), new List<string>());
            }
        }
        public (OperationResult operationResult, List<string> fileGroups) GetFileGroups()
        {
            try
            {
                var fileGroups = _DatabaseManager.GetFileGroups();
                return (OperationResult.Success("Get FileGroups successfully"), fileGroups);
            }
            catch (Exception ex)
            {
                return (OperationResult.Failure($"Error getting fileGroups: {ex.Message}"), new List<string>());
            }
        }
        public (OperationResult operationResult, List<string> indexTypes) GetIndexTypes()
        {
            try
            {
                var indexTypes = _DatabaseManager.GetIndexTypes();
                return (OperationResult.Success("Get IndexTypes successfully"), indexTypes);
            }
            catch (Exception ex)
            {
                return (OperationResult.Failure($"Error getting indexTypes: {ex.Message}"), new List<string>());
            }
        }
        public (OperationResult operationResult, List<string> referentialActions) GetReferentialActions()
        {
            try
            {
                var referentialActions = _DatabaseManager.GetReferentialActions();
                return (OperationResult.Success("Get Referential Actions successfully"), referentialActions);
            }
            catch (Exception ex)
            {
                return (OperationResult.Failure($"Error getting referential actions: {ex.Message}"), new List<string>());
            }
        }
    }
}
