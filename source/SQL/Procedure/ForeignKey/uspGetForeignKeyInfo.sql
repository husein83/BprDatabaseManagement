USE master;
GO

IF OBJECT_ID(N'uspGetForeignKeyInfo', N'P') IS NOT NULL
    DROP PROCEDURE uspGetForeignKeyInfo;
GO

CREATE PROCEDURE uspGetForeignKeyInfo
    @DatabaseName NVARCHAR(128),
    @SchemaName   NVARCHAR(128) = NULL,
    @TableName    NVARCHAR(128) = NULL,
    @ColumnName   NVARCHAR(128) = NULL
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
        RAISERROR(N'[uspGetForeignKeyInfo] DatabaseName is required.', 16, 1);
        RETURN;
    END

    IF NOT EXISTS (SELECT 1 FROM sys.databases WHERE name = @DatabaseName)
    BEGIN
        RAISERROR(N'[uspGetForeignKeyInfo] Database [%s] does not exist.', 16, 1, @DatabaseName);
        RETURN;
    END

    -- ═══════════════════════════════════════════════════════════════════
    -- RETRIEVE FOREIGN KEY INFO
    -- ═══════════════════════════════════════════════════════════════════
    SET @SQL = N'
    SELECT
        ps.name                                AS SchemaName,
        pt.name                                AS TableName,
        pc.name                                AS ColumnName,
        fk.name                                AS ForeignKeyName,
        rs.name                                AS RefSchemaName,
        rt.name                                AS RefTableName,
        rc.name                                AS RefColumnName,
        fk.delete_referential_action_desc      AS OnDelete,
        fk.update_referential_action_desc      AS OnUpdate,
        fk.is_not_for_replication              AS IsNotForReplication,
        CASE WHEN fk.is_disabled = 0 THEN 1 ELSE 0 END AS IsEnabled,
        fk.create_date                         AS CreateDate,
        fk.modify_date                         AS ModifyDate
    FROM ' + QUOTENAME(@DatabaseName) + N'.sys.foreign_keys fk
    INNER JOIN ' + QUOTENAME(@DatabaseName) + N'.sys.foreign_key_columns fkc ON fk.object_id = fkc.constraint_object_id
    INNER JOIN ' + QUOTENAME(@DatabaseName) + N'.sys.tables pt ON fk.parent_object_id = pt.object_id
    INNER JOIN ' + QUOTENAME(@DatabaseName) + N'.sys.schemas ps ON pt.schema_id = ps.schema_id
    INNER JOIN ' + QUOTENAME(@DatabaseName) + N'.sys.columns pc ON fkc.parent_object_id = pc.object_id AND fkc.parent_column_id = pc.column_id
    INNER JOIN ' + QUOTENAME(@DatabaseName) + N'.sys.tables rt ON fk.referenced_object_id = rt.object_id
    INNER JOIN ' + QUOTENAME(@DatabaseName) + N'.sys.schemas rs ON rt.schema_id = rs.schema_id
    INNER JOIN ' + QUOTENAME(@DatabaseName) + N'.sys.columns rc ON fkc.referenced_object_id = rc.object_id AND fkc.referenced_column_id = rc.column_id
    WHERE 1=1';

    IF @SchemaName IS NOT NULL
        SET @SQL = @SQL + N' AND ps.name = @SchemaName';

    IF @TableName IS NOT NULL
        SET @SQL = @SQL + N' AND pt.name = @TableName';

    IF @ColumnName IS NOT NULL
        SET @SQL = @SQL + N' AND pc.name = @ColumnName';

    SET @SQL = @SQL + N' ORDER BY ps.name, pt.name, pc.name;';

    EXEC sp_executesql @SQL,
        N'@SchemaName NVARCHAR(128), @TableName NVARCHAR(128), @ColumnName NVARCHAR(128)',
        @SchemaName, @TableName, @ColumnName;

END
GO