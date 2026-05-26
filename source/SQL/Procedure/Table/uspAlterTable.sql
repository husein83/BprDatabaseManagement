USE master;
GO

IF OBJECT_ID(N'uspAlterTable', N'P') IS NOT NULL
    DROP PROCEDURE uspAlterTable;
GO

CREATE PROCEDURE uspAlterTable
    @DatabaseName   NVARCHAR(128),
    @SchemaName     NVARCHAR(128),
    @TableName      NVARCHAR(128),
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

    -- Check if table exists
    SET @SQL = N'
        SELECT @Exists = 1
        FROM ' + QUOTENAME(@DatabaseName) + N'.sys.tables t
        INNER JOIN ' + QUOTENAME(@DatabaseName) + N'.sys.schemas s ON t.schema_id = s.schema_id
        WHERE t.name = ' + QUOTENAME(@TableName, '''') + N'
          AND s.name = ' + QUOTENAME(@SchemaName, '''');
    EXEC sp_executesql @SQL, N'@Exists BIT OUTPUT', @TableExists OUTPUT;

    IF @TableExists = 0
    BEGIN
        RAISERROR('Table does not exist. Use uspCreateTable to create it.', 16, 1);
        RETURN;
    END

    PRINT 'Table [' + @SchemaName + '.' + @TableName + '] exists.';
    PRINT 'Note: FileGroup cannot be changed after table creation.';

    -- Update Extended Property (Description)
    IF @Description IS NOT NULL
    BEGIN
        SET @SQL = N'
            USE ' + QUOTENAME(@DatabaseName) + N';
            IF EXISTS (
                SELECT 1 FROM sys.extended_properties
                WHERE major_id = OBJECT_ID(' + QUOTENAME(QUOTENAME(@SchemaName) + '.' + QUOTENAME(@TableName), '''') + N')
                  AND name = N''MS_Description''
                  AND minor_id = 0
            )
                EXEC sys.sp_updateextendedproperty
                    @name  = N''MS_Description'',
                    @value = ' + QUOTENAME(@Description, '''') + N',
                    @level0type = N''SCHEMA'', @level0name = ' + QUOTENAME(@SchemaName, '''') + N',
                    @level1type = N''TABLE'',  @level1name = ' + QUOTENAME(@TableName, '''') + N';
            ELSE
                EXEC sys.sp_addextendedproperty
                    @name  = N''MS_Description'',
                    @value = ' + QUOTENAME(@Description, '''') + N',
                    @level0type = N''SCHEMA'', @level0name = ' + QUOTENAME(@SchemaName, '''') + N',
                    @level1type = N''TABLE'',  @level1name = ' + QUOTENAME(@TableName, '''') + N';';
        EXEC sp_executesql @SQL;
        PRINT 'Table description updated.';
    END
END
GO