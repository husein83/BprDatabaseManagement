USE master;
GO

IF OBJECT_ID(N'uspCreateIndex', N'P') IS NOT NULL
    DROP PROCEDURE uspCreateIndex;
GO

CREATE PROCEDURE uspCreateIndex
    @DatabaseName    NVARCHAR(128),
    @SchemaName      NVARCHAR(128),
    @TableName       NVARCHAR(128),
    @IndexName       NVARCHAR(128)   = NULL,
    @Columns         NVARCHAR(1000),
    @IncludeColumns  NVARCHAR(1000)  = NULL,
    @IsUnique        BIT             = 0,
    @IndexType       NVARCHAR(20)    = 'NONCLUSTERED',
    @FillFactor      TINYINT         = 80,
    @PadIndex        BIT             = 1,
    @AllowRowLocks   BIT             = 1,
    @AllowPageLocks  BIT             = 1,
    @FilterPredicate NVARCHAR(500)   = NULL,
    @FileGroup       NVARCHAR(128)   = 'PRIMARY'
WITH ENCRYPTION
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @SQL       NVARCHAR(MAX);
    DECLARE @FullTable NVARCHAR(300);
    DECLARE @IdxExists BIT = 0;
    DECLARE @UniqueDef NVARCHAR(10);

    -- ═══════════════════════════════════════════════════════════════════
    -- VALIDATE
    -- ═══════════════════════════════════════════════════════════════════
    IF NULLIF(LTRIM(RTRIM(@DatabaseName)), N'') IS NULL OR
       NULLIF(LTRIM(RTRIM(@SchemaName)), N'') IS NULL OR
       NULLIF(LTRIM(RTRIM(@TableName)), N'') IS NULL OR
       NULLIF(LTRIM(RTRIM(@Columns)), N'') IS NULL
    BEGIN
        RAISERROR(N'[uspCreateIndex] DatabaseName, SchemaName, TableName and Columns are required.', 16, 1);
        RETURN;
    END

    IF @IndexType NOT IN ('CLUSTERED','NONCLUSTERED')
    BEGIN
        RAISERROR(N'[uspCreateIndex] IndexType must be CLUSTERED or NONCLUSTERED.', 16, 1);
        RETURN;
    END

    IF NOT EXISTS (SELECT 1 FROM sys.databases WHERE name = @DatabaseName)
    BEGIN
        RAISERROR(N'[uspCreateIndex] Database [%s] does not exist.', 16, 1, @DatabaseName);
        RETURN;
    END

    IF @FillFactor < 1 OR @FillFactor > 100
    BEGIN
        RAISERROR(N'[uspCreateIndex] FillFactor must be between 1 and 100.', 16, 1);
        RETURN;
    END

    -- ═══════════════════════════════════════════════════════════════════
    -- AUTO-GENERATE INDEX NAME
    -- ═══════════════════════════════════════════════════════════════════
    IF @IndexName IS NULL
    BEGIN
        DECLARE @ColShort NVARCHAR(100) = LEFT(REPLACE(REPLACE(REPLACE(@Columns,' ',''),'ASC',''),'DESC',''), 80);
        SET @ColShort   = REPLACE(@ColShort, ',', '_');
        SET @IndexName  = CASE WHEN @IsUnique = 1 THEN N'UIX_' ELSE N'IX_' END
                        + @TableName + N'_' + @ColShort;
    END

    SET @FullTable  = QUOTENAME(@SchemaName) + N'.' + QUOTENAME(@TableName);
    SET @UniqueDef  = CASE WHEN @IsUnique = 1 THEN N'UNIQUE ' ELSE N'' END;

    -- ═══════════════════════════════════════════════════════════════════
    -- CHECK IF INDEX EXISTS
    -- ═══════════════════════════════════════════════════════════════════
    SET @SQL = N'SELECT @E = 1 FROM ' + QUOTENAME(@DatabaseName) + N'.sys.indexes i
                 INNER JOIN ' + QUOTENAME(@DatabaseName) + N'.sys.tables t ON i.object_id = t.object_id
                 INNER JOIN ' + QUOTENAME(@DatabaseName) + N'.sys.schemas s ON t.schema_id = s.schema_id
                 WHERE i.name = @IndexName
                   AND s.name = @SchemaName
                   AND t.name = @TableName';
    
    EXEC sp_executesql @SQL, 
        N'@IndexName NVARCHAR(128), @SchemaName NVARCHAR(128), @TableName NVARCHAR(128), @E BIT OUTPUT',
        @IndexName, @SchemaName, @TableName, @IdxExists OUTPUT;

    IF @IdxExists = 1
    BEGIN
        RAISERROR(N'[uspCreateIndex] Index [%s] already exists on table [%s].[%s].', 16, 1, @IndexName, @SchemaName, @TableName);
        RETURN;
    END

    -- ═══════════════════════════════════════════════════════════════════
    -- BUILD CREATE INDEX STATEMENT
    -- ═══════════════════════════════════════════════════════════════════
    SET @SQL = N'USE ' + QUOTENAME(@DatabaseName) + N';
                 CREATE ' + @UniqueDef + @IndexType + N' INDEX ' + QUOTENAME(@IndexName) + N'
                 ON ' + @FullTable + N' (' + @Columns + N')';

    IF @IncludeColumns IS NOT NULL
        SET @SQL = @SQL + N' INCLUDE (' + @IncludeColumns + N')';

    IF @FilterPredicate IS NOT NULL
        SET @SQL = @SQL + N' WHERE ' + @FilterPredicate;

    SET @SQL = @SQL + N'
                        WITH (
                            FILLFACTOR             = ' + CAST(@FillFactor AS NVARCHAR(5)) + N',
                            PAD_INDEX              = ' + CASE WHEN @PadIndex=1       THEN 'ON' ELSE 'OFF' END + N',
                            ALLOW_ROW_LOCKS        = ' + CASE WHEN @AllowRowLocks=1  THEN 'ON' ELSE 'OFF' END + N',
                            ALLOW_PAGE_LOCKS       = ' + CASE WHEN @AllowPageLocks=1 THEN 'ON' ELSE 'OFF' END + N',
                            ONLINE                 = OFF,
                            SORT_IN_TEMPDB         = OFF,
                            STATISTICS_NORECOMPUTE = OFF
                        )
                        ON ' + QUOTENAME(@FileGroup) + N';';

    EXEC sp_executesql @SQL;
    PRINT N'[uspCreateIndex] ✓ Index created: [' + @IndexName + N']';

END
GO