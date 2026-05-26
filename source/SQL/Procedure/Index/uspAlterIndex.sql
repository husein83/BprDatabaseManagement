USE master;
GO

IF OBJECT_ID(N'uspAlterIndex', N'P') IS NOT NULL
    DROP PROCEDURE uspAlterIndex;
GO

CREATE PROCEDURE uspAlterIndex
    @DatabaseName    NVARCHAR(128),
    @SchemaName      NVARCHAR(128),
    @TableName       NVARCHAR(128),
    @IndexName       NVARCHAR(128),
    @Columns         NVARCHAR(1000)  = NULL,
    @IncludeColumns  NVARCHAR(1000)  = NULL,
    @IsUnique        BIT             = NULL,
    @IndexType       NVARCHAR(20)    = NULL,
    @FillFactor      TINYINT         = NULL,
    @PadIndex        BIT             = NULL,
    @AllowRowLocks   BIT             = NULL,
    @AllowPageLocks  BIT             = NULL,
    @FilterPredicate NVARCHAR(500)   = NULL,
    @FileGroup       NVARCHAR(128)   = NULL
WITH ENCRYPTION
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @SQL            NVARCHAR(MAX);
    DECLARE @FullTable      NVARCHAR(300);
    DECLARE @IdxExists      BIT = 0;
    DECLARE @UniqueDef      NVARCHAR(10);
    
    -- Current values
    DECLARE @CurColumns         NVARCHAR(1000);
    DECLARE @CurIncludeColumns  NVARCHAR(1000);
    DECLARE @CurIsUnique        BIT;
    DECLARE @CurIndexType       NVARCHAR(20);
    DECLARE @CurFillFactor      TINYINT;
    DECLARE @CurPadIndex        BIT;
    DECLARE @CurAllowRowLocks   BIT;
    DECLARE @CurAllowPageLocks  BIT;
    DECLARE @CurFilterPredicate NVARCHAR(500);
    DECLARE @CurFileGroup       NVARCHAR(128);

    -- ═══════════════════════════════════════════════════════════════════
    -- VALIDATE
    -- ═══════════════════════════════════════════════════════════════════
    IF NULLIF(LTRIM(RTRIM(@DatabaseName)), N'') IS NULL OR
       NULLIF(LTRIM(RTRIM(@SchemaName)), N'') IS NULL OR
       NULLIF(LTRIM(RTRIM(@TableName)), N'') IS NULL OR
       NULLIF(LTRIM(RTRIM(@IndexName)), N'') IS NULL
    BEGIN
        RAISERROR(N'[uspAlterIndex] DatabaseName, SchemaName, TableName and IndexName are required.', 16, 1);
        RETURN;
    END

    IF @IndexType IS NOT NULL AND @IndexType NOT IN ('CLUSTERED','NONCLUSTERED')
    BEGIN
        RAISERROR(N'[uspAlterIndex] IndexType must be CLUSTERED or NONCLUSTERED.', 16, 1);
        RETURN;
    END

    IF NOT EXISTS (SELECT 1 FROM sys.databases WHERE name = @DatabaseName)
    BEGIN
        RAISERROR(N'[uspAlterIndex] Database [%s] does not exist.', 16, 1, @DatabaseName);
        RETURN;
    END

    IF @FillFactor IS NOT NULL AND (@FillFactor < 1 OR @FillFactor > 100)
    BEGIN
        RAISERROR(N'[uspAlterIndex] FillFactor must be between 1 and 100.', 16, 1);
        RETURN;
    END

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

    IF @IdxExists = 0 OR @IdxExists IS NULL
    BEGIN
        RAISERROR(N'[uspAlterIndex] Index [%s] does not exist on table [%s].[%s].', 16, 1, @IndexName, @SchemaName, @TableName);
        RETURN;
    END

    -- ═══════════════════════════════════════════════════════════════════
    -- GET CURRENT INDEX SETTINGS
    -- ═══════════════════════════════════════════════════════════════════
    SET @SQL = N'
    SELECT 
        @CurIsUnique        = i.is_unique,
        @CurIndexType       = CASE WHEN i.type = 1 THEN ''CLUSTERED'' ELSE ''NONCLUSTERED'' END,
        @CurFillFactor      = CASE WHEN i.fill_factor = 0 THEN 100 ELSE i.fill_factor END,
        @CurPadIndex        = i.is_padded,
        @CurAllowRowLocks   = i.allow_row_locks,
        @CurAllowPageLocks  = i.allow_page_locks,
        @CurFilterPredicate = i.filter_definition,
        @CurFileGroup       = fg.name,
        @CurColumns         = STUFF((
            SELECT '', '' + c.name + CASE WHEN ic.is_descending_key = 1 THEN '' DESC'' ELSE '' ASC'' END
            FROM ' + QUOTENAME(@DatabaseName) + N'.sys.index_columns ic
            INNER JOIN ' + QUOTENAME(@DatabaseName) + N'.sys.columns c ON ic.object_id = c.object_id AND ic.column_id = c.column_id
            WHERE ic.object_id = i.object_id AND ic.index_id = i.index_id AND ic.is_included_column = 0
            ORDER BY ic.key_ordinal
            FOR XML PATH(''''), TYPE).value(''.'', ''NVARCHAR(1000)''), 1, 2, ''''),
        @CurIncludeColumns  = STUFF((
            SELECT '', '' + c.name
            FROM ' + QUOTENAME(@DatabaseName) + N'.sys.index_columns ic
            INNER JOIN ' + QUOTENAME(@DatabaseName) + N'.sys.columns c ON ic.object_id = c.object_id AND ic.column_id = c.column_id
            WHERE ic.object_id = i.object_id AND ic.index_id = i.index_id AND ic.is_included_column = 1
            ORDER BY ic.index_column_id
            FOR XML PATH(''''), TYPE).value(''.'', ''NVARCHAR(1000)''), 1, 2, '''')
    FROM ' + QUOTENAME(@DatabaseName) + N'.sys.indexes i
    INNER JOIN ' + QUOTENAME(@DatabaseName) + N'.sys.tables t ON i.object_id = t.object_id
    INNER JOIN ' + QUOTENAME(@DatabaseName) + N'.sys.schemas s ON t.schema_id = s.schema_id
    LEFT JOIN ' + QUOTENAME(@DatabaseName) + N'.sys.filegroups fg ON i.data_space_id = fg.data_space_id
    WHERE i.name = @IndexName
      AND s.name = @SchemaName
      AND t.name = @TableName';

    EXEC sp_executesql @SQL,
        N'@IndexName NVARCHAR(128), @SchemaName NVARCHAR(128), @TableName NVARCHAR(128),
          @CurColumns NVARCHAR(1000) OUTPUT, @CurIncludeColumns NVARCHAR(1000) OUTPUT,
          @CurIsUnique BIT OUTPUT, @CurIndexType NVARCHAR(20) OUTPUT,
          @CurFillFactor TINYINT OUTPUT, @CurPadIndex BIT OUTPUT,
          @CurAllowRowLocks BIT OUTPUT, @CurAllowPageLocks BIT OUTPUT,
          @CurFilterPredicate NVARCHAR(500) OUTPUT, @CurFileGroup NVARCHAR(128) OUTPUT',
        @IndexName, @SchemaName, @TableName,
        @CurColumns OUTPUT, @CurIncludeColumns OUTPUT,
        @CurIsUnique OUTPUT, @CurIndexType OUTPUT,
        @CurFillFactor OUTPUT, @CurPadIndex OUTPUT,
        @CurAllowRowLocks OUTPUT, @CurAllowPageLocks OUTPUT,
        @CurFilterPredicate OUTPUT, @CurFileGroup OUTPUT;

    -- ═══════════════════════════════════════════════════════════════════
    -- MERGE WITH PROVIDED VALUES
    -- ═══════════════════════════════════════════════════════════════════
    SET @Columns         = ISNULL(@Columns, @CurColumns);
    SET @IncludeColumns  = ISNULL(@IncludeColumns, @CurIncludeColumns);
    SET @IsUnique        = ISNULL(@IsUnique, @CurIsUnique);
    SET @IndexType       = ISNULL(@IndexType, @CurIndexType);
    SET @FillFactor      = ISNULL(@FillFactor, @CurFillFactor);
    SET @PadIndex        = ISNULL(@PadIndex, @CurPadIndex);
    SET @AllowRowLocks   = ISNULL(@AllowRowLocks, @CurAllowRowLocks);
    SET @AllowPageLocks  = ISNULL(@AllowPageLocks, @CurAllowPageLocks);
    SET @FilterPredicate = ISNULL(@FilterPredicate, @CurFilterPredicate);
    SET @FileGroup       = ISNULL(@FileGroup, @CurFileGroup);

    -- ═══════════════════════════════════════════════════════════════════
    -- DROP EXISTING INDEX
    -- ═══════════════════════════════════════════════════════════════════
    SET @FullTable = QUOTENAME(@SchemaName) + N'.' + QUOTENAME(@TableName);
    SET @SQL = N'USE ' + QUOTENAME(@DatabaseName) + N';
                 DROP INDEX ' + QUOTENAME(@IndexName) + N' ON ' + @FullTable + N';';
    EXEC sp_executesql @SQL;

    -- ═══════════════════════════════════════════════════════════════════
    -- RECREATE INDEX WITH NEW SETTINGS
    -- ═══════════════════════════════════════════════════════════════════
    SET @UniqueDef = CASE WHEN @IsUnique = 1 THEN N'UNIQUE ' ELSE N'' END;

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
    PRINT N'[uspAlterIndex] ✓ Index altered: [' + @IndexName + N']';

END
GO