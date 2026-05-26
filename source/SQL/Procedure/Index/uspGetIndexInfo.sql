USE master;
GO

IF OBJECT_ID(N'uspGetIndexInfo', N'P') IS NOT NULL
    DROP PROCEDURE uspGetIndexInfo;
GO

CREATE PROCEDURE uspGetIndexInfo
    @DatabaseName NVARCHAR(128),
    @SchemaName   NVARCHAR(128) = NULL,
    @TableName    NVARCHAR(128) = NULL,
    @IndexName    NVARCHAR(128) = NULL
WITH ENCRYPTION
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @SQL NVARCHAR(MAX);

    -- ═══════════════════════════════════════════════════════════════════
    -- VALIDATE
    -- ═══════════════════════════════════════════════════════════════════
    IF NULLIF(LTRIM(RTRIM(@DatabaseName)), N'') IS NULL
    BEGIN
        RAISERROR(N'[uspGetIndexInfo] DatabaseName is required.', 16, 1);
        RETURN;
    END

    IF NOT EXISTS (SELECT 1 FROM sys.databases WHERE name = @DatabaseName)
    BEGIN
        RAISERROR(N'[uspGetIndexInfo] Database [%s] does not exist.', 16, 1, @DatabaseName);
        RETURN;
    END

    -- ═══════════════════════════════════════════════════════════════════
    -- BUILD QUERY
    -- ═══════════════════════════════════════════════════════════════════
    SET @SQL = N'
    SELECT 
        s.name                  AS SchemaName,
        t.name                  AS TableName,
        i.name                  AS IndexName,
        i.type_desc             AS IndexType,
        i.is_unique             AS IsUnique,
        i.is_primary_key        AS IsPrimaryKey,
        i.is_unique_constraint  AS IsUniqueConstraint,
        CASE WHEN i.fill_factor = 0 THEN 100 ELSE i.fill_factor END AS [FillFactor],
        i.is_padded             AS [PadIndex],
        i.allow_row_locks       AS [AllowRowLocks],
        i.allow_page_locks      AS [AllowPageLocks],
        i.filter_definition     AS [FilterPredicate],
        fg.name                 AS [FileGroup],
        STUFF((
            SELECT '', '' + c.name + CASE WHEN ic.is_descending_key = 1 THEN '' DESC'' ELSE '' ASC'' END
            FROM ' + QUOTENAME(@DatabaseName) + N'.sys.index_columns ic
            INNER JOIN ' + QUOTENAME(@DatabaseName) + N'.sys.columns c ON ic.object_id = c.object_id AND ic.column_id = c.column_id
            WHERE ic.object_id = i.object_id AND ic.index_id = i.index_id AND ic.is_included_column = 0
            ORDER BY ic.key_ordinal
            FOR XML PATH(''''), TYPE).value(''.'', ''NVARCHAR(1000)''), 1, 2, '''') AS [Columns],
        STUFF((
            SELECT '', '' + c.name
            FROM ' + QUOTENAME(@DatabaseName) + N'.sys.index_columns ic
            INNER JOIN ' + QUOTENAME(@DatabaseName) + N'.sys.columns c ON ic.object_id = c.object_id AND ic.column_id = c.column_id
            WHERE ic.object_id = i.object_id AND ic.index_id = i.index_id AND ic.is_included_column = 1
            ORDER BY ic.index_column_id
            FOR XML PATH(''''), TYPE).value(''.'', ''NVARCHAR(1000)''), 1, 2, '''') AS IncludeColumns
    FROM ' + QUOTENAME(@DatabaseName) + N'.sys.indexes i
    INNER JOIN ' + QUOTENAME(@DatabaseName) + N'.sys.tables t ON i.object_id = t.object_id
    INNER JOIN ' + QUOTENAME(@DatabaseName) + N'.sys.schemas s ON t.schema_id = s.schema_id
    LEFT JOIN ' + QUOTENAME(@DatabaseName) + N'.sys.filegroups fg ON i.data_space_id = fg.data_space_id
    WHERE i.type > 0'; -- Exclude heap

    IF @SchemaName IS NOT NULL
        SET @SQL = @SQL + N' AND s.name = @SchemaName';
    
    IF @TableName IS NOT NULL
        SET @SQL = @SQL + N' AND t.name = @TableName';
    
    IF @IndexName IS NOT NULL
        SET @SQL = @SQL + N' AND i.name = @IndexName';

    SET @SQL = @SQL + N' ORDER BY s.name, t.name, i.name;';

    -- ═══════════════════════════════════════════════════════════════════
    -- EXECUTE
    -- ═══════════════════════════════════════════════════════════════════
    EXEC sp_executesql @SQL,
        N'@SchemaName NVARCHAR(128), @TableName NVARCHAR(128), @IndexName NVARCHAR(128)',
        @SchemaName, @TableName, @IndexName;

END
GO