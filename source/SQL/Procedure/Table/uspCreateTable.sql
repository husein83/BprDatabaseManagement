USE master;
GO

IF OBJECT_ID(N'uspCreateTable', N'P') IS NOT NULL
    DROP PROCEDURE uspCreateTable;
GO

CREATE PROCEDURE uspCreateTable
    @DatabaseName   NVARCHAR(128),
    @SchemaName     NVARCHAR(128),
    @TableName      NVARCHAR(128),
    @FileGroup      NVARCHAR(128)   = 'PRIMARY',
    @Description    NVARCHAR(500)   = NULL
WITH ENCRYPTION
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @SQL         NVARCHAR(MAX);
    DECLARE @TableExists BIT = 0;

    -- Validations
    IF @DatabaseName IS NULL OR LTRIM(RTRIM(@DatabaseName)) = ''
    BEGIN RAISERROR('DatabaseName is required.', 16, 1); RETURN; END

    IF @SchemaName IS NULL OR LTRIM(RTRIM(@SchemaName)) = ''
    BEGIN RAISERROR('SchemaName is required.', 16, 1); RETURN; END

    IF @TableName IS NULL OR LTRIM(RTRIM(@TableName)) = ''
    BEGIN RAISERROR('TableName is required.', 16, 1); RETURN; END

    IF NOT EXISTS (SELECT 1 FROM sys.databases WHERE name = @DatabaseName)
    BEGIN RAISERROR('The specified database does not exist.', 16, 1); RETURN; END

    -- Ensure schema exists
    SET @SQL = N'
        IF NOT EXISTS (SELECT 1 FROM ' + QUOTENAME(@DatabaseName) + N'.sys.schemas WHERE name = ' + QUOTENAME(@SchemaName, '''') + N')
        BEGIN
            EXEC ' + QUOTENAME(@DatabaseName) + N'..sp_executesql N''CREATE SCHEMA ' + QUOTENAME(@SchemaName) + N''';
            PRINT ''Schema [' + @SchemaName + N'] created.'';
        END';
    EXEC sp_executesql @SQL;

    -- Check if table exists
    SET @SQL = N'
        SELECT @Exists = 1
        FROM ' + QUOTENAME(@DatabaseName) + N'.sys.tables t
        INNER JOIN ' + QUOTENAME(@DatabaseName) + N'.sys.schemas s ON t.schema_id = s.schema_id
        WHERE t.name = ' + QUOTENAME(@TableName, '''') + N'
          AND s.name = ' + QUOTENAME(@SchemaName, '''');
    EXEC sp_executesql @SQL, N'@Exists BIT OUTPUT', @TableExists OUTPUT;

    IF @TableExists = 1
    BEGIN
        RAISERROR('Table already exists. Use uspAlterTable to modify it.', 16, 1);
        RETURN;
    END

    -- Create table
    SET @SQL = N'
        USE ' + QUOTENAME(@DatabaseName) + N';
        CREATE TABLE ' + QUOTENAME(@SchemaName) + N'.' + QUOTENAME(@TableName) + N'
        (
            Id INT,
            CONSTRAINT PK_' + @SchemaName + N'_' + @TableName + N'_Id PRIMARY KEY (Id)
        ) ON ' + QUOTENAME(@FileGroup) + N';';
    EXEC sp_executesql @SQL;
    PRINT 'Table [' + @SchemaName + '.' + @TableName + '] created successfully.';

    -- Extended Property (Description)
    IF @Description IS NOT NULL
    BEGIN
        SET @SQL = N'
            USE ' + QUOTENAME(@DatabaseName) + N';
            EXEC sys.sp_addextendedproperty
                @name  = N''MS_Description'',
                @value = ' + QUOTENAME(@Description, '''') + N',
                @level0type = N''SCHEMA'', @level0name = ' + QUOTENAME(@SchemaName, '''') + N',
                @level1type = N''TABLE'',  @level1name = ' + QUOTENAME(@TableName, '''') + N';';
        EXEC sp_executesql @SQL;
        PRINT 'Table description saved.';
    END
END
GO