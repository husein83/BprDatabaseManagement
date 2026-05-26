USE master;
GO

IF OBJECT_ID(N'uspDropIndex', N'P') IS NOT NULL
    DROP PROCEDURE uspDropIndex;
GO

CREATE PROCEDURE uspDropIndex
    @DatabaseName      NVARCHAR(128),
    @SchemaName        NVARCHAR(128),
    @TableName         NVARCHAR(128),
    @IndexName         NVARCHAR(128),
    @IgnoreIfNotExists BIT             = 1
WITH ENCRYPTION
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @SQL    NVARCHAR(MAX);
    DECLARE @Exists BIT = 0;

    -- ═══════════════════════════════════════════════════════════════════
    -- VALIDATE
    -- ═══════════════════════════════════════════════════════════════════
    IF NULLIF(LTRIM(RTRIM(@DatabaseName)), N'') IS NULL OR
       NULLIF(LTRIM(RTRIM(@SchemaName)), N'') IS NULL OR
       NULLIF(LTRIM(RTRIM(@TableName)), N'') IS NULL OR
       NULLIF(LTRIM(RTRIM(@IndexName)), N'') IS NULL
    BEGIN
        RAISERROR(N'[uspDropIndex] DatabaseName, SchemaName, TableName and IndexName are required.', 16, 1);
        RETURN;
    END

    IF NOT EXISTS (SELECT 1 FROM sys.databases WHERE name = @DatabaseName)
    BEGIN
        RAISERROR(N'[uspDropIndex] Database [%s] does not exist.', 16, 1, @DatabaseName);
        RETURN;
    END

    -- ═══════════════════════════════════════════════════════════════════
    -- CHECK INDEX EXISTS
    -- ═══════════════════════════════════════════════════════════════════
    SET @SQL = N'
    SELECT @E = 1
    FROM ' + QUOTENAME(@DatabaseName) + N'.sys.indexes i
    INNER JOIN ' + QUOTENAME(@DatabaseName) + N'.sys.tables t ON i.object_id = t.object_id
    INNER JOIN ' + QUOTENAME(@DatabaseName) + N'.sys.schemas s ON t.schema_id = s.schema_id
    WHERE i.name = @IndexName
      AND s.name = @SchemaName
      AND t.name = @TableName
      AND i.is_primary_key = 0
      AND i.is_unique_constraint = 0';

    EXEC sp_executesql @SQL,
        N'@IndexName NVARCHAR(128), @SchemaName NVARCHAR(128), @TableName NVARCHAR(128), @E BIT OUTPUT',
        @IndexName, @SchemaName, @TableName, @E = @Exists OUTPUT;

    IF @Exists = 0 OR @Exists IS NULL
    BEGIN
        IF @IgnoreIfNotExists = 1
        BEGIN
            PRINT N'[uspDropIndex] Index [' + @IndexName + N'] does not exist. Skipping.';
            RETURN;
        END
        ELSE
        BEGIN
            RAISERROR(N'[uspDropIndex] Index [%s] does not exist on table [%s].[%s].', 16, 1, @IndexName, @SchemaName, @TableName);
            RETURN;
        END
    END

    -- ═══════════════════════════════════════════════════════════════════
    -- DROP INDEX
    -- ═══════════════════════════════════════════════════════════════════
    SET @SQL = N'USE ' + QUOTENAME(@DatabaseName) + N';
                 DROP INDEX ' + QUOTENAME(@IndexName) + N'
                 ON ' + QUOTENAME(@SchemaName) + N'.' + QUOTENAME(@TableName) + N';';

    EXEC sp_executesql @SQL;
    PRINT N'[uspDropIndex] ✓ Index dropped: [' + @IndexName + N']';

END
GO