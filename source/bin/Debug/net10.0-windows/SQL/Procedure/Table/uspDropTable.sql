USE master;
GO

IF OBJECT_ID(N'uspDropTable', N'P') IS NOT NULL
    DROP PROCEDURE uspDropTable;
GO

CREATE PROCEDURE uspDropTable
    @DatabaseName       NVARCHAR(128),
    @SchemaName         NVARCHAR(128),
    @TableName          NVARCHAR(128),
    @IgnoreIfNotExists  BIT             = 1,
    @ForceDropDependent BIT             = 0
WITH ENCRYPTION
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @SQL       NVARCHAR(MAX);
    DECLARE @Exists    BIT = 0;
    DECLARE @FullTable NVARCHAR(300);

    -- Validation
    IF @DatabaseName IS NULL OR LTRIM(RTRIM(@DatabaseName)) = ''
    BEGIN RAISERROR('DatabaseName is required.', 16, 1); RETURN; END

    IF @SchemaName IS NULL OR LTRIM(RTRIM(@SchemaName)) = ''
    BEGIN RAISERROR('SchemaName is required.', 16, 1); RETURN; END

    IF @TableName IS NULL OR LTRIM(RTRIM(@TableName)) = ''
    BEGIN RAISERROR('TableName is required.', 16, 1); RETURN; END

    IF NOT EXISTS (SELECT 1 FROM sys.databases WHERE name = @DatabaseName)
    BEGIN RAISERROR('The specified database does not exist.', 16, 1); RETURN; END

    SET @FullTable = QUOTENAME(@SchemaName) + N'.' + QUOTENAME(@TableName);

    -- Check table exists
    SET @SQL = N'
        SELECT @E = 1
        FROM ' + QUOTENAME(@DatabaseName) + N'.sys.tables t
        INNER JOIN ' + QUOTENAME(@DatabaseName) + N'.sys.schemas s ON t.schema_id = s.schema_id
        WHERE t.name = ' + QUOTENAME(@TableName, '''') + N'
          AND s.name = ' + QUOTENAME(@SchemaName, '''') + N';';
    EXEC sp_executesql @SQL, N'@E BIT OUTPUT', @Exists OUTPUT;

    IF @Exists = 0
    BEGIN
        IF @IgnoreIfNotExists = 1
        BEGIN
            PRINT 'Table [' + @SchemaName + '.' + @TableName + '] does not exist. Skipping.';
            RETURN;
        END
        ELSE
        BEGIN
            RAISERROR('The specified table was not found.', 16, 1);
            RETURN;
        END
    END

    IF @ForceDropDependent = 1
    BEGIN
        -- Drop FK references pointing TO this table from other tables
        SET @SQL = N'
            USE ' + QUOTENAME(@DatabaseName) + N';
            DECLARE @fkName   NVARCHAR(128);
            DECLARE @fkSchema NVARCHAR(128);
            DECLARE @fkTable  NVARCHAR(128);
            DECLARE @dropSQL  NVARCHAR(500);

            DECLARE fk_cur CURSOR FOR
                SELECT fk.name,
                       sp.name AS parent_schema,
                       tp.name AS parent_table
                FROM sys.foreign_keys fk
                INNER JOIN sys.tables tr ON fk.referenced_object_id = tr.object_id
                INNER JOIN sys.schemas sr ON tr.schema_id = sr.schema_id
                INNER JOIN sys.tables tp ON fk.parent_object_id = tp.object_id
                INNER JOIN sys.schemas sp ON tp.schema_id = sp.schema_id
                WHERE tr.name = ' + QUOTENAME(@TableName, '''') + N'
                  AND sr.name = ' + QUOTENAME(@SchemaName, '''') + N';

            OPEN fk_cur;
            FETCH NEXT FROM fk_cur INTO @fkName, @fkSchema, @fkTable;

            WHILE @@FETCH_STATUS = 0
            BEGIN
                SET @dropSQL = N''ALTER TABLE '' + QUOTENAME(@fkSchema) + N''.'' + QUOTENAME(@fkTable) +
                               N'' DROP CONSTRAINT '' + QUOTENAME(@fkName);
                EXEC sp_executesql @dropSQL;
                PRINT ''FK dropped: '' + @fkName + '' from ['' + @fkSchema + ''.'' + @fkTable + '']'';
                FETCH NEXT FROM fk_cur INTO @fkName, @fkSchema, @fkTable;
            END

            CLOSE fk_cur;
            DEALLOCATE fk_cur;';
        EXEC sp_executesql @SQL;
    END

    -- Drop the table
    BEGIN TRY
        SET @SQL = N'DROP TABLE ' + QUOTENAME(@DatabaseName) + N'.' + @FullTable + N';';
        EXEC sp_executesql @SQL;
        PRINT 'Table [' + @SchemaName + '.' + @TableName + '] dropped successfully.';
    END TRY
    BEGIN CATCH
        DECLARE @ErrMsg NVARCHAR(4000) = ERROR_MESSAGE();
        RAISERROR('Failed to drop table: %s', 16, 1, @ErrMsg);
    END CATCH
END
GO