USE master;
GO

IF OBJECT_ID(N'uspAlterForeignKey', N'P') IS NOT NULL
    DROP PROCEDURE uspAlterForeignKey;
GO

CREATE PROCEDURE uspAlterForeignKey
    @DatabaseName        NVARCHAR(128),
    @SchemaName          NVARCHAR(128),
    @TableName           NVARCHAR(128),
    @ColumnName          NVARCHAR(128),
    @RefSchemaName       NVARCHAR(128)   = NULL,
    @RefTableName        NVARCHAR(128)   = NULL,
    @RefColumnName       NVARCHAR(128)   = NULL,
    @OnDelete            NVARCHAR(20)    = NULL,
    @OnUpdate            NVARCHAR(20)    = NULL,
    @IsNotForReplication BIT             = NULL,
    @Enabled             BIT             = NULL
WITH ENCRYPTION
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @SQL                    NVARCHAR(MAX);
    DECLARE @FKName                 NVARCHAR(300);
    DECLARE @FKExists               BIT = 0;
    DECLARE @CurrentRefSchema       NVARCHAR(128);
    DECLARE @CurrentRefTable        NVARCHAR(128);
    DECLARE @CurrentRefColumn       NVARCHAR(128);
    DECLARE @CurrentOnDelete        NVARCHAR(20);
    DECLARE @CurrentOnUpdate        NVARCHAR(20);
    DECLARE @CurrentNotForRepl      BIT;
    DECLARE @CurrentEnabled         BIT;

    -- ═══════════════════════════════════════════════════════════════════
    -- VALIDATE
    -- ═══════════════════════════════════════════════════════════════════
    IF NULLIF(LTRIM(RTRIM(@DatabaseName)), N'') IS NULL OR
       NULLIF(LTRIM(RTRIM(@SchemaName)), N'') IS NULL OR
       NULLIF(LTRIM(RTRIM(@TableName)), N'') IS NULL OR
       NULLIF(LTRIM(RTRIM(@ColumnName)), N'') IS NULL
    BEGIN
        RAISERROR(N'[uspAlterForeignKey] DatabaseName, SchemaName, TableName and ColumnName are required.', 16, 1);
        RETURN;
    END

    IF @OnDelete IS NOT NULL AND @OnDelete NOT IN (N'NO ACTION', N'CASCADE', N'SET NULL', N'SET DEFAULT')
    BEGIN
        RAISERROR(N'[uspAlterForeignKey] Invalid OnDelete value. Allowed: NO ACTION, CASCADE, SET NULL, SET DEFAULT.', 16, 1);
        RETURN;
    END

    IF @OnUpdate IS NOT NULL AND @OnUpdate NOT IN (N'NO ACTION', N'CASCADE', N'SET NULL', N'SET DEFAULT')
    BEGIN
        RAISERROR(N'[uspAlterForeignKey] Invalid OnUpdate value. Allowed: NO ACTION, CASCADE, SET NULL, SET DEFAULT.', 16, 1);
        RETURN;
    END

    IF NOT EXISTS (SELECT 1 FROM sys.databases WHERE name = @DatabaseName)
    BEGIN
        RAISERROR(N'[uspAlterForeignKey] Database [%s] does not exist.', 16, 1, @DatabaseName);
        RETURN;
    END

    -- ═══════════════════════════════════════════════════════════════════
    -- FIND EXISTING FK
    -- ═══════════════════════════════════════════════════════════════════
    SET @SQL = N'
    SELECT TOP 1
        @FKName = fk.name,
        @CurrentRefSchema = rs.name,
        @CurrentRefTable = rt.name,
        @CurrentRefColumn = rc.name,
        @CurrentOnDelete = fk.delete_referential_action_desc,
        @CurrentOnUpdate = fk.update_referential_action_desc,
        @CurrentNotForRepl = fk.is_not_for_replication,
        @CurrentEnabled = fk.is_disabled
    FROM ' + QUOTENAME(@DatabaseName) + N'.sys.foreign_keys fk
    INNER JOIN ' + QUOTENAME(@DatabaseName) + N'.sys.foreign_key_columns fkc ON fk.object_id = fkc.constraint_object_id
    INNER JOIN ' + QUOTENAME(@DatabaseName) + N'.sys.tables pt ON fk.parent_object_id = pt.object_id
    INNER JOIN ' + QUOTENAME(@DatabaseName) + N'.sys.schemas ps ON pt.schema_id = ps.schema_id
    INNER JOIN ' + QUOTENAME(@DatabaseName) + N'.sys.columns pc ON fkc.parent_object_id = pc.object_id AND fkc.parent_column_id = pc.column_id
    INNER JOIN ' + QUOTENAME(@DatabaseName) + N'.sys.tables rt ON fk.referenced_object_id = rt.object_id
    INNER JOIN ' + QUOTENAME(@DatabaseName) + N'.sys.schemas rs ON rt.schema_id = rs.schema_id
    INNER JOIN ' + QUOTENAME(@DatabaseName) + N'.sys.columns rc ON fkc.referenced_object_id = rc.object_id AND fkc.referenced_column_id = rc.column_id
    WHERE ps.name = @SchemaName
      AND pt.name = @TableName
      AND pc.name = @ColumnName';

    EXEC sp_executesql @SQL,
        N'@SchemaName NVARCHAR(128), @TableName NVARCHAR(128), @ColumnName NVARCHAR(128),
          @FKName NVARCHAR(300) OUTPUT, @CurrentRefSchema NVARCHAR(128) OUTPUT, @CurrentRefTable NVARCHAR(128) OUTPUT,
          @CurrentRefColumn NVARCHAR(128) OUTPUT, @CurrentOnDelete NVARCHAR(20) OUTPUT, @CurrentOnUpdate NVARCHAR(20) OUTPUT,
          @CurrentNotForRepl BIT OUTPUT, @CurrentEnabled BIT OUTPUT',
        @SchemaName, @TableName, @ColumnName,
        @FKName OUTPUT, @CurrentRefSchema OUTPUT, @CurrentRefTable OUTPUT,
        @CurrentRefColumn OUTPUT, @CurrentOnDelete OUTPUT, @CurrentOnUpdate OUTPUT,
        @CurrentNotForRepl OUTPUT, @CurrentEnabled OUTPUT;

    IF @FKName IS NULL
    BEGIN
        RAISERROR(N'[uspAlterForeignKey] No foreign key found on column [%s].[%s].[%s].', 16, 1, @SchemaName, @TableName, @ColumnName);
        RETURN;
    END

    -- ═══════════════════════════════════════════════════════════════════
    -- MERGE PARAMETERS WITH CURRENT VALUES
    -- ═══════════════════════════════════════════════════════════════════
    SET @RefSchemaName       = ISNULL(@RefSchemaName, @CurrentRefSchema);
    SET @RefTableName        = ISNULL(@RefTableName, @CurrentRefTable);
    SET @RefColumnName       = ISNULL(@RefColumnName, @CurrentRefColumn);
    SET @OnDelete            = ISNULL(@OnDelete, @CurrentOnDelete);
    SET @OnUpdate            = ISNULL(@OnUpdate, @CurrentOnUpdate);
    SET @IsNotForReplication = ISNULL(@IsNotForReplication, @CurrentNotForRepl);
    SET @Enabled             = ISNULL(@Enabled, CASE WHEN @CurrentEnabled = 0 THEN 1 ELSE 0 END);

    -- ═══════════════════════════════════════════════════════════════════
    -- DROP EXISTING FK
    -- ═══════════════════════════════════════════════════════════════════
    SET @SQL = N'USE ' + QUOTENAME(@DatabaseName) + N';
                 ALTER TABLE ' + QUOTENAME(@SchemaName) + N'.' + QUOTENAME(@TableName) + N'
                 DROP CONSTRAINT ' + QUOTENAME(@FKName) + N';';
    EXEC sp_executesql @SQL;
    PRINT N'[uspAlterForeignKey] Existing FK dropped: [' + @FKName + N']';

    -- ═══════════════════════════════════════════════════════════════════
    -- RECREATE FK WITH NEW SETTINGS
    -- ═══════════════════════════════════════════════════════════════════
    SET @FKName = N'FK_' + @SchemaName + N'_' + @TableName + N'_' + @ColumnName + N'_2_' + @RefSchemaName + N'_' + @RefTableName + N'_' + @RefColumnName;

    SET @SQL = N'USE ' + QUOTENAME(@DatabaseName) + N';
                 ALTER TABLE ' + QUOTENAME(@SchemaName) + N'.' + QUOTENAME(@TableName) + N'
                 ADD CONSTRAINT ' + QUOTENAME(@FKName) + N'
                 FOREIGN KEY (' + QUOTENAME(@ColumnName) + N')
                 REFERENCES ' + QUOTENAME(@RefSchemaName) + N'.' + QUOTENAME(@RefTableName) + N' (' + QUOTENAME(@RefColumnName) + N')
                 ON DELETE ' + @OnDelete + N'
                 ON UPDATE ' + @OnUpdate;

    IF @IsNotForReplication = 1
        SET @SQL = @SQL + N' NOT FOR REPLICATION';

    SET @SQL = @SQL + N';';
    EXEC sp_executesql @SQL;
    PRINT N'[uspAlterForeignKey] ✓ FK recreated: [' + @FKName + N']';

    -- ═══════════════════════════════════════════════════════════════════
    -- ENABLE OR DISABLE FK
    -- ═══════════════════════════════════════════════════════════════════
    IF @Enabled = 0
    BEGIN
        SET @SQL = N'USE ' + QUOTENAME(@DatabaseName) + N';
                     ALTER TABLE ' + QUOTENAME(@SchemaName) + N'.' + QUOTENAME(@TableName) + N'
                     NOCHECK CONSTRAINT ' + QUOTENAME(@FKName) + N';';
        EXEC sp_executesql @SQL;
        PRINT N'[uspAlterForeignKey] FK disabled.';
    END
    ELSE
    BEGIN
        SET @SQL = N'USE ' + QUOTENAME(@DatabaseName) + N';
                     ALTER TABLE ' + QUOTENAME(@SchemaName) + N'.' + QUOTENAME(@TableName) + N'
                     WITH CHECK CHECK CONSTRAINT ' + QUOTENAME(@FKName) + N';';
        EXEC sp_executesql @SQL;
        PRINT N'[uspAlterForeignKey] FK enabled and validated.';
    END

END
GO