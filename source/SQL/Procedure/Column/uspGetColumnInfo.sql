USE master;
GO

IF OBJECT_ID(N'uspGetColumnInfo', N'P') IS NOT NULL
    DROP PROCEDURE uspGetColumnInfo;
GO

CREATE PROCEDURE uspGetColumnInfo
    @DatabaseName NVARCHAR(128),
    @SchemaName   NVARCHAR(128),
    @TableName    NVARCHAR(128),
    @ColumnName   NVARCHAR(128) = NULL
WITH ENCRYPTION
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @SQL NVARCHAR(MAX);

    IF NULLIF(LTRIM(RTRIM(@DatabaseName)), N'') IS NULL OR
       NULLIF(LTRIM(RTRIM(@SchemaName)), N'') IS NULL OR
       NULLIF(LTRIM(RTRIM(@TableName)), N'') IS NULL
    BEGIN
        RAISERROR(N'DatabaseName, SchemaName, and TableName are required.', 16, 1);
        RETURN;
    END

    IF NOT EXISTS (SELECT 1 FROM sys.databases WHERE name = @DatabaseName)
    BEGIN
        RAISERROR(N'Database [%s] does not exist.', 16, 1, @DatabaseName);
        RETURN;
    END

    SET @SQL = N'
        USE [' + @DatabaseName + N'];
        SELECT
            s.name                  AS SchemaName,
            t.name                  AS TableName,
            c.name                  AS ColumnName,
            c.column_id             AS ColumnId,
            TYPE_NAME(c.user_type_id) AS DataType,
            c.max_length            AS MaxLength,
            c.precision             AS [Precision],
            c.scale                 AS Scale,
            c.is_nullable           AS IsNullable,
            c.is_identity           AS IsIdentity,
            IDENT_SEED(QUOTENAME(s.name) + ''.'' + QUOTENAME(t.name)) AS IdentitySeed,
            IDENT_INCR(QUOTENAME(s.name) + ''.'' + QUOTENAME(t.name)) AS IdentityIncrement,
            c.is_computed           AS IsComputed,
            cc.definition           AS ComputedFormula,
            cc.is_persisted         AS IsPersisted,
            dc.name                 AS DefaultConstraintName,
            dc.definition           AS DefaultValue,
            c.collation_name        AS Collation,
            CASE WHEN pk.column_id IS NOT NULL THEN 1 ELSE 0 END AS IsPrimaryKey,
            pk.name                 AS PrimaryKeyName,
            ep.value                AS Description
        FROM sys.columns c
        JOIN sys.tables t ON c.object_id = t.object_id
        JOIN sys.schemas s ON t.schema_id = s.schema_id
        LEFT JOIN sys.computed_columns cc ON c.object_id = cc.object_id AND c.column_id = cc.column_id
        LEFT JOIN sys.default_constraints dc ON c.object_id = dc.parent_object_id AND c.column_id = dc.parent_column_id
        LEFT JOIN (
            SELECT ic.object_id, ic.column_id, kc.name
            FROM sys.key_constraints kc
            JOIN sys.index_columns ic ON kc.parent_object_id = ic.object_id AND kc.unique_index_id = ic.index_id
            WHERE kc.type = ''PK''
        ) pk ON c.object_id = pk.object_id AND c.column_id = pk.column_id
        LEFT JOIN sys.extended_properties ep ON ep.major_id = c.object_id
                                              AND ep.minor_id = c.column_id
                                              AND ep.name = N''MS_Description''
        WHERE t.name = ' + QUOTENAME(@TableName, '''') + N'
          AND s.name = ' + QUOTENAME(@SchemaName, '''');

    IF @ColumnName IS NOT NULL
        SET @SQL = @SQL + N' AND c.name = ' + QUOTENAME(@ColumnName, '''');

    SET @SQL = @SQL + N' ORDER BY c.column_id;';

    EXEC sp_executesql @SQL;
END
GO